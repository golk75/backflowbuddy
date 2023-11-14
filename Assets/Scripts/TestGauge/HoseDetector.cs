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
    public GameObject sightTube;
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

        switch (operableComponentDescription.partsType)
        {
            case OperableComponentDescription.PartsType.TestKitHose:
                Actions.onHoseAttach?.Invoke(testCock, operableComponentDescription);
                break;
            case OperableComponentDescription.PartsType.TestKitSightTube:
                Actions.onSightTubeAttach?.Invoke(sightTube);
                break;
            default:
                Debug.Log($"operableComponentDescription: {operableComponentDescription} not valid");
                break;
        }

        if (playerController.primaryTouchPerformed)
        {
            onAttachAttempt = StartCoroutine(AttachInitiate());

        }
        isConnected = true;

    }


    // private void OnTriggerStay(Collider other)
    // {

    // }

    private void OnTriggerExit(Collider other)
    {
        isConnected = false;

        Actions.onHoseDetach?.Invoke(testCock, operableComponentDescription);
    }

    IEnumerator AttachInitiate()
    {

        yield return new WaitForSeconds(1.0f);
        if (isConnected == true)
        {

            Actions.onHoseBibConnect?.Invoke(gameObject, operableComponentDescription);
        }


    }


}
