using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor.MemoryProfiler;
using UnityEngine;

public class SightTubeController : MonoBehaviour
{
    public PlayerController playerController;
    public bool sightTubeGrabbed = false;
    public bool isAttaching = false;
    public Coroutine SightTubeMovement;
    public bool isConnected = false;
    public Vector3 connectionPoint;

    void OnEnable()
    {
        Actions.onSightTubeGrab += GrabSightTube;
        Actions.onSightTubeDrop += DropSightTube;
        // Actions.onSightTubeAttach += AttachSightTube;
        // Actions.onSightTubeDettach += DettachSightTube;

        Actions.onObjectConnect += ConnectionAttempt;
    }


    void OnDisable()
    {
        Actions.onSightTubeGrab -= GrabSightTube;
        Actions.onSightTubeDrop -= DropSightTube;
        // // Actions.onSightTubeAttach -= AttachSightTube;
        // // Actions.onSightTubeDettach -= DettachSightTube;
        // Actions.onSightTubeConnect -= DisconnectSightTube;


        Actions.onObjectConnect -= ConnectionAttempt;
    }



    private void ConnectionAttempt(GameObject obj, OperableComponentDescription description)
    {

        if (description.partsType == OperableComponentDescription.PartsType.TestKitSightTube)
        {
            connectionPoint = obj.transform.position;
            transform.position = connectionPoint;
            isConnected = true;

        }
    }




    private void ConnectSightTube(GameObject testcock, OperableComponentDescription description)
    {
        if (playerController.primaryTouchPerformed)
        {
            gameObject.transform.position = testcock.transform.position;
        }

    }
    private void DisconnectSightTube(GameObject testcock, OperableComponentDescription description)
    {
        throw new NotImplementedException();
    }

    private void DropSightTube(GameObject obj)
    {
        sightTubeGrabbed = false;
        isAttaching = false;
        // StopCoroutine(MovingSightTube(obj));
    }

    private void GrabSightTube(GameObject obj)
    {
        sightTubeGrabbed = true;
        isAttaching = false;
        SightTubeMovement = StartCoroutine(MovingSightTube(obj));


    }

    IEnumerator MovingSightTube(GameObject go)
    {

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
