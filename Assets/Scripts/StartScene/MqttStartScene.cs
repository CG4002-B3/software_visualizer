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

public class MqttStartScene : M2MqttUnityClient
{
    public SelfEquipConnectionController selfEquipConnectionController;
    public OppEquipConnectionController oppEquipConnectionController;

    // Broker Configurations
    public bool autoTest = false;
    public string topicSubscribe = "to_phone";
    public string topicPublish = "from_phone";
    public string msgPublish = "{grenade_throw: 1}";
    public List<string> eventMessages = new List<string>();

    public void TestPublish()
    {
        client.Publish(topicPublish, System.Text.Encoding.UTF8.GetBytes(msgPublish), MqttMsgBase.QOS_LEVEL_EXACTLY_ONCE, false);
        Debug.Log("[MQTT TEST] Publish test");
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
    }

    protected override void OnConnected()
    {
        Debug.Log("[MQTT CONNECTION] Connected to " + brokerAddress + ", " + brokerPort);
        SubscribeTopics();
    }

    protected override void OnConnectionFailed(string errorMessage)
    {
        Debug.Log("[MQTT ERROR] Connection failed: " + errorMessage);
    }

    protected override void OnDisconnected()
    {
        Debug.Log("[MQTT CONNECTION] Disconnected from " + brokerAddress);
    }

    protected override void OnConnectionLost()
    {
        Debug.Log("[MQTT ERROR] Connection lost!");
    }

    // Topic Subscription
    protected override void SubscribeTopics()
    {
        client.Subscribe(new string[] { "hardware1" }, new byte[] { MqttMsgBase.QOS_LEVEL_EXACTLY_ONCE });
        client.Subscribe(new string[] { "hardware2" }, new byte[] { MqttMsgBase.QOS_LEVEL_EXACTLY_ONCE });
        Debug.Log("[MQTT SUBSCRIPTION] Subscribed to hardware1");
    }

    protected override void UnsubscribeTopics()
    {
        client.Unsubscribe(new string[] { "hardware1" });
        client.Unsubscribe(new string[] { "hardware2" });
        Debug.Log("[MQTT SUBSCRIPTION] Unsubscribed to hardware1");
    }

    public void Publish()
    {
        client.Publish(topicPublish, System.Text.Encoding.UTF8.GetBytes(msgPublish));
        Debug.Log("[MQTT PUBLISH] Publish message to " + topicPublish);
    }

    protected override void DecodeMessage(string topic, byte[] message)
    {
        string msg = System.Text.Encoding.UTF8.GetString(message);
        var msgDict = JSON.Parse(msg);
        Debug.Log("[MQTT RECEIVED] Received new message: " + msg);
        StoreMessage(msg);

        if (topic == "hardware1")
        {
            string setId = msgDict["player"];
            string equipment = msgDict["equipment"];
            bool isEquipmentConnected = int.Parse(msgDict["status"]) == 1;

            if (setId == "p1")
            {
                if (equipment == "vest")
                {
                    selfEquipConnectionController.SetIsSelfVestConnected(isEquipmentConnected);
                }
                else if (equipment == "glove")
                {
                    selfEquipConnectionController.SetIsSelfGloveConnected(isEquipmentConnected);
                }
                else if (equipment == "gun")
                {
                    selfEquipConnectionController.SetIsSelfGunConnected(isEquipmentConnected);
                }
            }
        }
        else if (topic == "hardware2")
        {
            string setId = msgDict["player"];
            string equipment = msgDict["equipment"];
            bool isEquipmentConnected = int.Parse(msgDict["status"]) == 1;

            if (setId == "p2")
            {
                if (equipment == "vest")
                {
                    oppEquipConnectionController.SetIsOppVestConnected(isEquipmentConnected);
                }
                else if (equipment == "glove")
                {
                    oppEquipConnectionController.SetIsOppGloveConnected(isEquipmentConnected);
                }
                else if (equipment == "gun")
                {
                    oppEquipConnectionController.SetIsOppGunConnected(isEquipmentConnected);
                }
            }
        }
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
