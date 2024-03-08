using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Timers;
using com.zibra.liquid.Manipulators;
using com.zibra.liquid.Solver;
using DG.Tweening;
using JetBrains.Annotations;
using Unity.VisualScripting;

using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UIElements;

public class DoubleCheckTestKitController : MonoBehaviour
{

    public DCWaterController dcWaterController;
    public PlayerController playerController;
    public ShutOffValveController shutOffValveController;
    public TestCockController testCockController;
    public CheckValveStatus checkValveStatus;
    public Animator openKnobAnimation;

    public BleedHoseController bleedHoseController;



    public ZibraLiquid liquid;
    public GameObject highBleed;
    public GameObject highControl;
    public GameObject lowBleed;
    public GameObject lowControl;
    public GameObject bypassControl;
    public GameObject currentKnobIndicator;
    // public PressureZoneHUDController pressureZoneHUDController;
    // public RPZPressureZoneHUDController pressureZoneHUDController;
    public DCPressureZoneHUDController pressureZoneHUDController;



    public GameObject needle;
    public GameObject digitalKitNeedle;
    public GameObject currentKnob;
    public GameObject highBleedIndicator;
    public GameObject lowBleedIndicator;
    public GameObject highControlIndicator;
    public GameObject lowControlIndicator;
    public GameObject bypassControlIndicator;
    public GameObject LowHose;
    public GameObject HighHose;
    public GameObject BypassHose;
    public GameObject SightTube;
    GameObject currentHighHoseConnection;


    Vector3 initLowHosePosition;
    Vector3 initHighHosePosition;
    Vector3 initBypassHosePosition;
    public HoseBib HighHoseBib;
    public HoseBib LowHoseBib;
    public HoseBib BypassHoseBib;
    public HoseBib m_SightTubeHoseBib;

    [SerializeField]
    ZibraLiquidEmitter bleederHoseEmitter;
    public ZibraLiquidEmitter bypassHoseEmitter;
    public ZibraLiquidEmitter lowHoseEmitter;
    public ZibraLiquidEmitter highHoseEmitter;
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

    Tween TweenColor;

    // private const float MinNeedle_rotation = 55;
    // private const float MaxNeedle_rotation = -55;
    public float MinNeedle_rotation = 135;
    public float MaxNeedle_rotation = -135;
    public float hosePressure;
    public float maxPSID;
    private const float MinKnob_rotation = 0;

    //limit knobs to 4 complete rotations (x1 rotation = 360;)->
    private const float MaxKnob_rotation = 1440;
    private float currentKnobRotCount;

    private float currentKnobRotation;
    private float maxKnobRotation;
    private float minKnobRotation;

    [SerializeField]
    private float currentPSID;

    private float minPSID;
    float closingPoint = 0;
    public bool isOperableObject;

    public CheckValveCollision isCheck1Open;
    public CheckValve2Collision isCheck2Open;
    float lowHosePressure;

    public bool isConnectedTestCockOpen;
    public bool isTestCock1Open;
    public bool isTestCock2Open;
    public bool isTestCock3Open;
    public bool isTestCock4Open;

    public bool isHighHoseEngaged;
    public bool isLowHoseEngaged;
    public bool isBypassHoseEngaged;

    public bool isHighHoseConnected;
    public bool isLowHoseConnected;
    public bool isBypassHoseConnected;

    public bool isHighBleedOpen;
    public bool isLowBleedOpen;
    public bool isLowControlOpen;
    public bool isHighControlOpen;
    public bool isBypassControlOpen;


    public float highHosePressure;
    float bypasshosePressure;
    float needleSpeedDamp = 0.005f;
    public float knobRotationFactor = 0;


    Coroutine m_ChangeColorOpen;
    Coroutine m_ChangeColorClose;
    Coroutine KnobClickOperate;
    Coroutine ReliefValveOpeningPoint;
    Coroutine ReliefValveOpeningPointReturn;
    Coroutine CheckValve1Test;
    Coroutine CheckValve2Test;
    Coroutine Zone2TestRecovery;

    float reliefValveInitValue;
    float needleVelRef = 0;
    public float lerpDuration = 0.5f;
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
    private float zone1to2PsiDiff;
    private float zone2to3PsiDiff;

    public float needleRiseSpeed = 0.1f;
    public List<GameObject> StaticTestCockList;
    // public List<GameObject> TestCockList;
    public List<GameObject> AttachedTestCockList;
    public List<GameObject> AttachedHoseList;
    Color openColor = new Color(0.121469043f, 0.830188692f, 0, 10);
    Color closedColor = new Color(0.860397637f, 0.0180187989f, 0, 10);
    int rot = 0;
    public ForceMode fMode;
    public Vector3 forceDir;
    public float fStrength;
    private const string EmissionColor = "_EmissionColor";
    public GameObject check1BackSeat;
    Vector3 check1BackSeatInitPos;
    Vector3 check1BackSeatClosedPos = new Vector3(-0.129500002f, 0f, -0.0860999972f);
    Vector3 check1BackSeatLeakingPos = new Vector3(-0.133900002f, 0, -0.0860999972f);
    public float RVOP;
    public float m_lowSideManifoldPressure;
    public float m_highSideManifoldPressure;
    public float differentialPressure;
    public float needleTweenSpeed;
    public float m_GaugeReading;
    HoseDetector m_hoseDetector1;
    HoseDetector m_hoseDetector2;
    HoseDetector m_hoseDetector3;
    HoseDetector m_hoseDetector4;
    public bool Check1TestComplete;
    public bool Check1TestInProgress;
    public float previousSupplyPsi;
    void OnEnable()
    {


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
        Actions.onHighBleedOperate += HighBleedKnobOperate;
        Actions.onLowBleedOperate += LowBleedKnobOperate;
        Actions.onHighControlOperate += HighControlKnobOperate;
        Actions.onLowControlOperate += LowControlKnobOperate;
        Actions.onBypassControlOperate += BypassControlKnobOperate;
        Actions.onSupplyOpen += SupplyShutOffOpen;
        Actions.onSupplyClosed += SupplyShutOffClosed;


    }



    void OnDisable()
    {

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
        Actions.onHighBleedOperate -= HighBleedKnobOperate;
        Actions.onLowBleedOperate -= LowBleedKnobOperate;
        Actions.onHighControlOperate -= HighControlKnobOperate;
        Actions.onLowControlOperate -= LowControlKnobOperate;
        Actions.onBypassControlOperate -= BypassControlKnobOperate;
        Actions.onSupplyOpen -= SupplyShutOffOpen;
        Actions.onSupplyClosed -= SupplyShutOffClosed;


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
        maxPSID = 1;
        currentKnobRotation = 0;
        maxKnobRotation = 1440;
        minKnobRotation = 0;
        highHosePressure = 0;
        hosePressure = 0;
        initLowHosePosition = LowHose.transform.position;
        initHighHosePosition = HighHose.transform.position;
        initBypassHosePosition = BypassHose.transform.position;
        check1BackSeatInitPos = check1BackSeat.transform.position;
        m_hoseDetector1 = dcWaterController.hoseDetector1.GetComponent<HoseDetector>();
        m_hoseDetector2 = dcWaterController.hoseDetector2.GetComponent<HoseDetector>();
        m_hoseDetector3 = dcWaterController.hoseDetector3.GetComponent<HoseDetector>();
        m_hoseDetector4 = dcWaterController.hoseDetector4.GetComponent<HoseDetector>(); ;

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

        return MinNeedle_rotation - normalizedPsid * PsidDiff;

    }
    private float GetPsidDigitalNeedle()
    {
        float PsiDiff = MinFillPos - MaxFillPos;

        float normalizedPsid = hosePressure / maxPSID;

        return MinFillPos - normalizedPsid * PsiDiff;


    }


    /// <summary>
    /// Knob controls
    /// </summary>
    #region 


    IEnumerator OpenColorChange(Material mat)
    {
        float percentage = 0;
        float startTime = Time.time;

        Color initialColor = mat.GetColor(EmissionColor);
        Color currentColor;

        while (percentage < 1f)
        {
            percentage = (Time.time - startTime) / lerpDuration;
            currentColor = Color.Lerp(initialColor, openColor, percentage);
            mat.SetColor(EmissionColor, currentColor);
            yield return null;
        }

    }

    IEnumerator CloseColorChange(Material mat)
    {
        float percentage = 0;
        float startTime = Time.time;

        Color initialColor = mat.GetColor(EmissionColor);
        Color currentColor;

        while (percentage < 1f)
        {
            percentage = (Time.time - startTime) / lerpDuration;
            currentColor = Color.Lerp(initialColor, closedColor, percentage);
            mat.SetColor(EmissionColor, currentColor);
            yield return null;
        }

    }

    public float GetKnobRotation()
    {
        // max - min to rotate left while increasing
        float rotationDiff = MaxKnob_rotation - MinKnob_rotation;

        float normalizedRotation = currentKnobRotation / maxKnobRotation;

        knobRotation = MinKnob_rotation + normalizedRotation * rotationDiff;

        return MinKnob_rotation + normalizedRotation * rotationDiff * knobRotationFactor;
    }

    void HighBleedKnobOperate()
    {


        currentKnob = highBleed;

        if (isHighBleedOpen == false)
        {
            isHighBleedOpen = true;

            m_ChangeColorOpen = StartCoroutine(OpenColorChange(highBleedIndicator.GetComponent<Renderer>().material));
            KnobClickOperate = StartCoroutine(RotateKnobOpen(currentKnob, new Vector3(0, 0, 180)));

        }
        else
        {
            isHighBleedOpen = false;

            m_ChangeColorClose = StartCoroutine(CloseColorChange(highBleedIndicator.GetComponent<Renderer>().material));
            KnobClickOperate = StartCoroutine(RotateKnobClosed(currentKnob, new Vector3(0, 0, 180)));

        }

    }
    private void LowBleedKnobOperate()
    {

        currentKnob = lowBleed;
        if (isLowBleedOpen == false)
        {
            isLowBleedOpen = true;

            m_ChangeColorOpen = StartCoroutine(OpenColorChange(lowBleedIndicator.GetComponent<Renderer>().material));
            KnobClickOperate = StartCoroutine(RotateKnobOpen(currentKnob, new Vector3(0, 0, 180)));

        }
        else
        {
            isLowBleedOpen = false;

            m_ChangeColorClose = StartCoroutine(CloseColorChange(lowBleedIndicator.GetComponent<Renderer>().material));
            KnobClickOperate = StartCoroutine(RotateKnobClosed(currentKnob, new Vector3(0, 0, 180)));
        }

    }
    private void BypassControlKnobOperate()
    {

        currentKnob = bypassControl;
        if (isBypassControlOpen == false)
        {
            isBypassControlOpen = true;
            m_ChangeColorOpen = StartCoroutine(OpenColorChange(bypassControlIndicator.GetComponent<Renderer>().material));
            KnobClickOperate = StartCoroutine(RotateKnobOpen(currentKnob, new Vector3(0, 0, 180)));




        }
        else
        {
            isBypassControlOpen = false;

            m_ChangeColorClose = StartCoroutine(CloseColorChange(bypassControlIndicator.GetComponent<Renderer>().material));
            KnobClickOperate = StartCoroutine(RotateKnobClosed(currentKnob, new Vector3(0, 0, 180)));
        }
    }

    private void LowControlKnobOperate()
    {

        currentKnob = lowControl;
        if (isLowControlOpen == false)
        {
            isLowControlOpen = true;
            m_ChangeColorOpen = StartCoroutine(OpenColorChange(lowControlIndicator.GetComponent<Renderer>().material));
            KnobClickOperate = StartCoroutine(RotateKnobOpen(currentKnob, new Vector3(0, 0, 180)));


        }
        else
        {
            isLowControlOpen = false;
            m_ChangeColorClose = StartCoroutine(CloseColorChange(lowControlIndicator.GetComponent<Renderer>().material));
            KnobClickOperate = StartCoroutine(RotateKnobClosed(currentKnob, new Vector3(0, 0, 180)));
        }
    }

    private void HighControlKnobOperate()
    {

        currentKnob = highControl;
        if (isHighControlOpen == false)
        {
            isHighControlOpen = true;
            m_ChangeColorOpen = StartCoroutine(OpenColorChange(highControlIndicator.GetComponent<Renderer>().material));
            KnobClickOperate = StartCoroutine(RotateKnobOpen(currentKnob, new Vector3(0, 0, 180)));
        }
        else
        {
            isHighControlOpen = false;
            m_ChangeColorClose = StartCoroutine(CloseColorChange(highControlIndicator.GetComponent<Renderer>().material));
            KnobClickOperate = StartCoroutine(RotateKnobClosed(currentKnob, new Vector3(0, 0, 180)));
        }
    }

    IEnumerator RotateKnobOpen(GameObject obj, Vector3 targetRotation)
    {

        float timeLerped = 0.0f;

        while (timeLerped < 1.0)
        {

            timeLerped += Time.deltaTime;
            obj.transform.eulerAngles = Vector3.Lerp(Vector3.zero, targetRotation, timeLerped) * 10;

            yield return null;
        }


    }
    IEnumerator RotateKnobClosed(GameObject obj, Vector3 targetRotation)
    {
        // Debug.Log($"{obj} rotated CLOSED");
        float timeLerped = 0.0f;
        knobOpened = true;
        while (timeLerped < 1.0)
        {
            timeLerped += Time.deltaTime;
            obj.transform.eulerAngles = -Vector3.Lerp(Vector3.zero, targetRotation, timeLerped) * 10;
            yield return null;
        }
    }

    /// <summary>
    /// End - Knob controls
    /// </summary>
    #endregion


    /// <summary>
    /// Test cocks
    /// </summary>
    #region 
    private void TestCock4Closed()
    {
        isTestCock4Open = false;
    }

    private void TestCoc4Opened()
    {
        isTestCock4Open = true;
        if (m_hoseDetector3.currentHoseConnection == HighHose && m_hoseDetector4.currentHoseConnection == SightTube)
        {
            if (dcWaterController.isDeviceInTestingCondititons)
            {
                StartCoroutine(TestCheck2());

            }

        }
    }

    private void TestCock3Closed()
    {
        isTestCock3Open = false;
    }

    private void TestCock3Opened()
    {
        isTestCock3Open = true;
        if (m_hoseDetector2.currentHoseConnection == HighHose && m_hoseDetector3.currentHoseConnection == SightTube)
        {
            if (dcWaterController.isDeviceInTestingCondititons)
            {
                previousSupplyPsi = dcWaterController.supplyPsi;
                StartCoroutine(TestCheck1());
            }

        }
    }

    private void TestCock2Closed()
    {
        isTestCock2Open = false;
    }

    private void TestCoc2Opened()
    {
        isTestCock2Open = true;

        //Debug.Log($"m_hoseDetector2.currentHoseConnection: {m_hoseDetector2.currentHoseConnection}");
    }

    private void TestCock1Closed()
    {
        isTestCock1Open = false;
    }

    private void TestCock1Opened()
    {
        isTestCock1Open = true;



    }

    #endregion


    private void SupplyShutOffOpen()
    {
        if (Check1TestComplete)
        {
            Check1TestInProgress = false;
            StartCoroutine(Check1TestRecover());
        }
    }

    private void SupplyShutOffClosed()
    {

    }
    private void HoseEmittersControl()
    {
        //High
        if (isHighControlOpen && !isHighHoseConnected && dcWaterController.m_detectorZone1.ParticlesInside > 1000)
        {


            if (!isLowHoseConnected && !isBypassHoseConnected)
            {

                highHoseEmitter.enabled = false;
            }
            if (isLowHoseEngaged || isBypassHoseEngaged)
            {


                highHoseEmitter.enabled = true;

            }
            else
            {

                highHoseEmitter.enabled = false;
            }

        }
        else
        {

            highHoseEmitter.enabled = false;
        }

        //Low

        if (isLowControlOpen && !isLowHoseConnected && dcWaterController.m_detectorZone1.ParticlesInside > 1000)
        {


            if (!isHighHoseConnected && !isBypassHoseConnected)
            {

                lowHoseEmitter.enabled = false;
            }
            if (isHighHoseEngaged || isBypassHoseEngaged)
            {


                lowHoseEmitter.enabled = true;

            }
            else
            {

                lowHoseEmitter.enabled = false;
            }

        }
        else
        {

            lowHoseEmitter.enabled = false;
        }
        //Bypass
        if (isBypassControlOpen && !isBypassHoseConnected && dcWaterController.m_detectorZone1.ParticlesInside > 1000)
        {


            if (!isLowHoseConnected && !isHighHoseConnected)
            {

                bypassHoseEmitter.enabled = false;
            }
            if (isLowHoseEngaged || isHighHoseEngaged)
            {


                bypassHoseEmitter.enabled = true;

            }
            else
            {

                bypassHoseEmitter.enabled = false;
            }

        }
        else
        {

            bypassHoseEmitter.enabled = false;
        }
    }
    private void NeedleControl()
    {

        needle.transform.eulerAngles = new Vector3(0, 0, GetPsidNeedleRotation());
    }
    private void DigitalNeedleControl()
    {
        // _gaugeProgressBar.style.width = Length.Percent(GetPsidDigitalNeedle());
    }


    private void BleederHoseControl()
    {
        #region     
        if (Zone1Detector.ParticlesInside > 100)
        {
            if (isHighHoseEngaged && isLowHoseEngaged)
            {
                if (isHighBleedOpen && isLowBleedOpen)
                {
                    bleederHoseEmitter.VolumePerSimTime = 1;
                }
                else if (!isHighBleedOpen && isLowBleedOpen)
                {
                    bleederHoseEmitter.VolumePerSimTime = 1;
                }
                else if (isHighBleedOpen && !isLowBleedOpen)
                {
                    bleederHoseEmitter.VolumePerSimTime = 1;
                }
                else
                {
                    bleederHoseEmitter.VolumePerSimTime = 0;
                }


            }
            else if (isHighHoseEngaged && !isLowHoseEngaged)
            {
                if (isHighBleedOpen && isLowBleedOpen)
                {
                    bleederHoseEmitter.VolumePerSimTime = 1;
                }
                else if (!isHighBleedOpen && isLowBleedOpen)
                {
                    bleederHoseEmitter.VolumePerSimTime = 0;
                }
                else if (isHighBleedOpen && !isLowBleedOpen)
                {
                    bleederHoseEmitter.VolumePerSimTime = 1;
                }
                else
                {
                    bleederHoseEmitter.VolumePerSimTime = 0;
                }

            }
            else if (!isHighHoseEngaged && isLowHoseEngaged)
            {
                if (isHighBleedOpen && isLowBleedOpen)
                {
                    bleederHoseEmitter.VolumePerSimTime = 1;
                }
                else if (!isHighBleedOpen && isLowBleedOpen)
                {
                    bleederHoseEmitter.VolumePerSimTime = 1;
                }
                else if (isHighBleedOpen && !isLowBleedOpen)
                {
                    bleederHoseEmitter.VolumePerSimTime = 0;
                }
                else
                {
                    bleederHoseEmitter.VolumePerSimTime = 0;
                }

            }
            else if (!isHighHoseEngaged && !isLowHoseEngaged)
            {
                bleederHoseEmitter.VolumePerSimTime = 0;
            }
        }
        #endregion
    }
    private void PressureControl()
    {

        //track zone pressures
        zone1to2PsiDiff = dcWaterController.zone1to2PsiDiff;
        zone2to3PsiDiff = dcWaterController.zone2to3PsiDiff;


        /// <summary>
        /// High Hose
        /// 
        /// Needle should not move unless the high hose is attached to a testcock and the testcock is open
        /// 
        /// IRL: The test kit is seperated into 3 parts : High-side, Low-side, and control manifold.
        /// 
        /// The High-side of the test kit can only receive pressure through a hose on the high control portion of the manifold.
        /// The Low-side of the test kit can only receive pressure through a hose on the low control portion of the manifold.
        ///
        /// the control valves on the manifold introduces pressure to common "bar". This common bar can recieve pressure from any of the control knobs if they are connected to a supply 
        /// source and are open, the supply source would be another hose on the manifold hooked up to pressure..Which is to say either of the three sections of the manifold can inrtroduce water pressure out of the other hoses connected to the manifold (once they are opened) 
        /// 
        /// </summary>
        if (AttachedHoseList.Contains(HighHose))
        {
            if (HighHoseBib.testCock == TestCock1)
            {
                if (isTestCock1Open)
                {
                    isHighHoseEngaged = true;

                }
                else
                {
                    isHighHoseEngaged = false;
                }

            }
            if (HighHoseBib.testCock == TestCock2)
            {
                isHighHoseConnected = true;
                if (isTestCock2Open)
                {
                    isHighHoseEngaged = true;

                }
                else
                {
                    isHighHoseEngaged = false;

                }
            }
            else
            {
                isHighHoseConnected = false;
            }
            if (HighHoseBib.testCock == TestCock3)
            {
                if (isTestCock3Open)
                {
                    isHighHoseEngaged = true;
                }
                else
                {
                    isHighHoseEngaged = false;
                }
            }

            if (HighHoseBib.testCock == TestCock4)
            {
                if (isTestCock4Open)
                {
                    isHighHoseEngaged = true;
                }
                else
                {
                    isHighHoseEngaged = false;
                }
            }


        }
        else
        {
            //hosePressure = Mathf.SmoothStep(hosePressure, 0, needleRiseSpeed);
            isHighHoseConnected = false;
            isHighHoseEngaged = false;

        }
        /// <summary>
        /// End - High Hose
        /// </summary>

        /// <summary>
        /// Low Hose
        /// </summary>
        if (AttachedHoseList.Contains(LowHose))
        {

            if (LowHoseBib.testCock == TestCock1)
            {
                if (isTestCock1Open)
                {
                    // Debug.Log($"low hose on tc#1 && tc#1 opened");
                }
                else
                {
                    // Debug.Log($"low hose on tc#1 && tc#1 closed");
                }

            }
            if (LowHoseBib.testCock == TestCock2)
            {
                if (isTestCock2Open)
                {
                    // Debug.Log($"low hose on tc#2 && tc#2 opened");
                }
                else
                {
                    // Debug.Log($"low hose on tc#2 && tc#2 closed");
                }
            }
            if (LowHoseBib.testCock == TestCock3)
            {
                isLowHoseConnected = true;
                if (isTestCock3Open)
                {
                    isLowHoseEngaged = true;


                }
                else
                {

                    isLowHoseEngaged = false;
                    // Debug.Log($"low hose on tc#3 && tc#3 closed");
                }
            }
            else
            {
                isLowHoseConnected = false;
            }

            if (LowHoseBib.testCock == TestCock4)
            {
                if (isTestCock4Open)
                {
                    // Debug.Log($"low hose on tc#4 && tc#4 opened");
                }
                else
                {
                    // Debug.Log($"low hose on tc#4 && tc#4 closed");
                }
            }
        }
        else
        {
            isLowHoseConnected = false;

        }
        /// <summary>
        /// End - Low Hose
        /// </summary>


        /// <summary>
        /// Bypass Hose
        /// </summary>
        if (AttachedHoseList.Contains(BypassHose))
        {
            if (BypassHoseBib.testCock == TestCock1)
            {
                if (isTestCock1Open)
                {
                    // Debug.Log($"bypass hose on tc#1 && tc#1 opened");
                }
                else
                {
                    // Debug.Log($"bypass hose on tc#1 && tc#1 closed");
                }

            }
            if (BypassHoseBib.testCock == TestCock2)
            {
                if (isTestCock2Open)
                {
                    // Debug.Log($"bypass hose on tc#2 && tc#2 opened");
                }
                else
                {
                    // Debug.Log($"bypass hose on tc#2 && tc#2 closed");
                }
            }
            if (BypassHoseBib.testCock == TestCock3)
            {
                if (isTestCock3Open)
                {

                    // Debug.Log($"bypass hose on tc#3 && tc#3 opened");
                }
                else
                {

                    // Debug.Log($"bypass hose on tc#3 && tc#3 closed");
                }
            }

            if (BypassHoseBib.testCock == TestCock4)
            {
                isBypassHoseConnected = true;
                bypassHoseEmitter.enabled = false;
                if (isTestCock4Open)
                {
                    isBypassHoseEngaged = true;

                    // Debug.Log($"bypass hose on tc#4 && tc#4 opened");
                }
                else
                {
                    isBypassHoseEngaged = false;
                    // Debug.Log($"bypass hose on tc#4 && tc#4 closed");
                }
            }
            else
            {
                isBypassHoseConnected = false;
            }
        }
        else
        {
            isBypassHoseConnected = false;
        }
        /// <summary>
        /// End - Bypass Hose
        /// </summary>


        /*
                    //========================================
                    // Start Test Procedures//========================>
                    //========================================

                    //check if device is ready for test (so#1 open & so#2 closed)
                    if (waterController.m_detectorZone1.ParticlesInside > 1000)
                    {
                        if (isHighHoseEngaged == true)
                        {
                            hosePressure = Mathf.SmoothStep(hosePressure, maxPSID, needleRiseSpeed);
                        }

                        else if (!isHighHoseConnected)
                        {

                            //nothing attached, remove pressure from test gauge
                            hosePressure = Mathf.SmoothStep(
                                             hosePressure,
                                             0,
                                             needleRiseSpeed
                                         );


                        }

                        if (isHighHoseEngaged == true && HighHoseBib.testCock == TestCock2 && m_SightTubeHoseBib.testCock == TestCock3 && isTestCock3Open)
                        {

                            if (isCheck1Open == false)
                            {
                                if (waterController.isDeviceInTestingCondititons)

                                    CheckValve1Test = StartCoroutine(TestCheck1());
                                hosePressure -= 0.1f;
                            }
                            else
                            {
                                StopCoroutine(TestCheck1());
                            }



                        }
                        else
                        {
                            Zone2TestRecovery = StartCoroutine(StopTestCheck1());
                        }
                        // else if (isHighHoseEngaged == true)
                        // {
                        //     hosePressure = Mathf.SmoothStep(hosePressure, maxPSID, needleRiseSpeed);
                        // }

                        //========================================
                        // Check Valve #1//========================>
                        //========================================


                        //========================================
                        // END - Check Valve #1//==================>
                        //========================================





                        //========================================
                        // Check Valve #2//========================>
                        //========================================


                        //========================================
                        // END - Check Valve #2//==================>
                        //========================================






                    }
        */



        //========================================
        // Check Valve #2//========================>
        //========================================


        //========================================
        // END - Check Valve #2//==================>
        //========================================









        //========================================
        // End Test Procedures//========================>
        //========================================






        //keep gauge pressure maxed out if highhose is at least engaged--



    }
    public float HighSide(float highSideManifoldPressure)
    {
        // {
        //     highSideManifoldPressure = m_highSideManifoldPressure;
        //     lowSideManifoldPressure = m_lowSideManifoldPressure;
        //     bypassSideManifoldPressure = m_bypassSideManifoldPressure;

        return highSideManifoldPressure * 0.1f;
    }
    public float LowSide(float lowSideManifoldPressure)
    {
        return lowSideManifoldPressure * 0.1f;


    }
    public void DifferentialGauge()
    {



        if (hosePressure <= maxPSID)
        {

            if (isHighHoseEngaged)
            {
                //max out gauge---> m_highSideManifoldPressure = rpzWaterController.zone1Pressure / 10 * 0.1f;

                m_highSideManifoldPressure = dcWaterController.zone1Pressure;


            }
            else if (!isHighHoseConnected)
            {
                m_highSideManifoldPressure = 0.0f;

            }

            if (differentialPressure <= maxPSID)
            {

                differentialPressure = HighSide(m_highSideManifoldPressure) - LowSide(m_lowSideManifoldPressure);

            }


            if (differentialPressure > maxPSID)
            {
                differentialPressure = maxPSID;
            }

            if (Check1TestInProgress && !shutOffValveController.IsSupplyOn)
            {
                m_lowSideManifoldPressure = dcWaterController.zone2Pressure - dcWaterController.check1SpringForce;
            }
            // if (shutOffValveController.IsSupplyOn && !Check1TestInProgress)
            // {

            //     m_lowSideManifoldPressure = 0;
            // }


            DOTween.To(()
                         => hosePressure,
                         x => hosePressure = x,
                          differentialPressure, needleTweenSpeed).SetEase(Ease.Linear);


            m_GaugeReading = hosePressure * 10;
        }
        if (hosePressure > maxPSID)
        {

            hosePressure = maxPSID;
        }
        if (hosePressure < 0)
        {
            hosePressure = 0;

        }

    }
    private IEnumerator TestCheck1()
    {

        while (dcWaterController.zone1to2PsiDiff > 0)
        {

            Check1TestComplete = false;
            Check1TestInProgress = true;
            needleTweenSpeed = 0.1f;
            //pressureZoneHUDController.m_PressureZone2PanelSlider.value += 0.1f * Time.deltaTime * sliderTweenSpeed;


            // newVal -= 0.01f * Time.deltaTime;
            // pressureZoneHUDController.m_SupplyPressurePanelSlider.SetValueWithoutNotify(pressureZoneHUDController.m_SupplyPressurePanelSlider.value - newVal);
            dcWaterController.supplyPsi -= 0.01f * Time.deltaTime;
            var newVal = dcWaterController.zone1Pressure;
            pressureZoneHUDController.m_SupplyPressurePanelSlider.SetValueWithoutNotify(newVal);
            dcWaterController.zone2PsiChange += 0.01f * Time.deltaTime;

            // pressureZoneHUDController.m_PressureZone2PanelSlider.SetValueWithoutNotify(dcWaterController.zone1Pressure - pressureZoneHUDController.m_PressureZone2PanelSlider.value + newVal);

            if (newVal < 1)
            {
                // pressureZoneHUDController.m_Zone2PressureSliderValue.text = "+" + ((int)newVal).ToString();
                pressureZoneHUDController.m_Zone2PressureSliderValue.text = "+0";
                pressureZoneHUDController.m_SupplyPressureTextField.text = ((int)dcWaterController.zone2PsiChange).ToString();
            }
            else
            {
                pressureZoneHUDController.m_Zone2PressureSliderValue.text = "+" + ((int)dcWaterController.zone2PsiChange).ToString();
                pressureZoneHUDController.m_SupplyPressureTextField.text = ((int)newVal).ToString();
            }



            // Debug.Log($"test check#1 in progress");


            yield return null;
        }
        // if (dcWaterController.isReliefValveOpen)
        //     pressureZoneHUDController.m_Zone2PressureSliderValue.text = "+" + (rvop - rpzWaterController.zone1Pressure + rpzWaterController.zone1Pressure - rpzWaterController.check1SpringForce) * -1;
        Check1TestComplete = true;



    }

    private IEnumerator TestCheck2()
    {


        while (dcWaterController.zone2to3PsiDiff > 0)
        {

            needleTweenSpeed = 0.1f;
            //pressureZoneHUDController.m_PressureZone2PanelSlider.value += 0.1f * Time.deltaTime * sliderTweenSpeed;


            // newVal -= 0.01f * Time.deltaTime;
            // pressureZoneHUDController.m_SupplyPressurePanelSlider.SetValueWithoutNotify(pressureZoneHUDController.m_SupplyPressurePanelSlider.value - newVal);
            dcWaterController.supplyPsi -= 0.01f * Time.deltaTime;
            var newVal = dcWaterController.zone1Pressure;
            pressureZoneHUDController.m_SupplyPressurePanelSlider.SetValueWithoutNotify(newVal);
            dcWaterController.zone2PsiChange += 0.01f * Time.deltaTime;
            dcWaterController.zone3PsiChange += 0.01f * Time.deltaTime;
            pressureZoneHUDController.m_PressureZone2PanelSlider.SetValueWithoutNotify(dcWaterController.zone1Pressure - pressureZoneHUDController.m_PressureZone2PanelSlider.value + newVal);
            pressureZoneHUDController.m_PressureZone3PanelSlider.SetValueWithoutNotify(dcWaterController.zone2Pressure - pressureZoneHUDController.m_PressureZone3PanelSlider.value + newVal);

            if (newVal < 1)
            {
                // pressureZoneHUDController.m_Zone2PressureSliderValue.text = "+" + ((int)newVal).ToString();
                pressureZoneHUDController.m_Zone2PressureSliderValue.text = "+0";
                pressureZoneHUDController.m_Zone3PressureSliderValue.text = "+0";
                pressureZoneHUDController.m_SupplyPressureTextField.text = ((int)newVal).ToString();
            }
            else
            {
                pressureZoneHUDController.m_SupplyPressureTextField.text = ((int)newVal).ToString();
                pressureZoneHUDController.m_Zone2PressureSliderValue.text = "+" + ((int)dcWaterController.zone2PsiChange).ToString();
                pressureZoneHUDController.m_Zone3PressureSliderValue.text = "+" + ((int)dcWaterController.zone3PsiChange).ToString();

            }



            // Debug.Log($"test check#1 in progress");


            yield return null;
        }

    }
    private IEnumerator Check1TestRecover()
    {
        while (dcWaterController.zone2PsiChange > 0)
        {


            needleTweenSpeed = 0.1f;
            //pressureZoneHUDController.m_PressureZone2PanelSlider.value += 0.1f * Time.deltaTime * sliderTweenSpeed;


            // newVal -= 0.01f * Time.deltaTime;
            // pressureZoneHUDController.m_SupplyPressurePanelSlider.SetValueWithoutNotify(pressureZoneHUDController.m_SupplyPressurePanelSlider.value - newVal);
            dcWaterController.supplyPsi += 0.01f * Time.deltaTime;
            var newVal = dcWaterController.zone1Pressure;
            pressureZoneHUDController.m_SupplyPressurePanelSlider.SetValueWithoutNotify(newVal);
            dcWaterController.zone2PsiChange -= 0.01f * Time.deltaTime;

            // pressureZoneHUDController.m_PressureZone2PanelSlider.SetValueWithoutNotify(dcWaterController.zone1Pressure - pressureZoneHUDController.m_PressureZone2PanelSlider.value + newVal);

            if (newVal < 1)
            {
                // pressureZoneHUDController.m_Zone2PressureSliderValue.text = "+" + ((int)newVal).ToString();
                pressureZoneHUDController.m_Zone2PressureSliderValue.text = "+0";
                pressureZoneHUDController.m_SupplyPressureTextField.text = ((int)dcWaterController.zone2PsiChange).ToString();
            }
            else
            {
                pressureZoneHUDController.m_Zone2PressureSliderValue.text = "+" + ((int)dcWaterController.zone2PsiChange).ToString();
                pressureZoneHUDController.m_SupplyPressureTextField.text = ((int)newVal).ToString();
            }



            // Debug.Log($"test check#1 in progress");


            yield return null;
        }

    }


    private IEnumerator StopTestCheck2()
    {
        while (pressureZoneHUDController.m_PressureZone2PanelSlider.value >= 0)
        // while
        {

            pressureZoneHUDController.m_PressureZone2PanelSlider.value -= 1.0f / (1000 * 0.1f);

            yield return new WaitForSeconds(1);
        }

    }

    // private IEnumerator TestRVOP()
    // {

    //     // while (pressureZoneHUDController.m_PressureZone2PanelSlider.value <= 6)
    //     // while (waterController.zone1Pressure - (waterController.zone1Pressure - waterController.check1SpringForce) > RVOP)
    //     while (waterController.isReliefValveOpen == false)
    //     // while
    //     {

    //         pressureZoneHUDController.m_PressureZone2PanelSlider.value += 1.0f / (1000 * 0.1f);

    //         yield return new WaitForSeconds(1);
    //     }

    // }

    // private IEnumerator StopTestRVOP()
    // {
    //     while (pressureZoneHUDController.m_PressureZone2PanelSlider.value >= 0)
    //     // while
    //     {

    //         pressureZoneHUDController.m_PressureZone2PanelSlider.value -= 1.0f / (1000 * 0.1f);

    //         yield return new WaitForSeconds(1);
    //     }

    // }




    // Update is called once per frame
    void Update()
    {
        DifferentialGauge();
        PressureControl();

        BleederHoseControl();
        NeedleControl();
        DigitalNeedleControl();
        HoseEmittersControl();


    }

}