using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using com.zibra.liquid.Manipulators;
using com.zibra.liquid.Solver;
using JetBrains.Annotations;
using Unity.VisualScripting;

using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UIElements;

public class TestKitManager : MonoBehaviour
{
    public WaterController waterController;
    public PlayerController playerController;
    public ShutOffValveController shutOffValveController;
    public TestCockController testCockController;
    public CheckValveStatus checkValveStatus;
    public Animator openKnobAnimation;

    public ZibraLiquid liquid;
    public GameObject highBleed;
    public GameObject highControl;
    public GameObject lowBleed;
    public GameObject lowControl;
    public GameObject bypassControl;

    public GameObject needle;
    public GameObject digitalKitNeedle;
    public GameObject currentKnob;

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
    ZibraLiquidEmitter bleederHoseEmitter;
    [SerializeField]
    ZibraLiquidDetector LowHoseDetector;

    [SerializeField]
    ZibraLiquidDetector HighHoseDetector;

    [SerializeField]
    ZibraLiquidDetector BypassHoseDetector;

    [SerializeField]
    ZibraLiquidDetector Zone1Detector;
    [SerializeField]
    ZibraLiquidDetector Zone2Detector;
    [SerializeField]
    ZibraLiquidDetector Zone3Detector;
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

    // private const float MinNeedle_rotation = 55;
    // private const float MaxNeedle_rotation = -55;
    private const float MinNeedle_rotation = 61;
    private const float MaxNeedle_rotation = -58;
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
    public float hosePressure;
    public float highHosePressure;
    float bypasshosePressure;
    float needleSpeedDamp = 0.005f;
    public float knobRotationFactor = 0;

    Coroutine KnobClickOperate;

    Coroutine Check1ClosingPoint;
    float needleVelRef = 0;
    public bool knobOpened = false;

    //ui toolkit
    public UIDocument _root;
    //public VisualElement _root;
    // private VisualElement _gaugeProgressBar;
    // private Length MinFillPos = Length.Percent(0);
    // private Length MaxFillPos = Length.Percent(100);
    private float MinFillPos = 0;
    private float MaxFillPos = 100;
    public float knobRotation;
    public List<GameObject> StaticTestCockList;
    // public List<GameObject> TestCockList;
    public List<GameObject> AttachedTestCockList;
    public List<GameObject> AttachedHoseList;
    int rot = 0;
    void OnEnable()
    {

        // Actions.onHoseAttach += AttachHoseBib;
        // Actions.onHoseDetach += DetachHoseBib;
        Actions.onTestCock1Opened += TestCock1Opened;
        Actions.onTestCock1Closed += TestCock1Closed;
        Actions.onTestCock2Opened += TestCoc2Opened;
        Actions.onTestCock2Closed += TestCock2Closed;
        Actions.onTestCock3Opened += TestCock3Opened;
        Actions.onTestCock3Closed += TestCock3Closed;
        Actions.onTestCock4Opened += TestCoc4Opened;
        Actions.onTestCock4Closed += TestCock4Closed;
        Actions.onAddTestCockToList += AddTestCockToList;
        Actions.onRemoveTestCockFromList += RemoveTestCockFromList;
        Actions.onAddHoseToList += AddHoseToList;
        Actions.onRemoveHoseFromList += RemoveHoseFromList;

        // Actions.onHighBleedOpen += HighBleedKnobOpened;
        // Actions.onHighBleedClosed += HighBleedKnobClosed;


    }


    void OnDisable()
    {
        // Actions.onHoseAttach -= AttachHoseBib;
        // Actions.onHoseDetach -= DetachHoseBib;
        Actions.onTestCock1Opened -= TestCock1Opened;
        Actions.onTestCock1Closed -= TestCock1Closed;
        Actions.onTestCock2Opened -= TestCoc2Opened;
        Actions.onTestCock2Opened -= TestCock2Closed;
        Actions.onTestCock3Opened -= TestCock3Opened;
        Actions.onTestCock3Closed -= TestCock3Closed;
        Actions.onTestCock4Opened -= TestCoc4Opened;
        Actions.onTestCock4Closed -= TestCock4Closed;
        Actions.onAddTestCockToList -= AddTestCockToList;
        Actions.onRemoveTestCockFromList -= RemoveTestCockFromList;
        Actions.onAddHoseToList -= AddHoseToList;
        Actions.onRemoveHoseFromList -= RemoveHoseFromList;

        // Actions.onHighBleedOpen -= HighBleedKnobOpened;
        // Actions.onHighBleedClosed -= HighBleedKnobClosed;

        //Actions.onTestCockOpen -= DetectTestCockOpen;
    }

    // Start is called before the first frame update
    void Start()
    {


        /// <summary>
        /// //ui tool kit for digital gauge
        /// </summary>
        /// <typeparam name="UIDocument"></typeparam>
        /// <returns></returns>
        // _root = GetComponent<UIDocument>().rootVisualElement;
        // _gaugeProgressBar = _root.rootVisualElement.Q<VisualElement>("Gauge_progress_bar");


        currentPSID = 0;
        minPSID = 0;
        maxPSID = 58;
        currentKnobRotation = 0;
        maxKnobRotation = 1440;
        minKnobRotation = 0;
        highHosePressure = 0;
        hosePressure = 0;
        initLowHosePosition = LowHose.transform.position;
        initHighHosePosition = HighHose.transform.position;
        initBypassHosePosition = BypassHose.transform.position;
    }


    private void AddHoseToList(GameObject @object, OperableComponentDescription description)
    {
        if (!AttachedHoseList.Contains(@object))
        {
            AttachedHoseList.Add(@object);
        }
    }
    private void RemoveHoseFromList(GameObject @object, OperableComponentDescription description)
    {

        if (AttachedHoseList.Contains(@object))
        {
            AttachedHoseList.Remove(@object);
        }
    }


    private void AddTestCockToList(GameObject @object, OperableComponentDescription description)
    {
        if (!AttachedTestCockList.Contains(@object))
        {
            AttachedTestCockList.Add(@object);
        }
    }

    private void RemoveTestCockFromList(GameObject @object, OperableComponentDescription description)
    {

        if (AttachedTestCockList.Contains(@object))
        {
            AttachedTestCockList.Remove(@object);
        }
    }

    private float GetPsidNeedleRotation()
    {
        float PsidDiff = MinNeedle_rotation - MaxNeedle_rotation;

        float normalizedPsid = hosePressure / maxPSID;
        // Debug.Log($"normalizedPsid: {normalizedPsid}");
        return MinNeedle_rotation - normalizedPsid * PsidDiff;

    }
    private float GetPsidDigitalNeedle()
    {
        float PsiDiff = MinFillPos - MaxFillPos;
        float percentChange = hosePressure;
        float normalizedPsid = hosePressure / maxPSID;

        return MinFillPos - normalizedPsid * PsiDiff;
        // Debug.Log($"hosePressure = {hosePressure}|| progressBar = {_gaugeProgressBar.style.width} || MinFillPos - percentChange = {MinFillPos - percentChange}");

    }


    public float GetKnobRotation()
    {
        // max - min to rotate left while increasing
        float rotationDiff = MaxKnob_rotation - MinKnob_rotation;

        float normalizedRotation = currentKnobRotation / maxKnobRotation;

        knobRotation = MinKnob_rotation + normalizedRotation * rotationDiff;
        //return MinKnob_rotation + normalizedRotation * rotationDiff;
        return MinKnob_rotation + normalizedRotation * rotationDiff * knobRotationFactor;
    }

    private void OperateControls()
    {
        if (playerController.isOperableObject == true)
        {
            if (
                playerController.operableComponentDescription.partsType
                == OperableComponentDescription.PartsType.TestKitValve
            )
            {
                switch (playerController.operableComponentDescription.componentId)
                {
                    case OperableComponentDescription.ComponentId.HighBleed:
                        currentKnob = highBleed;
                        break;
                    case OperableComponentDescription.ComponentId.LowBleed:
                        currentKnob = lowBleed;
                        break;
                    case OperableComponentDescription.ComponentId.LowControl:
                        currentKnob = lowControl;
                        break;
                    case OperableComponentDescription.ComponentId.HighControl:
                        currentKnob = highControl;
                        break;
                    case OperableComponentDescription.ComponentId.BypassControl:
                        currentKnob = bypassControl;
                        break;
                    default:
                        currentKnob = null;
                        break;
                }

                if (playerController.ClickOperationEnabled == false)
                {

                    // currentKnob = playerController.OperableTestGaugeObject;

                    //check click operation status

                    //if disabled, use click and drag

                    currentKnobRotation +=
                        (
                            playerController.touchStart.x
                            - Camera.main.ScreenToWorldPoint(Input.mousePosition).x
                        ) / 5;


                    if (currentKnobRotation > maxKnobRotation)
                    {
                        currentKnobRotation = maxKnobRotation;
                    }
                    if (currentKnobRotation < minKnobRotation)
                    {
                        currentKnobRotation = minKnobRotation;
                    }
                    if (currentKnob != null)
                        currentKnob.transform.eulerAngles = new Vector3(0, 0, GetKnobRotation());
                }
                //if enabled, spin to max rotation
                else if (playerController.ClickOperationEnabled == true)
                {
                    if (bleederHoseEmitter.VolumePerSimTime > 0)
                    {
                        KnobClickOperate = StartCoroutine(RotateKnobClosed(currentKnob, new Vector3(0, 0, 180)));

                    }
                    else
                    {
                        KnobClickOperate = StartCoroutine(RotateKnobOpen(currentKnob, new Vector3(0, 0, 180)));

                    }


                }

            }


        }
    }

    IEnumerator RotateKnobClosed(GameObject obj, Vector3 targetRotation)
    {
        float timeLerped = 0.0f;

        while (timeLerped < 1.0)
        {
            timeLerped += Time.deltaTime;
            obj.transform.eulerAngles = Vector3.Lerp(Vector3.zero, targetRotation, timeLerped) * 10;
            yield return null;
        }
    }
    IEnumerator RotateKnobOpen(GameObject obj, Vector3 targetRotation)
    {

        float timeLerped = 0.0f;
        knobOpened = true;
        while (timeLerped < 1.0)
        {
            timeLerped += Time.deltaTime;
            obj.transform.eulerAngles = -Vector3.Lerp(Vector3.zero, targetRotation, timeLerped) * 10;
            yield return null;
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


    private void NeedleControl()
    {
        // For  now, soely using high hose (double check assembly)
        needle.transform.eulerAngles = new Vector3(0, 0, GetPsidNeedleRotation());
    }
    private void DigitalNeedleControl()
    {
        // _gaugeProgressBar.style.width = Length.Percent(GetPsidDigitalNeedle());
    }
    private void PressureControl()
    {
        // For  now, soely using high hose (double check assembly testing)


        if (isConnectedToAssembly == true)
        {
            //checking if hose/ test kit is connected to test cock while supply is open

            //test cock #1 will have continuous pressure whether the supply is open or closed, as it sits upstream of #1 shut off valve
            if (AttachedTestCockList.Contains(TestCock1) && isTestCock1Open)
            {
                //supply is open and test cock is open
                hosePressure = Mathf.SmoothStep(
                    hosePressure,
                    Zone1Detector.ParticlesInside,
                    needleSpeedDamp
                );
                // Debug.Log($"test cock #1 is connected & open");
            }

            else if (
                AttachedTestCockList.Contains(TestCock3)
                && shutOffValveController.IsSupplyOn == true
                && isTestCock3Open
            )
            {
                //supply is open and test cock is open
                hosePressure = Mathf.SmoothStep(
                    hosePressure,
                    Zone2Detector.ParticlesInside,
                    needleSpeedDamp
                );
                // Debug.Log($"supply is open and test cock 3 is connected & open");
            }
            else if (
                AttachedTestCockList.Contains(TestCock4)
                && shutOffValveController.IsSupplyOn == true
                && isTestCock4Open
            )
            {
                //supply is open and test cock is open
                hosePressure = Mathf.SmoothStep(
                    hosePressure,
                    Zone3Detector.ParticlesInside,
                    needleSpeedDamp
                );
                // Debug.Log($"supply is open and test cock 4 is connected & open");
            }
            //END CHECKING IS TC IS HOOKED UP TO MOVE GAUGE WHILE DEVICE IS OPEN




            //========================================
            // #1 Check Test//========================>
            //========================================

            else if (
                AttachedTestCockList.Contains(TestCock2)
                && shutOffValveController.IsSupplyOn == true
                && isTestCock2Open
            // && !isTestCock3Open
            )
            {
                hosePressure = Mathf.SmoothStep(
                    hosePressure,
                    TestCock2Detector.ParticlesInside,
                    0.015f
                );
                // Debug.Log(
                //     $"supply is open & test cock #2 is connected & open & test cock #3 is closed"
                // );
            }
            else if (
                AttachedTestCockList.Contains(TestCock2)
                && isTestCock2Open
                && isTestCock3Open
                && shutOffValveController.IsSupplyOn == false
                && checkValveStatus.isCheck1Closed == false
            )
            {
                //best looking psid drop so far is: hosePressure -= 0.3f;
                // differnce ratio between windows to mac = 1:15
                //Windows----------------

                if (liquid.UseFixedTimestep == true)
                {
                    hosePressure -= 0.04f;
                }
                //!Windows----------------
                else
                {
                    hosePressure -= 0.65f;
                }

            }
            else if (
                AttachedTestCockList.Contains(TestCock2)
                && isTestCock2Open
                && isTestCock3Open
                && shutOffValveController.IsSupplyOn == false
                && checkValveStatus.isCheck1Closed == true
            )
            {
                hosePressure += 0;

            }
            //========================================
            // END - #1 Check Test//==================>
            //========================================
            //========================================
            // #2 Check Test//========================>
            //========================================
            else if (
                AttachedTestCockList.Contains(TestCock3)
                && shutOffValveController.IsSupplyOn == true
                && isTestCock3Open
            // && !isTestCock4Open
            )
            {
                hosePressure = Mathf.SmoothStep(
                    hosePressure,
                    TestCock2Detector.ParticlesInside,
                    0.1f
                );

            }
            else if (
                AttachedTestCockList.Contains(TestCock3)
                && isTestCock3Open
                && isTestCock4Open
                && shutOffValveController.IsSupplyOn == false
                && checkValveStatus.isCheck2Closed == false
            )
            {
                //best looking psid drop so far is: hosePressure -= 0.3f;
                //Windows----------------
                if (Application.platform == RuntimePlatform.WindowsPlayer)
                {
                    hosePressure -= 0.025f;
                }
                //OSX--------------------
                else
                {
                    hosePressure -= 0.4f;
                }


            }
            else if (
                AttachedTestCockList.Contains(TestCock3)
                && isTestCock3Open
                && isTestCock4Open
                && shutOffValveController.IsSupplyOn == false
                && checkValveStatus.isCheck2Closed == true
            )
            {
                hosePressure += 0;

            }
            //========================================
            // END - #2 Check Test//==================>
            //========================================
        }
        if (isConnectedToAssembly == false)
        {
            hosePressure -= 5;
        }
        if (hosePressure <= minPSID)
        {
            hosePressure = minPSID;
        }
        if (hosePressure > maxPSID)
        {
            hosePressure = maxPSID;
        }

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

        if (AttachedHoseList.Count > 0)
        {
            isConnectedToAssembly = true;
        }
        else
        {
            isConnectedToAssembly = false;
        }
        PressureControl();
        OperateControls();
        NeedleControl();
        DigitalNeedleControl();
        knobRotation = highBleed.transform.eulerAngles.z;

    }


}

