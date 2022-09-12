using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class OppShieldController : MonoBehaviour
{
    public GameObject oppShieldPrefab;

    // Start is called before the first frame update
    void Start()
    {
        oppShieldPrefab.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {

    }

    void ToggleShield()
    {
        oppShieldPrefab.SetActive(!oppShieldPrefab.activeSelf);
    }
}
