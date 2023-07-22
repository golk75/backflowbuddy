using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using com.zibra.liquid.Manipulators;
using TMPro;

public class ResetButton : MonoBehaviour
{
    public GameObject ShutOffValve1;
    public Vector3 initialShutOffValveRot;
    public ZibraLiquidVoid resetVoid;
    public ZibraLiquidEmitter supplyEmitter;
    public PlayerController playerController;
    public ZibraLiquidDetector detector;
    public TestCockController testCockController;
    ShutOffValveController shutOffValveController;

    public void ButtonPress()
    {
        StartCoroutine("Reset");
    }

    IEnumerator Reset()
    {
        foreach (TestCock testCock in testCockController.TestCockList)
        {
            Debug.Log(testCock.description);
        }
        resetVoid.enabled = true;
        Debug.Log($"Coroutine started");
        playerController._operableObject = ShutOffValve1;

        ShutOffValve1.transform.eulerAngles = initialShutOffValveRot;
        playerController._operableObjectRotation = initialShutOffValveRot;
        shutOffValveController.mainSupplyEmitter.VolumePerSimTime = 0f;
        yield return new WaitForSeconds(0.5f);
        shutOffValveController.mainSupplyEmitter.VolumePerSimTime = 0f;
        Debug.Log($"Coroutine stoppped");
        playerController._operableObjectRotation = Vector3.zero;
        playerController.touchStart = Vector3.zero;
        if (playerController._operableObject != null)
            playerController._operableObject = null;

        resetVoid.enabled = false;
    }

    // Start is called before the first frame update
    void Awake()
    {
        initialShutOffValveRot = ShutOffValve1.transform.rotation.eulerAngles;
    }

    // Update is called once per frame
    void Update() { }
}
