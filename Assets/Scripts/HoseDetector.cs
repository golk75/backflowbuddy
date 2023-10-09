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
    void OnEnable()
    {

    }
    public void OnTriggerEnter(Collider other)
    {
        operableComponentDescription = other.GetComponent<OperableComponentDescription>();
        Actions.onHoseAttach?.Invoke(testCock, operableComponentDescription);

        if (playerController.primaryTouchPerformed)
        {
            onAttachAttempt = StartCoroutine(AttachInitiate());
            Debug.Log($"detector= {name}");
        }
        isConnected = true;
        // Debug.Log($"isConnected = {isConnected}");
    }

    //
    private void OnTriggerStay(Collider other)
    {

    }

    private void OnTriggerExit(Collider other)
    {
        isConnected = false;

        Actions.onHoseDetach?.Invoke(testCock, operableComponentDescription);
    }

    IEnumerator AttachInitiate()
    {

        yield return new WaitForSeconds(1.75f);
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
