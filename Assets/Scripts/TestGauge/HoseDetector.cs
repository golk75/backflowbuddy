using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Events;

public class HoseDetector : MonoBehaviour
{
    public TestCockController testCockController;
    public HoseSpring hoseSpring;
    public GameObject testCock;
    public bool isConnected;
    Coroutine onAttachAttempt;
    public GameObject Hose;
    public PlayerController playerController;
    public OperableComponentDescription operableComponentDescription;
    private Collider collider;
    private Coroutine InitialColliderBlock;

    public CameraController cameraController;
    void OnEnable()
    {
        collider = GetComponent<Collider>();
    }
    void Start()
    {
        InitialColliderBlock = StartCoroutine(HideCollider());

    }
    /// <summary>
    /// If collider is enabled at opening of scene, the hosebib gets thrown around due to the hosbib config joint moving the bib through the collider @Start
    /// </summary>
    /// <returns></returns>
    private IEnumerator HideCollider()
    {
        yield return new WaitForSeconds(1);
        if (collider.enabled == false)
        {
            collider.enabled = true;
        }

    }
    public void OnTriggerEnter(Collider other)
    {
        operableComponentDescription = other.GetComponent<OperableComponentDescription>();



        // operableComponentDescription = other.GetComponent<OperableComponentDescription>();
        if (operableComponentDescription.partsType == OperableComponentDescription.PartsType.TestKitHose)
        {
            Actions.onHoseAttach?.Invoke(testCock, operableComponentDescription);
        }


        // if (playerController.primaryTouchPerformed)
        // {
        //     onAttachAttempt = StartCoroutine(AttachInitiate());

        // }


        if (cameraController.isPanning == false)
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
            OperableComponentDescription connectedObjectDescription = operableComponentDescription;
            Actions.onObjectConnect?.Invoke(gameObject, operableComponentDescription);


        }

    }
}
