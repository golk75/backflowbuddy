using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class HoseDetector : MonoBehaviour
{
    public GameObject testCock;

    public void OnTriggerEnter(Collider other)
    {
        //onHoseBibEnter.Invoke();
        Actions.onHoseAttach?.Invoke(testCock);
    }

    private void OnTriggerStay() { }

    private void OnTriggerExit(Collider other)
    {
        Actions.onHoseDetach?.Invoke(testCock);
    }
}
