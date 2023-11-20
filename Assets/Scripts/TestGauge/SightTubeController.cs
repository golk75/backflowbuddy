using System;
using Unity.VisualScripting;
using UnityEngine;

public class SightTubeController : MonoBehaviour
{
    public GameObject currentTestCockConnection;
    void OnEnable()
    {
        Actions.onTestCockColliderEnter += CurrentTestCockEnter;
        Actions.onTestCockColliderExit += CurrentTestCockExit;
    }
    void OnDisable()
    {

        Actions.onTestCockColliderEnter -= CurrentTestCockEnter;
        Actions.onTestCockColliderExit -= CurrentTestCockExit;
    }
    private void CurrentTestCockExit(GameObject @object, OperableComponentDescription description)
    {

        currentTestCockConnection = null;
    }

    private void CurrentTestCockEnter(GameObject @object, OperableComponentDescription description)
    {

        if (description.partsType == OperableComponentDescription.PartsType.TestCock)
            currentTestCockConnection = @object;
    }
}
