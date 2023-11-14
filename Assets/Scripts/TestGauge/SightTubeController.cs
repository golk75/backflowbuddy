using System;
using System.Collections;
using System.Collections.Generic;

using UnityEngine;

public class SightTubeController : MonoBehaviour
{
    public PlayerController playerController;
    public bool sightTubeGrabbed = false;
    public bool isAttaching = false;
    public Coroutine SightTubeMovement;

    void OnEnable()
    {
        Actions.onSightTubeGrab += GrabSightTube;
        Actions.onSightTubeDrop += DropSightTube;
        Actions.onSightTubeAttach += AttachSightTube;
        Actions.onSightTubeDettach += DettachSightTube;

    }

    void OnDisable()
    {
        Actions.onSightTubeGrab -= GrabSightTube;
        Actions.onSightTubeDrop -= DropSightTube;
        Actions.onSightTubeAttach -= AttachSightTube;
        Actions.onSightTubeDettach -= DettachSightTube;
    }
    private void DropSightTube(GameObject obj)
    {
        sightTubeGrabbed = false;
        isAttaching = false;
    }

    private void GrabSightTube(GameObject obj)
    {
        sightTubeGrabbed = true;
        isAttaching = false;
        SightTubeMovement = StartCoroutine(MovingSightTube(obj));


    }

    private void DettachSightTube(GameObject @object)
    {

    }

    private void AttachSightTube(GameObject @object)
    {

    }


    IEnumerator MovingSightTube(GameObject go)
    {

        while (
            playerController.primaryTouchStarted > 0 && sightTubeGrabbed == true && isAttaching == false
            || playerController.primaryClickStarted > 0 && sightTubeGrabbed == true && isAttaching == false
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

    }
}
