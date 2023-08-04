using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using com.zibra.liquid.Manipulators;
using System;
using JetBrains.Annotations;

public class TestKitController : MonoBehaviour
{
    public WaterController waterController;
    public PlayerController playerController;
    public ShutOffValveController shutOffValveController;

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

    [SerializeField]
    GameObject Check1;

    [SerializeField]
    Vector3 Check1Pos;

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

    [SerializeField]
    ZibraLiquidDetector Zone1Detector;

    [SerializeField]
    GameObject connectedTestCock;

    [SerializeField]
    ZibraLiquidDetector TestCock2Detector;

    private const float MinNeedle_rotation = 55;
    private const float MaxNeedle_rotation = -55;
    private const float MinKnob_rotation = 0;

    //limit knobs to 4 complete rotations (x1 rotation = 360;)->
    private const float MaxKnob_rotation = 1440;
    private float currentKnobRotCount;

    private float currentKnobRotation;
    private float maxKnobRotation;
    private float minKnobRotation;

    [SerializeField]
    private float currentPSID;
    private float maxPSID;
    private float minPSID;
    float closingPoint = 0;
    public bool isOperableObject;
    public bool isConnectedToAssembly;
    float lowHosePressure;
    float highHosePressure;
    float bypasshosePressure;
    float needleSpeedDamp = 0.005f;

    [SerializeField]
    public List<GameObject> TestCockList;

    [SerializeField]
    public List<ZibraLiquidDetector> TestCockDetectorList;
    Coroutine Check1ClosingPoint;

    void OnEnable()
    {
        Actions.onHoseAttach += AttachHoseBib;
        Actions.onHoseDetach += DetachHoseBib;
    }

    void OnDisable()
    {
        Actions.onHoseAttach -= AttachHoseBib;
        Actions.onHoseDetach -= DetachHoseBib;
    }

    // Start is called before the first frame update
    void Start()
    {
        currentPSID = 0;
        minPSID = 0;
        maxPSID = 55;
        currentKnobRotation = 0;
        maxKnobRotation = 1440;
        minKnobRotation = 0;
        highHosePressure = 0;
        initLowHosePosition = LowHose.transform.position;
        initHighHosePosition = HighHose.transform.position;
        initBypassHosePosition = BypassHose.transform.position;
    }

    private float GetPsidNeedleRotation()
    {
        float PsidDiff = MinNeedle_rotation - MaxNeedle_rotation;

        float normalizedPsid = highHosePressure / maxPSID;

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
            if (
                playerController.operableComponentDescription.partsType
                == OperableComponentDescription.PartsType.TestKitValve
            )
                currentKnob = playerController.OperableTestGaugeObject;

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
            if (currentKnob != null)
            {
                currentKnob.transform.eulerAngles = new Vector3(0, 0, GetKnobRotation());
            }
        }
    }

    public void AttachHoseBib(GameObject testCock)
    {
        isConnectedToAssembly = true;

        if (TestCockList.Contains(testCock) != true)
        {
            TestCockList.Add(testCock);
        }
        if (
            TestCockDetectorList.Contains(testCock.GetComponentInChildren<ZibraLiquidDetector>())
            != true
        )
        {
            TestCockDetectorList.Add(testCock.GetComponentInChildren<ZibraLiquidDetector>());
        }
        //  Debug.Log($"{gameObject} attached to assembly");
    }

    public void DetachHoseBib(GameObject testCock)
    {
        isConnectedToAssembly = false;

        TestCockList.Remove(testCock);

        //Debug.Log($"{gameObject} detached from assembly");
    }

    float needleVelRef = 0;

    private void NeedleControl()
    {
        // For  now, soely using high hose (double check assembly)
        needle.transform.eulerAngles = new Vector3(0, 0, GetPsidNeedleRotation());
    }

    private void PressureControl()
    {
        // For  now, soely using high hose (double check assembly)


        if (isConnectedToAssembly == true)
        {
            if (
                TestCockList[0].transform.eulerAngles.z > 0
                && shutOffValveController.IsSupplyOn == true
            )
            {
                //supply is open and test cock is open
                highHosePressure = Mathf.SmoothStep(
                    highHosePressure,
                    Zone1Detector.ParticlesInside,
                    needleSpeedDamp
                );
            }
            if (
                TestCockList[0].transform.eulerAngles.z > 0
                && shutOffValveController.IsSupplyOn == false
            )
            {
                //supply is closed and test cock is open
                highHosePressure = Mathf.SmoothStep(
                    highHosePressure,
                    TestCock2Detector.ParticlesInside,
                    needleSpeedDamp
                );
            }
        }

        if (isConnectedToAssembly == false)
        {
            highHosePressure -= 5;
        }
        if (highHosePressure <= minPSID)
        {
            highHosePressure = minPSID;
        }
        if (highHosePressure > maxPSID)
        {
            highHosePressure = maxPSID;
        } /*
        Debug.Log(
            $"highHosePressure = {highHosePressure}| GetPsidNeedleRotation() = {GetPsidNeedleRotation()}| closingPoint = {closingPoint}"
        );
        */
        // lowHosePressure = LowHoseDetector.ParticlesInside;
        // bypasshosePressure = BypassHoseDetector.ParticlesInside;
        //Debug.Log(highHosePressure);
    }

    IEnumerator Check1Test()
    {
        while (true)
        {
            closingPoint += 0.1f * Time.deltaTime;
            yield return null;
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
