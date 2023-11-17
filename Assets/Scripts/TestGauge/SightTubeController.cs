using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor.MemoryProfiler;
using UnityEngine;

public class SightTubeController : MonoBehaviour
{
    public PlayerController playerController;
    public TestCockController testCockController;
    public CameraController cameraController;
    public GameObject currentTestCock;
    public bool sightTubeGrabbed = false;
    public bool isAttaching = false;
    public Coroutine SightTubeMovement;
    public bool isConnected = false;
    public Vector3 connectionPoint;
    public Vector3 sightTubeHomePos;
    public float testCockPositionOffset = 1.2f;
    public bool isTestCockBeingUsed;
    public bool isCurrentTestCockAttached;

    void OnEnable()
    {
        Actions.onSightTubeGrab += GrabSightTube;
        Actions.onSightTubeDrop += DropSightTube;
        // Actions.onObjectConnect += ConnectionAttempt;
        // Actions.onTestCockColliderEnter += GetCurrentTestCockColliderEntry;
        // Actions.onTestCockColliderExit += GetCurrentTestCockColliderExit;
        sightTubeHomePos = transform.localPosition;


    }



    void OnDisable()
    {
        Actions.onSightTubeGrab -= GrabSightTube;
        Actions.onSightTubeDrop -= DropSightTube;
        // Actions.onObjectConnect -= ConnectionAttempt;
        // Actions.onTestCockColliderEnter -= GetCurrentTestCockColliderEntry;
        // Actions.onTestCockColliderExit -= GetCurrentTestCockColliderExit;
    }


    private void GetCurrentTestCockColliderEntry(GameObject testCockDetector, OperableComponentDescription description)
    {
        currentTestCock = testCockDetector;
    }
    private void GetCurrentTestCockColliderExit(GameObject testCockDetector)
    {
        currentTestCock = testCockDetector;
        if (!isCurrentTestCockAttached)
        {


        }
    }


    //listening to HoseDetector(s)--> obj = test cock and/or hose detector
    private void ConnectionAttempt(GameObject obj, OperableComponentDescription description)
    {


        currentTestCock = obj;
        if (description.partsType == OperableComponentDescription.PartsType.TestKitSightTube)
        {
            Actions.onAddTestCockToList?.Invoke(currentTestCock, description);
            Actions.onAddHoseToList?.Invoke(this.gameObject, GetComponent<OperableComponentDescription>());

            connectionPoint = new Vector3(obj.transform.position.x, obj.transform.position.y + testCockPositionOffset, obj.transform.position.z);
            transform.position = obj.transform.position;
            isConnected = true;
        }

    }


    private void DropSightTube(GameObject obj)
    {

        sightTubeGrabbed = false;
        isAttaching = false;
        transform.localPosition = sightTubeHomePos;
        StopCoroutine(MovingSightTube(obj));


    }

    private void GrabSightTube(GameObject obj)
    {

        sightTubeGrabbed = true;
        isAttaching = false;
        SightTubeMovement = StartCoroutine(MovingSightTube(obj));
        if (!cameraController.isPanning)
        {
            if (currentTestCock != null)
            {

                //add check for panning camera since sight tube floats a little offset from test cock if camera is panned aggressively/ fast
                Actions.onRemoveTestCockFromList?.Invoke(currentTestCock, currentTestCock.GetComponent<OperableComponentDescription>());
                Actions.onRemoveHoseFromList?.Invoke(this.gameObject, GetComponent<OperableComponentDescription>());
            }
        }


    }

    IEnumerator MovingSightTube(GameObject go)
    {
        isConnected = false;
        while (
            playerController.primaryTouchStarted > 0 && sightTubeGrabbed == true && isConnected == false
            || playerController.primaryClickStarted > 0 && sightTubeGrabbed == true && isConnected == false
        )
        {
            Vector3 direction =
              Camera.main.ScreenToWorldPoint(Input.mousePosition);



            go.transform.position = new Vector3(direction.x, direction.y, go.transform.position.z);

            yield return null;
        }

    }

    // Update is called once per frame
    void Update()
    {
        if (isConnected)
        {
            transform.position = connectionPoint;
        }

    }
}
