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
    public GameObject Hose;
    public OperableComponentDescription operableComponentDescription;

    public void OnTriggerEnter(Collider other)
    {
        isConnected = true;
        operableComponentDescription = other.GetComponent<OperableComponentDescription>();
        Actions.onHoseAttach?.Invoke(testCock, operableComponentDescription);
        Debug.Log(Hose);
    }

    private void OnTriggerStay(Collider other)
    {
        onAttachAttempt = StartCoroutine(AttachInitiate());
    }

    private void OnTriggerExit(Collider other)
    {
        isConnected = false;
        Actions.onHoseDetach?.Invoke(testCock, operableComponentDescription);
    }

    IEnumerator AttachInitiate()
    {
        yield return new WaitForSeconds(2);
        if (isConnected == true)
        {
            Actions.onHoseBibConnect?.Invoke(testCock, operableComponentDescription);
        }
    }
}
