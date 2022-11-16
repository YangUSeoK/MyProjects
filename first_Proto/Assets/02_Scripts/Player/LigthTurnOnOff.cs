using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LigthTurnOnOff : MonoBehaviour
{
    private GameObject Light = null;
    private bool isTurnOn = true;

    private void Awake()
    {
        Light = GetComponentInChildren<Light>().gameObject;
    }

    private void Update()
    {

        if (Input.GetKeyDown(KeyCode.Space))
        {
            if (isTurnOn)
            {
                Light.SetActive(false);
                isTurnOn = false;
            }
            else
            {
                Light.SetActive(true);
                isTurnOn = true;
            }
        }

        
    }



}
