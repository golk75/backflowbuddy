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
    public TestCockController testCockController;
    public CheckValveStatus checkValveStatus;
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

    [SerializeField]
    ZibraLiquidDetector LowHoseDetector;

    [SerializeField]
    ZibraLiquidDetector HighHoseDetector;

    [SerializeField]
    ZibraLiquidDetector BypassHoseDetector;

    [SerializeField]
    ZibraLiquidDetector Zone1Detector;

    [SerializeField]
    GameObject TestCock1;

    [SerializeField]
    GameObject TestCock2;

    [SerializeField]
    GameObject TestCock3;

    [SerializeField]
    GameObject TestCock4;

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
    public bool isCheck1Open;
    public bool isCheck2Open;
    float lowHosePressure;

    public bool isConnectedTestCockOpen;
    public bool isTestCock1Open;
    public bool isTestCock2Open;
    public bool isTestCock3Open;
    public bool isTestCock4Open;

    public float highHosePressure;
    float bypasshosePressure;
    float needleSpeedDamp = 0.005f;

    [SerializeField]
    public List<GameObject> TestCockList;

    [SerializeField]
    public List<ZibraLiquidDetector> TestCockDetectorList;
    Coroutine Check1ClosingPoint;
    float needleVelRef = 0;

    void OnEnable()
    {
        Actions.onHoseAttach += AttachHoseBib;
        Actions.onHoseDetach += DetachHoseBib;
        Actions.onTestCock1Opened += TestCock1Opened;
        Actions.onTestCock1Closed += TestCock1Closed;
        Actions.onTestCock2Opened += TestCoc2Opened;
        Actions.onTestCock2Closed += TestCock2Closed;
        Actions.onTestCock3Opened += TestCock3Opened;
        Actions.onTestCock3Closed += TestCock3Closed;
        Actions.onTestCock4Opened += TestCoc4Opened;
        Actions.onTestCock4Closed += TestCock4Closed;

        //Actions.onTestCockOpen += DetectTestCockOpen;
    }

    void OnDisable()
    {
        Actions.onHoseAttach -= AttachHoseBib;
        Actions.onHoseDetach -= DetachHoseBib;
        Actions.onTestCock1Opened -= TestCock1Opened;
        Actions.onTestCock1Closed -= TestCock1Closed;
        Actions.onTestCock2Opened -= TestCoc2Opened;
        Actions.onTestCock2Opened -= TestCock2Closed;
        Actions.onTestCock3Opened -= TestCock3Opened;
        Actions.onTestCock3Closed -= TestCock3Closed;
        Actions.onTestCock4Opened -= TestCoc4Opened;
        Actions.onTestCock4Closed -= TestCock4Closed;

        //Actions.onTestCockOpen -= DetectTestCockOpen;
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

    private void TestCock4Closed()
    {
        isTestCock4Open = false;
    }

    private void TestCoc4Opened()
    {
        isTestCock4Open = true;
    }

    private void TestCock3Closed()
    {
        isTestCock3Open = false;
    }

    private void TestCock3Opened()
    {
        isTestCock3Open = true;
    }

    private void TestCock2Closed()
    {
        isTestCock2Open = false;
    }

    private void TestCoc2Opened()
    {
        isTestCock2Open = true;
    }

    private void TestCock1Closed()
    {
        isTestCock1Open = false;
    }

    private void TestCock1Opened()
    {
        isTestCock1Open = true;
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
            //END CHECKING IS TC IS HOOKED UP TO MOVE GAUGE WHILE DEVICE IS OPEN
            if (TestCockList.Contains(TestCock1) && shutOffValveController.IsSupplyOn == true)
            {
                //supply is open and test cock is open
                highHosePressure = Mathf.SmoothStep(
                    highHosePressure,
                    Zone1Detector.ParticlesInside,
                    needleSpeedDamp
                );
                Debug.Log($"supply is open and test cock #1 is connected & open");
            }
            else if (
                TestCockList.Contains(TestCock3)
                && shutOffValveController.IsSupplyOn == true
                && isTestCock3Open
            )
            {
                //supply is open and test cock is open
                highHosePressure = Mathf.SmoothStep(
                    highHosePressure,
                    Zone1Detector.ParticlesInside,
                    needleSpeedDamp
                );
                Debug.Log($"supply is open and test cock 3 is connected & open");
            }
            else if (
                TestCockList.Contains(TestCock4)
                && shutOffValveController.IsSupplyOn == true
                && isTestCock4Open
            )
            {
                //supply is open and test cock is open
                highHosePressure = Mathf.SmoothStep(
                    highHosePressure,
                    Zone1Detector.ParticlesInside,
                    needleSpeedDamp
                );
                Debug.Log($"supply is open and test cock 4 is connected & open");
            }
            //END CHECKING IS TC IS HOOKED UP TO MOVE GAUGE WHILE DEVICE IS OPEN




            //========================================
            // #1 Check Test//========================>
            //========================================

            else if (
                TestCockList.Contains(TestCock2)
                && shutOffValveController.IsSupplyOn == true
                && isTestCock2Open
                && !isTestCock3Open
            )
            {
                highHosePressure = Mathf.SmoothStep(
                    highHosePressure,
                    TestCock2Detector.ParticlesInside,
                    0.015f
                );
                Debug.Log(
                    $"supply is open & test cock #2 is connected & open & test cock #3 is closed"
                );
            }
            else if (
                TestCockList.Contains(TestCock2)
                && isTestCock2Open
                && isTestCock3Open
                && shutOffValveController.IsSupplyOn == false
                && checkValveStatus.isCheck1Closed == false
            )
            {
                //best looking psid drop so far is: highHosePressure -= 0.3f;
                highHosePressure -= 0.3f;

                Debug.Log($"highHosePressure = {highHosePressure}");

                Debug.Log(
                    $"supply is closed & check1 is open & test cock #2 is connected & open & test cock #3 is open"
                );
            }
            else if (
                TestCockList.Contains(TestCock2)
                && isTestCock2Open
                && isTestCock3Open
                && shutOffValveController.IsSupplyOn == false
                && checkValveStatus.isCheck1Closed == true
            )
            {
                highHosePressure += 0;
                //CaptureCheck1ClosingPoint(highHosePressure);

                //Debug.Log($"closingPoint = {closingPoint}");
            }
            //========================================
            // END - #1 Check Test//==================>
            //========================================
            //========================================
            // #2 Check Test//========================>
            //========================================
            else if (
                TestCockList.Contains(TestCock3)
                && shutOffValveController.IsSupplyOn == true
                && isTestCock3Open
                && !isTestCock4Open
            )
            {
                highHosePressure = Mathf.SmoothStep(
                    highHosePressure,
                    TestCock2Detector.ParticlesInside,
                    0.015f
                );
                Debug.Log(
                    $"supply is open & test cock #3 is connected & open & test cock #4 is closed"
                );
            }
            else if (
                TestCockList.Contains(TestCock3)
                && isTestCock3Open
                && isTestCock4Open
                && shutOffValveController.IsSupplyOn == false
                && checkValveStatus.isCheck2Closed == false
            )
            {
                //best looking psid drop so far is: highHosePressure -= 0.3f;
                highHosePressure -= 0.4f;

                Debug.Log($"highHosePressure = {highHosePressure}");

                Debug.Log(
                    $"supply is closed & check2 is open & test cock #3 is connected & open & test cock #4 is open"
                );
            }
            else if (
                TestCockList.Contains(TestCock3)
                && isTestCock3Open
                && isTestCock4Open
                && shutOffValveController.IsSupplyOn == false
                && checkValveStatus.isCheck2Closed == true
            )
            {
                highHosePressure += 0;
                Debug.Log(
                    $"supply is closed & check2 is closed & test cock #3 is connected & open & test cock #4 is open"
                );
            }
            //========================================
            // END - #2 Check Test//==================>
            //========================================
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
        }
        /*
        Debug.Log(
            $"highHosePressure = {highHosePressure}| GetPsidNeedleRotation() = {GetPsidNeedleRotation()}| closingPoint = {closingPoint}"
        );
        */
        // lowHosePressure = LowHoseDetector.ParticlesInside;
        // bypasshosePressure = BypassHoseDetector.ParticlesInside;
        //Debug.Log(highHosePressure);
    }

    private float CaptureCheck1ClosingPoint(float psid)
    {
        return psid;
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
        isCheck1Open = checkValveStatus.isCheck1Open;

        PressureControl();
        OperateControls();
        NeedleControl();
    }
}
