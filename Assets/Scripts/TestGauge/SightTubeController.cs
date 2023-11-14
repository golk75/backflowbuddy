using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SightTubeController : MonoBehaviour
{
    void OnEnable()
    {
        Actions.onSightTubeGrab += GrabSightTube;
        Actions.onSightTubeDrop += DropSightTube;

    }
    void OnDisable()
    {
        Actions.onSightTubeGrab -= GrabSightTube;
        Actions.onSightTubeDrop -= DropSightTube;
    }
    private void DropSightTube(GameObject obj)
    {
        Debug.Log($"object: {obj}");
    }

    private void GrabSightTube(GameObject obj)
    {
        Debug.Log($"object: {obj}");
    }

    // Update is called once per frame
    void Update()
    {

    }
}
