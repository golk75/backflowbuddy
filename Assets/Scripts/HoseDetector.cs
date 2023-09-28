using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Events;

public class HoseDetector : MonoBehaviour
{
    public HoseSpring hoseSpring;
    public GameObject testCock;
    public bool isConnected;
    Coroutine onAttachAttempt;
    public GameObject Hose;
    public PlayerController playerController;
    public OperableComponentDescription operableComponentDescription;

    public void OnTriggerEnter(Collider other)
    {
        operableComponentDescription = other.GetComponent<OperableComponentDescription>();
        Actions.onHoseAttach?.Invoke(testCock, operableComponentDescription);
        Actions.onHoseContact?.Invoke(gameObject, operableComponentDescription);
        if (playerController.primaryTouchPerformed)
        {
            onAttachAttempt = StartCoroutine(AttachInitiate());
            Debug.Log($"detector= {name}");
        }
        isConnected = true;
    }

    private void OnTriggerStay(Collider other)
    {
        // Debug.Log($"isConnected = {isConnected}");
    }

    private void OnTriggerExit(Collider other)
    {
        isConnected = false;

        Actions.onHoseDetach?.Invoke(testCock, operableComponentDescription);
    }

    IEnumerator AttachInitiate()
    {
        Debug.Log($"waiting for 0.5 sec.");
        yield return new WaitForSeconds(0.5f);
        if (isConnected == true)
        {
            Debug.Log($"wait completed");
            Actions.onHoseBibConnect?.Invoke(gameObject, operableComponentDescription);
        }
    }

    void Update()
    {
        // Debug.Log($"isConnected = {isConnected}");
    }
}
