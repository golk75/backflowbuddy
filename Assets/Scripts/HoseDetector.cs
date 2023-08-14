using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
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
        //Actions.onHoseAttach?.Invoke(testCock, operableComponentDescription);

        onAttachAttempt = StartCoroutine(AttachInitiate());
    }

    private void OnTriggerStay(Collider other)
    {
        isConnected = true;
    }

    private void OnTriggerExit(Collider other)
    {
        if (isConnected == true)
        {
            isConnected = false;
        }

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

    void Update() { }
}
