using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using com.zibra.liquid.Manipulators;
using System;

public class TestKitController : MonoBehaviour
{
    public WaterController waterController;
    public PlayerController playerController;
    public GameObject lowBleed;
    public GameObject lowControl;

    public GameObject highBleed;
    public GameObject highControl;

    public GameObject bypassControl;

    public GameObject needle;
    private GameObject currentKnob;

    [SerializeField]
    GameObject LowHose;

    [SerializeField]
    GameObject HighHose;

    [SerializeField]
    GameObject BypassHose;

    Vector3 initLowHosePosition;
    Vector3 initHighHosePosition;
    Vector3 initBypassHosePosition;

    GameObject ConnectedTestCock;

    [SerializeField]
    ZibraLiquidDetector LowHoseDetector;

    [SerializeField]
    ZibraLiquidDetector HighHoseDetector;

    [SerializeField]
    ZibraLiquidDetector BypassHoseDetector;

    private const float MinNeedle_rotation = 55;
    private const float MaxNeedle_rotation = -55;
    private const float MinKnob_rotation = 0;

    //limit knobs to 4 complete rotations (x1 rotation = 360;)->
    private const float MaxKnob_rotation = 1440;
    private float currentKnobRotCount;

    private float currentKnobRotation;
    private float maxKnobRotation;
    private float minKnobRotation;

    private float currentPSID;
    private float maxPSID;
    public bool isOperableObject;
    public bool isConnectedToAssembly;
    float lowHosePressure;
    float highHosePressure;
    float bypasshosePressure;

    void OnEnable()
    {
        Actions.OnTestKitOperate += TestKitOperate;
        Actions.onHoseAttach += AttachHoseBib;
        Actions.onHoseDetach += DetachHoseBib;
    }

    void OnDisable()
    {
        Actions.OnTestKitOperate -= TestKitOperate;
        Actions.onHoseAttach -= AttachHoseBib;
        Actions.onHoseDetach += DetachHoseBib;
    }

    // Start is called before the first frame update
    void Start()
    {
        currentPSID = 0;
        maxPSID = 55;
        currentKnobRotation = 0;
        maxKnobRotation = 1440;
        minKnobRotation = 0;
        initLowHosePosition = LowHose.transform.position;
        initHighHosePosition = HighHose.transform.position;
        initBypassHosePosition = BypassHose.transform.position;
    }

    private float GetPsidNeedleRotation()
    {
        float PsidDiff = MinNeedle_rotation - MaxNeedle_rotation;

        float normalizedPsid = currentPSID / maxPSID;

        return MinNeedle_rotation - normalizedPsid * PsidDiff;
    }

    private float GetKnobRotation()
    {
        // max - min to rotate left while increasing
        float rotationDiff = MaxKnob_rotation - MinKnob_rotation;

        float normalizedRotation = currentKnobRotation / maxKnobRotation;

        //return MinKnob_rotation + normalizedRotation * rotationDiff;
        return MinKnob_rotation + normalizedRotation * rotationDiff;
    }

    private void TestKitOperate(TestKitOperableCheck testKit)
    {
        currentKnob = testKit.gameObject;

        //currentKnobRotation = testKit.gameObject.transform.eulerAngles.z;
    }

    /*
    private void OperateControls()
    {
        if (isOperableObject == true)
        {
            float counter = 0;
            currentKnobRotation = (
                playerController.touchStart.x
                - Camera.main.ScreenToWorldPoint(Input.mousePosition).x
            );
           
            if (currentKnob.transform.eulerAngles.z > maxKnobRotation)
            {
                currentKnobRotation = maxKnobRotation;
            }
            //currentKnob.transform.eulerAngles = new Vector3(0, 0, GetKnobRoation());
            currentKnob.transform.rotation = Quaternion.Euler(
                new Vector3(0, 0, currentKnob.transform.eulerAngles.z + GetKnobRotation() * 0.5f)
            );
            Debug.Log($"isOperableObject= {isOperableObject}; counter = {counter}");
        }
    }
    */
    private void OperateControls()
    {
        if (playerController.isOperableObject == true)
        {
            currentKnobRotation +=
                (
                    playerController.touchStart.x
                    - Camera.main.ScreenToWorldPoint(Input.mousePosition).x
                ) / 5;

            //currentKnobRotation += 1 * Time.deltaTime;

            if (currentKnobRotation > maxKnobRotation)
            {
                currentKnobRotation = maxKnobRotation;
            }
            if (currentKnobRotation < minKnobRotation)
            {
                currentKnobRotation = minKnobRotation;
            }

            currentKnob.transform.eulerAngles = new Vector3(0, 0, GetKnobRotation());
        }
    }

    public void AttachHoseBib(GameObject gameObject)
    {
        isConnectedToAssembly = true;

        //Debug.Log($"Connected to Assembly");
    }

    public void DetachHoseBib(GameObject gameObject)
    {
        isConnectedToAssembly = false;
        //Debug.Log($"Disconnected from Assembly");
    }

    private void NeedleControl()
    {
        if (isConnectedToAssembly == true)
        {
            // For now, soely using high hose (double check assembly)
            currentPSID += highHosePressure * Time.deltaTime;
            if (currentPSID > maxPSID)
            {
                currentPSID = maxPSID;
            }
            needle.transform.eulerAngles = new Vector3(0, 0, GetPsidNeedleRotation());
        }
    }

    private void PressureControl()
    {
        if (isConnectedToAssembly == true)
        {
            highHosePressure = HighHoseDetector.ParticlesInside;
            lowHosePressure = LowHoseDetector.ParticlesInside;
            bypasshosePressure = BypassHoseDetector.ParticlesInside;
        }
    }

    // Update is called once per frame
    void Update()
    {
        PressureControl();
        OperateControls();
        NeedleControl();
    }
}
