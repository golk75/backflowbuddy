using System;
using com.zibra.liquid.Manipulators;
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
    private void OnTriggerStay(Collider other)
    {
        if (other.GetComponent<OperableComponentDescription>() != null)
        {
            if (other.GetComponent<OperableComponentDescription>().partsType == OperableComponentDescription.PartsType.TestCock)
            {
                currentTestCockConnection = other.gameObject;
            }

        }



    }
    private void OnTriggerExit(Collider other)
    {
        currentTestCockConnection = null;
        // 
    }
    private void CurrentTestCockExit(GameObject @object, OperableComponentDescription description)
    {
        Actions.onRemoveHoseFromList?.Invoke(this.gameObject, GetComponent<OperableComponentDescription>());
        Actions.onRemoveTestCockFromList?.Invoke(@object, GetComponent<OperableComponentDescription>());
        // currentTestCockConnection = null;
    }

    private void CurrentTestCockEnter(GameObject @object, OperableComponentDescription description)
    {

        // if (description.partsType == OperableComponentDescription.PartsType.TestCock) { }
        // currentTestCockConnection = @object;
    }
}
