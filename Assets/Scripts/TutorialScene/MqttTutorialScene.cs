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

public class MqttTutorialScene : M2MqttUnityClient
{
    // Broker Configurations
    public bool autoTest = false;
    public string topicSubscribe = "to_phone";
    public string topicPublish = "from_phone";
    public string msgPublish = "{grenade_throw: 1}";
    public List<string> eventMessages = new List<string>();

    private string selfIdString;

    private string selfAction = "none";
    private bool justDecodedData = false;
    private bool isMyActionSent = false;

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
        client.Subscribe(new string[] { "to_phone" }, new byte[] { MqttMsgBase.QOS_LEVEL_EXACTLY_ONCE });
        Debug.Log("[MQTT SUBSCRIPTION] Subscribed to to_phone");
    }

    protected override void UnsubscribeTopics()
    {
        Debug.Log("[MQTT SUBSCRIPTION] Unsubscribed to to_phone");
    }

    public void PublishFinishTutorial()
    {
        if (selfIdString == "p1")
        {
            client.Publish(topicPublish, System.Text.Encoding.UTF8.GetBytes("finish_tutorial_1"));
        }
        else if (selfIdString == "p2")
        {
            client.Publish(topicPublish, System.Text.Encoding.UTF8.GetBytes("finish_tutorial_2"));
        }
        Debug.Log("[MQTT PUBLISH] Publish message to " + topicPublish);
    }

    protected override void DecodeMessage(string topic, byte[] message)
    {
        string msg = System.Text.Encoding.UTF8.GetString(message);
        var msgDict = JSON.Parse(msg);
        Debug.Log("[MQTT RECEIVED] Received new message: " + msg);
        StoreMessage(msg);

        if (topic == "to_phone" && msg.Contains(selfIdString))
        {
            justDecodedData = true;
            selfAction = msgDict[selfIdString];
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

        selfIdString = PlayerChoiceController.getSelfId() == 1 ? "p1" : "p2";

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

    public string getSelfAction()
    {
        return selfAction;
    }

    public string getSelfIdString()
    {
        return selfIdString;
    }

    public bool getJustDecodedData()
    {
        return justDecodedData;
    }

    public void setJustDecodedData(bool val)
    {
        justDecodedData = val;
    }
}
