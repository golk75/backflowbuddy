using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class HoseDetector : MonoBehaviour
{
    public GameObject testCock;
    public bool isConnected;

    public void OnTriggerEnter(Collider other)
    {
        //onHoseBibEnter.Invoke();
        isConnected = true;
        Actions.onHoseAttach?.Invoke(testCock);
    }

    private void OnTriggerStay() { }

    private void OnTriggerExit(Collider other)
    {
        isConnected = false;
        Actions.onHoseDetach?.Invoke(testCock);
    }
}
