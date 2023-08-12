using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class HoseDetector : MonoBehaviour
{
    public GameObject testCock;
    public bool isConnected;
    Coroutine onAttachAttempt;

    public void OnTriggerEnter(Collider other)
    {
        //onHoseBibEnter.Invoke();
        isConnected = true;
        Actions.onHoseAttach?.Invoke(testCock);
    }

    private void OnTriggerStay(Collider other)
    {
        onAttachAttempt = StartCoroutine(AttachInitiate());
    }

    private void OnTriggerExit(Collider other)
    {
        isConnected = false;
        Actions.onHoseDetach?.Invoke(testCock);
    }

    IEnumerator AttachInitiate()
    {
        yield return new WaitForSeconds(2);
        if (isConnected == true)
        {
            Actions.onHoseBibConnect?.Invoke(testCock);
        }
    }
}
