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

    public BulletController selfBulletController;
    public GrenadeController selfGrenadeController;
    public SelfShieldController selfShieldController;
    public SelfScoreController selfScoreController;

    public OppHealthBarController oppHealthBarController;

    // Broker Configurations
    public bool autoTest = false;
    public string topicSubscribe = "to_phone";
    public string topicPublish = "from_phone";
    public string msgPublish = "{grenade_throw: 1}";
    public List<string> eventMessages = new List<string>();

    private bool checkingGrenadeHit = false;

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

    protected override void DecodeMessage(string topic, byte[] message)
    {
        string msg = System.Text.Encoding.UTF8.GetString(message);
        var msgDict = JSON.Parse(msg);
        Debug.Log("[MQTT RECEIVED] Received new message: " + msg);
        SetStatus("Received");
        StoreMessage(msg);

        if (int.Parse(msgDict["game_engine_update"]) == 1)
        {
            selfBulletController.SetBulletsRemaining(int.Parse(msgDict["p1"]["bullets"]), false);
            selfGrenadeController.SetGrenadesRemaining(int.Parse(msgDict["p1"]["grenades"]), false);
            selfShieldController.SetShieldRemaining(int.Parse(msgDict["p1"]["num_shield"]), false);
            selfScoreController.SetNumKills(int.Parse(msgDict["p2"]["num_deaths"]));

            oppHealthBarController.SetHealthRemaining(int.Parse(msgDict["p2"]["hp"]));
            return;
        }

        string selfAction = msgDict["p1"]["action"];
        bool selfActionValid = int.Parse(msgDict["p1"]["action_valid"]) == 1;
        bool shouldUpdateHp = int.Parse(msgDict["p1"]["should_update_hp"]) == 1;

        bool selfIsValidGrenade = selfAction == "grenade" && selfActionValid;
        bool selfIsValidReload = selfAction == "reload" && selfActionValid;
        bool selfIsValidShoot = selfAction == "shoot" && int.Parse(msgDict["p1"]["bullets"]) > 0;
        bool selfIsValidShield = selfAction == "shield" && selfActionValid;

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
            else if (selfAction == "shoot" && int.Parse(msgDict["p1"]["bullets"]) == 0)
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
            selfGrenadeController.SetGrenadesRemaining(int.Parse(msgDict["p1"]["grenades"]) - 1, selfIsValidGrenade);

            msgPublish = selfGrenadeController.GetIsOppFound()? "{\"grenade_throw\": 1}" : "{\"grenade_throw\": 0}";
            Debug.Log("[MQTT PUBLISH] Created message " + topicPublish);

            Publish();
            return;
        }

        selfBulletController.SetBulletsRemaining(int.Parse(msgDict["p1"]["bullets"]), selfIsValidShoot);
        selfGrenadeController.SetGrenadesRemaining(int.Parse(msgDict["p1"]["grenades"]), false);
        selfShieldController.SetShieldRemaining(int.Parse(msgDict["p1"]["num_shield"]), selfIsValidShield);
        selfBulletController.StartReloading(selfIsValidReload);
        selfScoreController.SetNumKills(int.Parse(msgDict["p2"]["num_deaths"]));

        oppHealthBarController.SetHealthRemaining(int.Parse(msgDict["p2"]["hp"]));
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
