using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SelfEquipConnectionController : MonoBehaviour
{
    private Color CONNECTED_COLOR = new Color(0f, 1f, 0f, 1f);
    private Color DISCONNECTED_COLOR = new Color(1f, 0f, 0f, 0.4f);

    public Image selfVest;
    public Image selfGlove;
    public Image selfGun;

    private bool isSelfVestConnected;
    private bool isSelfGloveConnected;
    private bool isSelfGunConnected;

    void Start()
    {
        isSelfVestConnected = false;
        isSelfGloveConnected = false;
        isSelfGunConnected = false;
    }

    void Update()
    {
        selfVest.color = isSelfVestConnected ? CONNECTED_COLOR : DISCONNECTED_COLOR;
        selfGlove.color = isSelfGloveConnected ? CONNECTED_COLOR : DISCONNECTED_COLOR;
        selfGun.color = isSelfGunConnected ? CONNECTED_COLOR : DISCONNECTED_COLOR;
    }

    public void SetIsSelfVestConnected(bool isConnected)
    {
        isSelfVestConnected = isConnected;
    }

    public void SetIsSelfGloveConnected(bool isConnected)
    {
        isSelfGloveConnected = isConnected;
    }

    public void SetIsSelfGunConnected(bool isConnected)
    {
        isSelfGunConnected = isConnected;
    }

    public bool GetIsSelfGunConnected()
    {
        return isSelfGunConnected;
    }

    public bool GetIsSelfGloveConnected()
    {
        return isSelfGloveConnected;
    }

    public bool GetIsSelfVestConnected()
    {
        return isSelfVestConnected;
    }
}
