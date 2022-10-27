using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class OppEquipConnectionController : MonoBehaviour
{
    private Color CONNECTED_COLOR = Color.green;
    private Color DISCONNECTED_COLOR = Color.red;

    public Image oppVest;
    public Image oppGlove;
    public Image oppGun;

    private bool isOppVestConnected;
    private bool isOppGloveConnected;
    private bool isOppGunConnected;

    void Start()
    {
        isOppVestConnected = false;
        isOppGloveConnected = false;
        isOppGunConnected = false;
    }

    void Update()
    {
        oppVest.color = isOppVestConnected ? CONNECTED_COLOR : DISCONNECTED_COLOR;
        oppGlove.color = isOppGloveConnected ? CONNECTED_COLOR : DISCONNECTED_COLOR;
        oppGun.color = isOppGunConnected ? CONNECTED_COLOR : DISCONNECTED_COLOR;
    }

    public void SetIsOppVestConnected(bool isConnected)
    {
        isOppVestConnected = isConnected;
    }

    public void SetIsOppGloveConnected(bool isConnected)
    {
        isOppGloveConnected = isConnected;
    }

    public void SetIsOppGunConnected(bool isConnected)
    {
        isOppGunConnected = isConnected;
    }
}
