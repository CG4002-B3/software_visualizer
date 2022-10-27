using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using uPLibrary.Networking.M2Mqtt;
using uPLibrary.Networking.M2Mqtt.Messages;
using M2MqttUnity;
using TMPro;
using SimpleJSON;

public class UnityMqttClient : M2MqttUnityClient
{
    // UI Elements
    public TextMeshProUGUI status;
    public TextMeshProUGUI messageText;

    public InvalidActionFeedbackController invalidActionFeedbackController;

    public Text selfPlayerId;

    public BulletController selfBulletController;
    public GrenadeController selfGrenadeController;
    public SelfShieldController selfShieldController;
    public SelfScoreController selfScoreController;
    public SelfHealthBarController selfHealthBarController;

    public OppShieldController oppShieldController;
    public OppHealthBarController oppHealthBarController;

    // Broker Configurations
    public bool autoTest = false;
    public string topicSubscribe = "to_phone";
    public string topicPublish = "from_phone";
    public string msgPublish = "{grenade_throw: 1}";
    public List<string> eventMessages = new List<string>();

    private bool checkingGrenadeHit = false;
    private string selfIdString;
    private string oppIdString;

    public void TestPublish()
    {
        client.Publish(topicPublish, System.Text.Encoding.UTF8.GetBytes(msgPublish), MqttMsgBase.QOS_LEVEL_EXACTLY_ONCE, false);
        Debug.Log("[MQTT TEST] Publish test");
    }

    public void SetStatus(string msg)
    {
        status.text = msg;
    }

    public void DisplayMessage(string msg)
    {
        messageText.text = msg;
    }

    private void StoreMessage(string newMsg)
    {
        eventMessages.Add(newMsg);
        Debug.Log("[MQTT MESSAGE QUEUE] New message in queue: " + newMsg);
    }

    // Connection status
    protected override void OnConnecting()
    {
        Debug.Log("[MQTT CONNECTION] Connecting to " + brokerAddress + ", " + brokerPort);
        SetStatus("Connecting");
    }

    protected override void OnConnected()
    {
        Debug.Log("[MQTT CONNECTION] Connected to " + brokerAddress + ", " + brokerPort);
        SetStatus("Connected");
        SubscribeTopics();
    }

    protected override void OnConnectionFailed(string errorMessage)
    {
        Debug.Log("[MQTT ERROR] Connection failed: " + errorMessage);
        SetStatus("Failed Connection");
    }

    protected override void OnDisconnected()
    {
        Debug.Log("[MQTT CONNECTION] Disconnected from " + brokerAddress);
        SetStatus("Disconnected");
    }

    protected override void OnConnectionLost()
    {
        Debug.Log("[MQTT ERROR] Connection lost!");
        SetStatus("Connection Lost");
    }

    // Topic Subscription
    protected override void SubscribeTopics()
    {
        client.Subscribe(new string[] { topicSubscribe }, new byte[] { MqttMsgBase.QOS_LEVEL_EXACTLY_ONCE });
        Debug.Log("[MQTT SUBSCRIPTION] Subscribed to " + topicSubscribe);
        SetStatus("Subscribed");
    }

    protected override void UnsubscribeTopics()
    {
        client.Unsubscribe(new string[] { topicSubscribe });
        Debug.Log("[MQTT SUBSCRIPTION] Unsubscribed to " + topicSubscribe);
        SetStatus("Unsubscribed");
    }

    public void Publish()
    {
        client.Publish(topicPublish, System.Text.Encoding.UTF8.GetBytes(msgPublish));
        Debug.Log("[MQTT PUBLISH] Publish message to " + topicPublish);
        SetStatus("Publish");
    }

    protected void setSelfInventoryFromGameEngine(JSONNode msgDict)
    {
        selfBulletController.SetBulletsRemaining(int.Parse(msgDict[selfIdString]["bullets"]), false);
        selfGrenadeController.SetGrenadesRemaining(int.Parse(msgDict[selfIdString]["grenades"]), false);
        selfShieldController.SetShieldRemaining(int.Parse(msgDict[selfIdString]["num_shield"]), false);
    }

    protected void setSelfScoreFromGameEngine(JSONNode msgDict)
    {
        selfScoreController.SetNumKills(int.Parse(msgDict[oppIdString]["num_deaths"]));
        selfScoreController.SetNumDeaths(int.Parse(msgDict[selfIdString]["num_deaths"]));
    }

    protected void setBothPlayersHpFromGameEngine(JSONNode msgDict)
    {
        selfHealthBarController.SetHealthRemaining(int.Parse(msgDict[selfIdString]["hp"]));
        oppHealthBarController.SetHealthRemaining(int.Parse(msgDict[oppIdString]["hp"]));
    }

    protected override void DecodeMessage(string topic, byte[] message)
    {
        string msg = System.Text.Encoding.UTF8.GetString(message);
        var msgDict = JSON.Parse(msg);
        Debug.Log("[TYPE] " + msgDict.GetType());
        Debug.Log("[MQTT RECEIVED] Received new message: " + msg);
        SetStatus("Received");
        StoreMessage(msg);

        if (int.Parse(msgDict["game_engine_update"]) == 1)
        {
            setSelfInventoryFromGameEngine(msgDict);
            setBothPlayersHpFromGameEngine(msgDict);
            setSelfScoreFromGameEngine(msgDict);

            return;
        }

        string selfAction = msgDict[selfIdString]["action"];
        bool selfActionValid = int.Parse(msgDict[selfIdString]["action_valid"]) == 1;
        bool shouldUpdateHp = int.Parse(msgDict[selfIdString]["should_update_hp"]) == 1;

        int selfShieldHp = int.Parse(msgDict[selfIdString]["shield_health"]);
        int oppShieldHp = int.Parse(msgDict[oppIdString]["shield_health"]);

        float oppShieldTime = float.Parse(msgDict[oppIdString]["shield_time"]);

        bool selfIsValidGrenade = selfAction == "grenade" && selfActionValid;
        bool selfIsValidReload = selfAction == "reload" && selfActionValid;
        bool selfIsValidShoot = selfAction == "shoot" && int.Parse(msgDict[selfIdString]["bullets"]) > 0;
        bool selfIsValidShield = selfAction == "shield" && selfActionValid;

        if (oppShieldTime == 10f)
        {
            oppShieldController.ActivateShield();
        }

        if (selfShieldHp == 0)
        {
            selfShieldController.DeactivateShield();
        }

        if (oppShieldHp == 0)
        {
            oppShieldController.DeactivateShield();
        }

        if (selfAction == "logout")
        {
            invalidActionFeedbackController.SetFeedback("Game Over");
            return;
        }

        if (!selfActionValid)
        {
            if (selfAction == "grenade" && shouldUpdateHp && !checkingGrenadeHit)
            {
                invalidActionFeedbackController.SetFeedback("Invalid Grenade Action");
            }
            else if (selfAction == "reload")
            {
                invalidActionFeedbackController.SetFeedback("Invalid Reload Action");
            }
            else if (selfAction == "shoot" && selfBulletController.GetBulletsRemaining() == 0
                    && int.Parse(msgDict[selfIdString]["bullets"]) == 0)
            {
                invalidActionFeedbackController.SetFeedback("Out of Bullets");
            }
            else if (selfAction == "shield")
            {
                invalidActionFeedbackController.SetFeedback("Invalid Shield Action");
            }
            else
            {
                invalidActionFeedbackController.ClearFeedback();
            }
        }

        if (selfAction == "grenade" && checkingGrenadeHit)
        {
            checkingGrenadeHit = false;
        }

        if (selfIsValidGrenade && !shouldUpdateHp)
        {
            checkingGrenadeHit = true;
            selfGrenadeController.SetGrenadesRemaining(int.Parse(msgDict[selfIdString]["grenades"]) - 1, selfIsValidGrenade);

            bool isOppFound = selfGrenadeController.GetIsOppFound();
            msgPublish = isOppFound ? "{\"grenade_throw\": 1}" : "{\"grenade_throw\": 0}";
            Debug.Log("[MQTT PUBLISH] Created message " + topicPublish);

            Publish();

            if (isOppFound)
            {
                invalidActionFeedbackController.SetFeedback("Grenade Hit");
            }
            else
            {
                invalidActionFeedbackController.SetFeedback("Grenade Wasted");
            }
            return;
        }

        if (selfAction == "shoot" && selfBulletController.GetBulletsRemaining() > 0)
        {
            if (oppHealthBarController.GetHealthRemaining() != int.Parse(msgDict[oppIdString]["hp"]))
            {
                invalidActionFeedbackController.SetFeedback("Nice Shot");
            }
            else
            {
                invalidActionFeedbackController.SetFeedback("Can You Aim Better Please?");
            }
        }

        selfBulletController.SetBulletsRemaining(int.Parse(msgDict[selfIdString]["bullets"]), selfIsValidShoot);
        selfGrenadeController.SetGrenadesRemaining(int.Parse(msgDict[selfIdString]["grenades"]), false);
        selfShieldController.SetShieldRemaining(int.Parse(msgDict[selfIdString]["num_shield"]), selfIsValidShield);
        selfBulletController.StartReloading(selfIsValidReload);
        selfScoreController.SetNumKills(int.Parse(msgDict[oppIdString]["num_deaths"]));

        setBothPlayersHpFromGameEngine(msgDict);
    }

    public void DisconnectButton()
    {
        UnsubscribeTopics();
        Disconnect();
    }

    // Start is called before the first frame update
    protected override void Start()
    {
        Debug.Log("[MQTT START] Starting MQTT Client " + brokerAddress);
        base.Start();
    }

    // Update is called once per frame
    protected override void Update()
    {
        base.Update();

        selfIdString = PlayerChoiceController.getSelfId() == 1 ? "p1" : "p2";
        oppIdString = PlayerChoiceController.getOppId() == 1 ? "p1" : "p2";
        selfPlayerId.text = PlayerChoiceController.getSelfId() == 1 ? "PLAYER 1" : "PLAYER 2";
        status.text = selfIdString;
        messageText.text = oppIdString;

        // process message
        if (eventMessages.Count > 0)
        {
            foreach (string msg in eventMessages)
            {
                DisplayMessage(msg);
            }

            // empty queue
            eventMessages.Clear();
        }
    }

    private void OnDestroy()
    {
        UnsubscribeTopics();
        Disconnect();
    }

    protected override void OnApplicationQuit()
    {
        UnsubscribeTopics();
        Disconnect();
    }
}
