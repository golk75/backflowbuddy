
using System.Collections;
using System.Collections.Generic;
using com.zibra.liquid.Manipulators;
using com.zibra.liquid.Solver;
using DG.Tweening;
using UnityEngine;
using UnityEngine.UIElements;

public class RpzTestKitController : MonoBehaviour
{
    public DCWaterController waterController;
    public PlayerController playerController;
    public ShutOffValveController shutOffValveController;
    public TestCockController testCockController;
    public CheckValveStatus checkValveStatus;
    public Animator openKnobAnimation;
    public RPZWaterController rpzWaterController;
    public BleedHoseController bleedHoseController;



    public ZibraLiquid liquid;

    public GameObject highBleed;
    public GameObject highControl;
    public GameObject lowBleed;
    public GameObject lowControl;
    public GameObject bypassControl;
    public GameObject currentKnobIndicator;
    // public PressureZoneHUDController pressureZoneHUDController;
    public RPZPressureZoneHUDController pressureZoneHUDController;

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

    public HoseBib HighHoseBib;
    public HoseBib LowHoseBib;
    public HoseBib BypassHoseBib;
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

    // private const float MinNeedle_rotation = 55;
    // private const float MaxNeedle_rotation = -55;
    public float MinNeedle_rotation = 135;
    public float MaxNeedle_rotation = -135;
    public float hosePressure;

    public float DifferentialPresure { get { return differentialPressure; } private set { differentialPressure = value; } }
    [SerializeField]
    private float differentialPressure;
    public int maxPSID;
    private const float MinKnob_rotation = 0;

    //limit knobs to 4 complete rotations (x1 rotation = 360;)->
    private const float MaxKnob_rotation = 1440;

    private float currentKnobRotation;
    private float maxKnobRotation;
    public bool isOperableObject;

    public bool isCheck1Open;
    public bool isCheck2Open;


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

    public bool isDeviceBleeding;
    public bool isDeviceBled;
    public bool isApparentReadingShown;

    public float needleSpeedDamp = 0.005f;
    public float knobRotationFactor = 0;


    Coroutine m_ChangeColorOpen;
    Coroutine m_ChangeColorClose;
    Coroutine KnobClickOperate;
    Coroutine ReliefValveOpeningPoint;
    public float lerpDuration = 0.5f;
    public bool knobOpened = false;
    public float needleLerpDuration = 0.5f;

    //ui toolkit
    public UIDocument _root;
    //public VisualElement _root;
    // private VisualElement _gaugeProgressBar;
    // private Length MinFillPos = Length.Percent(0);
    // private Length MaxFillPos = Length.Percent(100);
    //digital gauge
    // private float MinFillPos = 0;
    // private float MaxFillPos = 100;
    public float knobRotation;
    private float zone1to2PsiDiff;
    private float zone2to3PsiDiff;

    public float needleRiseSpeed;
    public List<GameObject> StaticTestCockList;
    // public List<GameObject> TestCockList;
    public List<GameObject> AttachedTestCockList;
    public List<GameObject> AttachedHoseList;
    Color openColor = new Color(0.121469043f, 0.830188692f, 0, 10);
    Color closedColor = new Color(0.860397637f, 0.0180187989f, 0, 10);
    public ForceMode fMode;
    public Vector3 forceDir;
    public float fStrength;
    private const string EmissionColor = "_EmissionColor";
    public GameObject check1BackSeat;
    public float RVOP;
    private float hosePressureRef = 0;
    public float apparentReading;
    public float preRvopReading;
    public bool rvopTestInprogress;
    public float rvop = 2;
    public float previousZone2SliderVal;

    public float m_highManifoldHosePressure;
    public float m_lowManifoldHosePressure;
    public float m_bypassManifoldHosePressure;
    public float m_highSideManifoldPressure;
    public float m_lowSideManifoldPressure;
    public float m_bypassSideManifoldPressure;
    public float m_GaugeReading;
    public Ease sliderEase;
    public float needleTweenSpeed;
    public float sliderTweenSpeed;
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




        maxPSID = 1;
        currentKnobRotation = 0;
        maxKnobRotation = 1440;
        hosePressure = 0;




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
    // private float GetPsidDigitalNeedle()
    // {
    //     float PsiDiff = MinFillPos - MaxFillPos;

    //     float normalizedPsid = hosePressure / maxPSID;

    //     return MinFillPos - normalizedPsid * PsiDiff;


    // }


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

            if (isDeviceBled && isHighHoseEngaged && isLowHoseEngaged && !shutOffValveController.IsSecondShutOffOpen)
            {
                //apparent reading

                // StartCoroutine(ApparrentReading());


            }
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
            if (isHighHoseEngaged && m_GaugeReading > 0)
            {
                StartCoroutine(BleedTestKit());
            }

            // if (isHighHoseEngaged && isLowControlOpen && isHighControlOpen && !isBypassControlOpen)
            // {

            //     //  ReliefValveOpeningPointReturn = StartCoroutine(StopTestRVOP());


            // }
        }
        else
        {

            isLowBleedOpen = false;
            // /if (isDeviceBled && !isHighBleedOpen && !shutOffValveController.IsSecondShutOffOpen)


            // if (isDeviceBled && !isLowBleedOpen && !isHighBleedOpen && !isLowControlOpen && !shutOffValveController.IsSecondShutOffOpen)
            // {
            //     StartCoroutine(ApparrentReading());
            // }

            if (isDeviceBled && isHighHoseEngaged && isLowHoseEngaged && !isHighBleedOpen && !shutOffValveController.IsSecondShutOffOpen)
            {
                //apparent reading

                StartCoroutine(ApparrentReading());


            }



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

            if (isHighHoseEngaged && isLowControlOpen && isHighControlOpen && !isBypassControlOpen)
            {
                ReliefValveOpeningPoint = StartCoroutine(TestRVOP());

            }



        }
        else
        {
            isLowControlOpen = false;
            if (rpzWaterController.isReliefValveOpen)
            {
                StartCoroutine(RvopRecover());
            }
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
        knobOpened = true;
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
        knobOpened = false;
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

    #endregion

    private void HoseEmittersControl()
    {
        //High
        if (isHighControlOpen && !isHighHoseConnected && rpzWaterController.m_detectorZone1.ParticlesInside > 1000)
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

        if (isLowControlOpen && !isLowHoseConnected && rpzWaterController.m_detectorZone1.ParticlesInside > 1000)
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
        if (isBypassControlOpen && !isBypassHoseConnected && rpzWaterController.m_detectorZone1.ParticlesInside > 1000)
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
        zone1to2PsiDiff = rpzWaterController.zone1to2PsiDiff;
        zone2to3PsiDiff = rpzWaterController.zone2to3PsiDiff;



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
            isHighHoseConnected = true;
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
            // hosePressure = Mathf.SmoothStep(hosePressure, 0, needleRiseSpeed);

            isHighHoseConnected = false;
            isHighHoseEngaged = false;
            isDeviceBled = false;

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




    }
    private IEnumerator RvopRecover()
    {
        needleTweenSpeed = 0.1f;
        //pressureZoneHUDController.m_PressureZone2PanelSlider.value += 0.1f * Time.deltaTime * sliderTweenSpeed;
        var newVal = pressureZoneHUDController.m_PressureZone2PanelSlider.value;
        while (rpzWaterController.isReliefValveOpen)
        {

            //newVal += 0.1f * Time.deltaTime * sliderTweenSpeed;
            pressureZoneHUDController.m_PressureZone2PanelSlider.SetValueWithoutNotify(newVal);
            rpzWaterController.zone2PsiChange -= 0.01f;
            if (newVal < 1)
            {
                // pressureZoneHUDController.m_Zone2PressureSliderValue.text = "+" + ((int)newVal).ToString();
                pressureZoneHUDController.m_Zone2PressureSliderValue.text = "+0";

            }
            else
            {
                pressureZoneHUDController.m_Zone2PressureSliderValue.text = "+" + ((int)newVal).ToString();
            }
            yield return null;
        }


    }
    private IEnumerator TestRVOP()
    {
        var newVal = pressureZoneHUDController.m_PressureZone2PanelSlider.value;
        while (!rpzWaterController.isReliefValveOpen && isLowControlOpen && !isLowBleedOpen)
        {

            needleTweenSpeed = 0.1f;
            //pressureZoneHUDController.m_PressureZone2PanelSlider.value += 0.1f * Time.deltaTime * sliderTweenSpeed;


            newVal += 0.1f * Time.deltaTime * 10;
            pressureZoneHUDController.m_PressureZone2PanelSlider.SetValueWithoutNotify(newVal);
            rpzWaterController.zone2PsiChange += 0.1f * Time.deltaTime * 10;
            if (newVal < 1)
            {
                // pressureZoneHUDController.m_Zone2PressureSliderValue.text = "+" + ((int)newVal).ToString();
                pressureZoneHUDController.m_Zone2PressureSliderValue.text = "+0";

            }
            else
            {
                pressureZoneHUDController.m_Zone2PressureSliderValue.text = "+" + ((int)newVal).ToString();
            }



            //  Debug.Log($"rvop test in progress");


            yield return null;
        }
        if (rpzWaterController.isReliefValveOpen)
            pressureZoneHUDController.m_Zone2PressureSliderValue.text = "+" + (rvop - rpzWaterController.zone1Pressure + rpzWaterController.zone1Pressure - rpzWaterController.check1SpringForce) * -1;



    }
    private IEnumerator ApparrentReading()
    {
        while (isLowBleedOpen == false && differentialPressure > rpzWaterController.check1SpringForce * 0.1f)
        {
            needleTweenSpeed = 0.1f;
            //pressureZoneHUDController.m_PressureZone2PanelSlider.value += 0.1f * Time.deltaTime * sliderTweenSpeed;
            var newVal = pressureZoneHUDController.m_PressureZone2PanelSlider.value;


            //newVal += 0.1f * Time.deltaTime * sliderTweenSpeed;
            pressureZoneHUDController.m_PressureZone2PanelSlider.SetValueWithoutNotify(newVal);
            rpzWaterController.zone2PsiChange += 0.01f;
            if (newVal < 1)
            {
                // pressureZoneHUDController.m_Zone2PressureSliderValue.text = "+" + ((int)newVal).ToString();
                pressureZoneHUDController.m_Zone2PressureSliderValue.text = "+0";

            }
            else
            {
                pressureZoneHUDController.m_Zone2PressureSliderValue.text = "+" + ((int)newVal).ToString();
            }




            //    Debug.Log($"falling to apparent reading");






            yield return null;


        }
        if (isDeviceBled && !isHighBleedOpen && isLowControlOpen)
        {
            StartCoroutine(TestRVOP());
        }

    }
    private IEnumerator BleedTestKit()
    {

        while (m_GaugeReading < 10 && isLowBleedOpen)
        {
            needleTweenSpeed = 0.1f;
            //pressureZoneHUDController.m_PressureZone2PanelSlider.value += 0.1f * Time.deltaTime * sliderTweenSpeed;
            sliderTweenSpeed = 10;
            var newVal = pressureZoneHUDController.m_PressureZone2PanelSlider.value;
            if (newVal > 0)
            {
                newVal -= 1f * Time.deltaTime * sliderTweenSpeed;
            }
            else
            {
                newVal = 0;
            }
            pressureZoneHUDController.m_PressureZone2PanelSlider.SetValueWithoutNotify(newVal);
            //rpzWaterController.zone2PsiChange -= 0.01f;
            // rpzWaterController.zone2PsiChange -= 1f * Time.deltaTime * sliderTweenSpeed;
            if (rpzWaterController.zone2Pressure > rpzWaterController.zone1Pressure - 10)
            {
                rpzWaterController.zone2PsiChange -= 1f * Time.deltaTime * sliderTweenSpeed;
            }


            if (newVal < 1)
            {
                // pressureZoneHUDController.m_Zone2PressureSliderValue.text = "+" + ((int)newVal).ToString();
                pressureZoneHUDController.m_Zone2PressureSliderValue.text = "+0";
                // m_lowSideManifoldPressure = 0;

            }
            else
            {
                pressureZoneHUDController.m_Zone2PressureSliderValue.text = "+" + ((int)newVal).ToString();
            }





            //Debug.Log($"bleeding in progress");


            yield return null;
        }
        isDeviceBled = true;
        sliderTweenSpeed = 6;


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
    public float Manifold()
    {

        //highHosePressure will always be assumed as rpzWaterController.zone1Pressure
        if (isHighHoseEngaged && !isLowControlOpen && !isHighControlOpen && !isBypassControlOpen)
        {
            m_highManifoldHosePressure = rpzWaterController.zone1Pressure;

        }
        else if (isHighHoseEngaged && !isLowControlOpen && isHighControlOpen && !isBypassControlOpen)
        {

            m_lowManifoldHosePressure = 0;
            m_bypassManifoldHosePressure = 0;

        }
        else if (isHighHoseEngaged && isLowControlOpen && isHighControlOpen && !isBypassControlOpen)
        {
            //rvop test
            if (!shutOffValveController.IsSecondShutOffOpen)
            {
                // needleSpeedDamp = 0.9f;
                //pressureZoneHUDController.m_PressureZone2PanelSlider.value = Mathf.Ceil(rpzWaterController.check1SpringForce - rvop);
                //pressureZoneHUDController.m_PressureZone2PanelSlider.value = Mathf.SmoothDamp(pressureZoneHUDController.m_PressureZone2PanelSlider.value, Mathf.Ceil(rpzWaterController.check1SpringForce - rvop), ref rvopRef, needleSpeedDamp);
                // DOTween.To(()
                //            => pressureZoneHUDController.m_PressureZone2PanelSlider.value,
                //            x => pressureZoneHUDController.m_PressureZone2PanelSlider.value = x,
                //            rpzWaterController.check1SpringForce - (rvop - 1f), 1.0f).SetEase(sliderEase)
                //            ;
                var high = rpzWaterController.zone1Pressure;
                var low = rpzWaterController.zone1Pressure - rpzWaterController.check1SpringForce;
                float endVal = (rvop - high + low) * -1;

                //DOTween.To(() => pressureZoneHUDController.m_PressureZone2PanelSlider.value, x => pressureZoneHUDController.m_PressureZone2PanelSlider.value = Mathf.Round(x * 10) * 0.1f, endVal, sliderTweenSpeed);
                // DOTween.To(()
                //          => pressureZoneHUDController.m_PressureZone2PanelSlider.value,
                //          x => pressureZoneHUDController.m_PressureZone2PanelSlider.value = x,
                //     (rvop - rpzWaterController.zone1Pressure + rpzWaterController.zone1Pressure - rpzWaterController.check1SpringForce) * -1, needleTweenSpeed).SetEase(Ease.Linear)
                //          ;
                //ReliefValveOpeningPoint = StartCoroutine(TestRVOP());
                //pressureZoneHUDController.m_PressureZone2PanelSlider.value = Mathf.Round(pressureZoneHUDController.m_PressureZone2PanelSlider.value * 10) * 0.1f;
                //pressureZoneHUDController.m_PressureZone2PanelSlider.value = endVal;
                //pressureZoneHUDController.m_PressureZone2PanelSlider.value = endVal;
                //DOTween.To(() => pressureZoneHUDController.m_PressureZone2PanelSlider.value, x => pressureZoneHUDController.m_PressureZone2PanelSlider.value = x, endVal, sliderTweenSpeed).SetEase(Ease.Linear);
                Debug.Log($"m_lowSideManifoldPressure : {m_lowSideManifoldPressure}; pressureZoneHUDController.m_PressureZone2PanelSlider.value: {pressureZoneHUDController.m_PressureZone2PanelSlider.value}");
            }

        }
        if (isHighHoseEngaged && !isLowControlOpen && isHighControlOpen && !isBypassControlOpen)
        {
            if (!shutOffValveController.IsSecondShutOffOpen)
            {
                pressureZoneHUDController.m_PressureZone2PanelSlider.value = 0;
                needleSpeedDamp = 0.5f;

            }
        }



        return (m_highSideManifoldPressure - (m_highManifoldHosePressure - (m_lowManifoldHosePressure + m_bypassManifoldHosePressure))) * 0.1f;
    }

    public void DifferentialGauge()
    {



        if (hosePressure <= maxPSID)
        {

            if (isHighHoseEngaged)
            {
                //max out gauge---> m_highSideManifoldPressure = rpzWaterController.zone1Pressure / 10 * 0.1f;

                m_highSideManifoldPressure = rpzWaterController.zone1Pressure;


            }
            else if (!isHighHoseConnected)
            {
                m_highSideManifoldPressure = 0.0f;

            }

            if (isLowHoseEngaged && isHighHoseEngaged)
            {
                if (!shutOffValveController.IsSecondShutOffOpen)
                {

                    if (isDeviceBled && !isLowBleedOpen && !isHighBleedOpen && !isLowControlOpen)
                    {
                        m_lowSideManifoldPressure = rpzWaterController.zone2Pressure;

                    }
                    if (isLowBleedOpen)
                    {

                        isDeviceBleeding = true;
                        m_lowSideManifoldPressure = 0;
                        // Debug.Log($"m_lowSideManifoldPressure: bleeding {m_lowSideManifoldPressure / 10}; m_highSideManifoldPressure: {m_highSideManifoldPressure / 10}");
                    }
                    if (isHighControlOpen && isLowControlOpen)
                    {
                        m_lowSideManifoldPressure = rpzWaterController.zone2Pressure;
                    }
                    if (isDeviceBled && !isLowControlOpen && isHighControlOpen)
                    {
                        m_lowSideManifoldPressure = rpzWaterController.zone2Pressure;
                    }
                    if (isDeviceBled && !isHighBleedOpen && !isLowBleedOpen)
                    {
                        m_lowSideManifoldPressure = rpzWaterController.zone2Pressure;
                    }

                    if (isDeviceBleeding)
                    {
                        if (!isLowBleedOpen)
                        {
                            isDeviceBled = true;
                            isDeviceBleeding = false;

                        }

                    }

                }
                else
                {
                    m_lowSideManifoldPressure = 0;
                }


            }

            else if (isLowHoseEngaged && !isHighHoseConnected)
            {
                m_highSideManifoldPressure = 0.0f;

            }
            if (differentialPressure <= maxPSID)
            {

                differentialPressure = HighSide(m_highSideManifoldPressure) - LowSide(m_lowSideManifoldPressure);

            }
            else if (differentialPressure > maxPSID)
            {
                differentialPressure = maxPSID;
            }


            // DOTween.To(()
            //              => hosePressure,
            //              x => hosePressure = x,
            //               differentialPressure, needleTweenSpeed).SetEase(Ease.Linear);
            hosePressure = Mathf.SmoothDamp(hosePressure, differentialPressure, ref hosePressureRef, needleTweenSpeed, 1);






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

    // IEnumerator Needle()
    // {
    //     float timeElapsed = 0;

    //     while (timeElapsed < needleLerpDuration)
    //     {
    //         hosePressure = Mathf.Lerp(hosePressure, differentialPressure, timeElapsed / needleLerpDuration);
    //         timeElapsed += Time.deltaTime;

    //         yield return null;
    //     }

    //     hosePressure = differentialPressure;
    // }
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

