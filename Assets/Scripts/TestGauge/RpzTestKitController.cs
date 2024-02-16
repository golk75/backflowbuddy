using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using com.zibra.liquid.Manipulators;
using com.zibra.liquid.Solver;
using DG.Tweening;
using JetBrains.Annotations;
using Unity.VisualScripting;

using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.Timeline;
using UnityEngine.UIElements;

public class RpzTestKitController : MonoBehaviour
{
    public WaterController waterController;
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
    public PressureZoneHUDController pressureZoneHUDController;

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

    GameObject currentHighHoseConnection;

    [SerializeField]
    GameObject Check1;

    [SerializeField]
    Vector3 Check1Pos;

    Vector3 initLowHosePosition;
    Vector3 initHighHosePosition;
    Vector3 initBypassHosePosition;
    public HoseBib HighHoseBib;
    public HoseBib LowHoseBib;
    public HoseBib BypassHoseBib;
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

    public bool isCheck1Open;
    public bool isCheck2Open;
    float lowHosePressure;

    public bool isConnectedTestCockOpen;
    public bool isTestCock1Open;
    public bool isTestCock2Open;
    public bool isTestCock3Open;
    public bool isTestCock4Open;

    public bool isHighHoseEngaged;
    public bool isLowHoseEngaged;
    public bool isBypassHoseEngaged;

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

    private const string EmissionColor = "_EmissionColor";
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
        Actions.onBypassControlOperate += BypassControlOperate;


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
        Actions.onBypassControlOperate -= BypassControlOperate;


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
            bleederHoseEmitter.VolumePerSimTime = 1;
            m_ChangeColorOpen = StartCoroutine(OpenColorChange(highBleedIndicator.GetComponent<Renderer>().material));
            KnobClickOperate = StartCoroutine(RotateKnobOpen(currentKnob, new Vector3(0, 0, 180)));

        }
        else
        {
            isHighBleedOpen = false;
            bleederHoseEmitter.VolumePerSimTime = 0;
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
            bleederHoseEmitter.VolumePerSimTime = 1;
            m_ChangeColorOpen = StartCoroutine(OpenColorChange(lowBleedIndicator.GetComponent<Renderer>().material));
            KnobClickOperate = StartCoroutine(RotateKnobOpen(currentKnob, new Vector3(0, 0, 180)));

        }
        else
        {
            isLowBleedOpen = false;
            bleederHoseEmitter.VolumePerSimTime = 0;
            m_ChangeColorClose = StartCoroutine(CloseColorChange(lowBleedIndicator.GetComponent<Renderer>().material));
            KnobClickOperate = StartCoroutine(RotateKnobClosed(currentKnob, new Vector3(0, 0, 180)));
        }

    }
    private void BypassControlOperate()
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

            //move water depending on what else in control manifold is open/closed.






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


    private void NeedleControl()
    {

        needle.transform.eulerAngles = new Vector3(0, 0, GetPsidNeedleRotation());
    }
    private void DigitalNeedleControl()
    {
        // _gaugeProgressBar.style.width = Length.Percent(GetPsidDigitalNeedle());
    }



    private void PressureControl()
    {

        //track zone pressures
        zone1to2PsiDiff = rpzWaterController.zone1to2PsiDiff;
        zone2to3PsiDiff = rpzWaterController.zone2to3PsiDiff;

        if (AttachedHoseList.Count > 0)
        {

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
                    if (isTestCock2Open)
                    {
                        isHighHoseEngaged = true;

                    }
                    else
                    {
                        isHighHoseEngaged = false;

                    }
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
                hosePressure = Mathf.SmoothStep(hosePressure, 0, needleRiseSpeed);
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

                    if (isTestCock3Open)
                    {
                        isLowHoseEngaged = true;

                        // Debug.Log($"low hose on tc#3 && tc#3 opened");
                        if (rpzWaterController.m_detectorZone2.ParticlesInside > 100)
                        {


                            // hosePressure = Mathf.SmoothStep(hosePressure, 0, needleRiseSpeed);

                        }
                    }
                    else
                    {

                        isLowHoseEngaged = false;
                        // Debug.Log($"low hose on tc#3 && tc#3 closed");
                    }
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
                    if (isTestCock4Open)
                    {
                        // Debug.Log($"bypass hose on tc#4 && tc#4 opened");
                    }
                    else
                    {
                        // Debug.Log($"bypass hose on tc#4 && tc#4 closed");
                    }
                }
            }
            /// <summary>
            /// End - Bypass Hose
            /// </summary>



            //========================================
            // Start Test Procedures//========================>
            //========================================
            if (rpzWaterController.m_detectorZone1.ParticlesInside > 100)
            {

                if (isHighHoseEngaged == true && isHighBleedOpen == true && isLowBleedOpen == false)
                {
                    hosePressure = Mathf.SmoothStep(hosePressure, maxPSID - 0.09f, needleRiseSpeed);
                }
                else if (isHighHoseEngaged == true && isLowHoseEngaged == true && isHighBleedOpen == true && isLowBleedOpen == true)
                {
                    hosePressure = Mathf.SmoothStep(hosePressure, maxPSID - 0.1f, needleRiseSpeed);
                }
                else if (isHighHoseEngaged == true && isLowHoseEngaged == true && isHighBleedOpen == false && isLowBleedOpen == false)
                {
                    //apparent reading
                    hosePressure = Mathf.SmoothStep(hosePressure, rpzWaterController.check1SpringForce / 10, needleRiseSpeed);


                    //========================================
                    // Relief Valave Opening Point//========================>
                    //========================================
                    if (isLowControlOpen & isHighControlOpen)
                    {
                        Debug.Log($"here");
                        /*
                        1. move pressure accross #1 check, simulating a leak. (increase pressure in zone two)
                        2. drop needle while pressure increases in zone 2
                        3.crack open relief, stop needle.
                        */

                        ReliefValveOpeningPoint = StartCoroutine(TestRVOP(pressureZoneHUDController.m_PressureZone2Panel.Q<Slider>(className: "pressure-zone-slider").value));


                    }

                    //========================================
                    // END - Relief Valave Opening Point//==================>
                    //========================================  



                }
                else if (isHighHoseEngaged == true)
                {

                    hosePressure = Mathf.SmoothStep(hosePressure, maxPSID, needleRiseSpeed);
                }




            }




            //========================================
            // Check Valve #2//========================>
            //========================================


            //========================================
            // END - Check Valve #2//==================>
            //========================================


            //========================================
            // Check Valve #1//========================>
            //========================================


            //========================================
            // END - Check Valve #1//==================>
            //========================================






            //========================================
            // End Test Procedures//========================>
            //========================================
        }
        /// <summary>
        /// No hoses attached
        /// </summary>
        else
        {
            //nothing attached, remove pressure from test gauge
            hosePressure = Mathf.SmoothStep(
                             hosePressure,
                             0,
                             needleRiseSpeed
                         );
        }
    }

    private IEnumerator TestRVOP(float zone2Pressure)
    {
        // pressureZoneHUDController.m_PressureZone2Panel.Q<Slider>(className: "pressure-zone-slider").value += 1;

        while (zone2Pressure > rpzWaterController.reliefValveOpeningPoint)
        {
            Debug.Log($"adding pressure to zone 2");
            pressureZoneHUDController.m_PressureZone2Panel.Q<Slider>(className: "pressure-zone-slider").value += 0.1f;
            yield return null;
        }

    }






    // Update is called once per frame
    void Update()
    {
        PressureControl();
        NeedleControl();
        DigitalNeedleControl();

    }


}

