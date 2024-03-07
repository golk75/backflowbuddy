
using System.Collections;
using System.Collections.Generic;
using com.zibra.liquid.DataStructures;
using com.zibra.liquid.Manipulators;
using com.zibra.liquid.SDFObjects;
using com.zibra.liquid.Solver;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;

public class DCWaterController : MonoBehaviour
{

    public GameObject Check1Status;
    public GameObject Check2Status;
    public SightTubeController m_SightTubeController;
    [SerializeField]
    GameObject testCockManager;

    [SerializeField]
    GameObject shutOffValveManager;
    [SerializeField]
    HoseController hoseController;
    [SerializeField]
    GameObject sightTube;
    public CheckValveStatus checkValveStatus;

    TestCockController testCockController;
    ShutOffValveController shutOffValveController;
    public RpzTestKitController rpzTestKitController;
    public DoubleCheckTestKitController m_DoubleCheckTestKitController;

    [SerializeField]
    SightTubeController sightTubeController;
    [SerializeField]
    GameObject ShutOffValve1;

    [SerializeField]
    ZibraLiquidEmitter mainSupplyEmitter;

    public ZibraLiquidEmitter m_sensingLineEmitter;

    [SerializeField]
    GameObject ShutOffValve2;

    [SerializeField]
    ZibraLiquidCollider supplyCollider;

    [SerializeField]
    ZibraLiquidVoid supplyVoid;

    [SerializeField]
    ZibraLiquidEmitter TestCock1Emitter;

    [SerializeField]
    ZibraLiquidDetector TestCockDetector1;

    [SerializeField]
    ZibraLiquidDetector TestCockDetector2;

    [SerializeField]
    ZibraLiquidDetector TestCockDetector3;

    [SerializeField]
    ZibraLiquidDetector TestCockDetector4;

    [SerializeField]
    HoseDetector TestCockHoseDetect1;

    [SerializeField]
    HoseDetector TestCockHoseDetect2;

    [SerializeField]
    HoseDetector TestCockHoseDetect3;

    [SerializeField]
    HoseDetector TestCockHoseDetect4;

    [SerializeField]
    ZibraLiquidDetector BodyDetectorZone1;

    [SerializeField]
    ZibraLiquidDetector check1Detector;

    [SerializeField]
    ZibraLiquidDetector check2Detector;

    [SerializeField]
    ZibraLiquidForceField TestCockFF1;

    [SerializeField]
    ZibraLiquidForceField TestCockFF2;

    [SerializeField]
    ZibraLiquidForceField TestCockFF3;

    [SerializeField]
    ZibraLiquidForceField TestCockFF4;
    public GameObject hoseDetector1;
    public GameObject hoseDetector2;
    public GameObject hoseDetector3;
    public GameObject hoseDetector4;
    [SerializeField]
    ZibraLiquidEmitter sightTubeEmitter;
    [SerializeField]
    ZibraLiquidVoid sightTubeVoid;


    float sightTubeVoidInitSurfaceDist;

    public ZibraLiquidVoid bleederCatchVoid;
    public ZibraLiquidVoid m_VoidCheck1;
    public ZibraLiquidVoid m_VoidCheck2;
    public ZibraLiquidCollider m_Collider_Check1Close;
    public ZibraLiquidCollider m_Collider_Check2Close;
    public ZibraLiquidVoid Void_floor;
    public ZibraLiquidVoid Void_floor_mid;
    public ZibraLiquidVoid Void_ReliefCheck_front;
    public ZibraLiquidVoid m_ReliefValve_top_void;
    public ZibraLiquidVoid m_VoidOutfeed;
    public ZibraLiquidVoid m_Zone1Void;
    public ZibraLiquidVoid m_Zone2Void;
    public ZibraLiquidVoid m_Zone3Void;

    [SerializeField]
    public ZibraLiquidForceField check1housingForceField;

    [SerializeField]
    public ZibraLiquidForceField check2housingForceField;

    public ZibraLiquidForceField m_sensingLineFF;
    public ZibraLiquidForceField m_ReliefDumpFF;


    //Coroutines
    Coroutine MaxParticleNumberRegulation;
    Coroutine CloseCheck1;


    float tc4ffScaleUpSpeed;
    float tc4ffScaleDownSpeed;

    //Vector3 Zone2VoidMaxSize = new Vector3(0.045f, 0.035f, 0.02f);


    public float Check1VoidGrowSpeed = 2.5f;
    public float Check2VoidGrowSpeed = 8.0f;

    private ZibraLiquidDetector _testCockDetector;
    public ZibraLiquidDetector TestCockDetector
    {
        get { return _testCockDetector; }
        private set { value = _testCockDetector; }
    }
    private ZibraLiquidDetector _checkZoneDetector;
    public ZibraLiquidDetector CheckZoneDetector
    {
        get { return _checkZoneDetector; }
        private set { value = _checkZoneDetector; }
    }


    public ZibraLiquidDetector m_detectorZone1;
    public ZibraLiquidDetector m_detectorZone2;
    public ZibraLiquidDetector m_detectorZone3;
    public ZibraLiquidDetector m_reliefValveSensingLineDetector;
    public GameObject CheckValve1;
    public GameObject CheckValve2;
    public GameObject sightTubeCurrentConnection;

    Vector3 initSupplyEmitterPos;
    Vector3 CheckValve1StartingPos;
    Vector3 CheckValve2StartingPos;
    Vector3 supplyColliderPos;
    Vector3 supplyColliderClosedPos;
    Vector3 initSupplyColliderPos;
    Vector3 supplyColliderTargetPos = new Vector3(-15f, 0, 0);
    Vector3 supplyVoidTargetPos = new Vector3(-9.5f, 0, 0);
    Vector3 initSupplyVoidPos;
    Vector3 maxCheck1CloseColliderSize = new Vector3(0.057f, 0.007f, 0.07f);
    Vector3 maxCheck2CloseVoidSize = new Vector3(0.055f, 0.005f, 0.05f);
    public Vector3 check1VoidMaxSize = new Vector3(0.045f, 0.0354f, 0.0201f);
    public Vector3 check2VoidMaxSize = new Vector3(0.045f, 0.0354f, 0.0201f);
    public Vector3 zoneVoidMaxSize = new Vector3(4f, 0.72f, 1f);

    Vector3 zoneVoidScaleRef = Vector3.zero;
    Vector3 supplyVoidScaleRef = Vector3.zero;
    Vector3 check1ColliderClose = Vector3.zero;
    Vector3 check2ColliderClose = Vector3.zero;
    Vector3 check1VoidRef = Vector3.zero;
    Vector3 check2VoidRef = Vector3.zero;
    Vector3 check1VoidTC1Ref = Vector3.zero;
    Vector3 check1FFref = Vector3.zero;
    Vector3 check2FFref = Vector3.zero;
    Vector3 testCockFF1Ref = Vector3.zero;
    Vector3 testCockFF2Ref = Vector3.zero;
    Vector3 testCockFF3Ref = Vector3.zero;
    Vector3 testCockFF4Ref = Vector3.zero;
    Vector3 maxSupplyVoidSize = new Vector3(3.93f, 1.87f, -0.04f);

    Vector3 initSupplyVoidScale;
    Vector3 currentSupplyVoidScale;
    Vector3 targetSupplyVoidScale;

    public float testCock1MaxStr;

    public float testCock2MaxStr;

    public float testCock3MaxStr;

    public float testCock3MinStr;

    public float testCock3Str;
    public HoseDetector HoseDetector2;
    public int testCock4MinStr;
    public int testCock4MaxStr;
    public float testCock4Str;
    public GameObject checkValve1;
    public GameObject checkValve2;
    public Rigidbody check1Rb;
    public Rigidbody check2Rb;
    public Rigidbody reliefCheckRb;
    public ZibraLiquid liquid;
    ZibraLiquidSolverParameters liquidSolverParameters;

    public float initialCheck1Mass;
    public float initialCheck2Mass;
    public float inputForce = 1;
    public float volume;
    public float supplyPsi = 80f;

    public float zone1Pressure;
    public float zone2Pressure;
    public float zone3Pressure;

    public float check1SpringForce;
    public float check2SpringForce;
    public float reliefValveSpringForce;
    public float zone1PsiChange;
    public float zone2PsiChange;
    public float zone3PsiChange;
    public float zone1to2PsiDiff;
    public float zone2to3PsiDiff;
    public float zone2primedParticleCount = 10000;
    public float zone3primedParticleCount = 10000;
    public float devicePrimedParticleCount = 39000;
    public bool test1InProgress = false;
    public float check1FFStrength;
    public float check2FFStrength;
    public float Zone1TcMinParticleCount = 3000;
    public float reliefValveOpeningPoint;
    public float pressureAgainstRelief;
    public float supplyVoidToZone1ConversionFactor = 10000f;
    float maxGaugeReading = 10;
    //booleans
    public bool randomizePressure = true;
    public bool isDeviceInTestingCondititons;
    public bool isDeviceInStaticCondition;
    public bool isTeachingModeEnabled = false;
    public bool isReliefValveOpen;
    public bool isCheck1Closed;
    public bool isCheck2Closed;

    [SerializeField]
    List<ZibraLiquidVoid> VoidList;
    [SerializeField]
    List<ZibraLiquidCollider> ColliderList;
    void Start()
    {

        testCockController = testCockManager.GetComponent<TestCockController>();
        shutOffValveController = shutOffValveManager.GetComponent<ShutOffValveController>();
        initSupplyVoidPos = supplyVoid.transform.localPosition;
        initSupplyColliderPos = supplyCollider.transform.localPosition;
        check1Rb = checkValve1.GetComponent<Rigidbody>();
        check2Rb = checkValve2.GetComponent<Rigidbody>();

        initialCheck1Mass = check1Rb.mass;
        initialCheck2Mass = check1Rb.mass;
        sightTubeVoidInitSurfaceDist = sightTubeVoid.GetComponent<AnalyticSDF>().SurfaceDistance;
    }

    /// <summary>
    /// This function is called when the object becomes enabled and active.
    /// </summary>
    private void OnEnable()
    {
        // testCock4Str = Random.Range(testCock4MinStr, testCock4MaxStr);
        // testCock3Str = Random.Range(testCock3MinStr, testCock3MaxStr);
        testCock4Str = UnityEngine.Random.Range(testCock4MinStr, testCock4MaxStr);
        testCock3Str = UnityEngine.Random.Range(testCock3MinStr, testCock3MaxStr);
        CheckValve1StartingPos = checkValve1.transform.position;
        CheckValve2StartingPos = checkValve2.transform.position;
        liquidSolverParameters = liquid.GetComponent<ZibraLiquidSolverParameters>();
        initSupplyEmitterPos = mainSupplyEmitter.transform.localPosition;
        mainSupplyEmitter.VolumePerSimTime = 0;

    }
    void SupplyRegulate()
    {
        /// <summary>
        /// Regulate supply pressure-------------------------------------------------------
        /// </summary>
        supplyColliderTargetPos.x =
            shutOffValveController.ShutOffValve1.transform.eulerAngles.z / 90;
        supplyVoidTargetPos.x = shutOffValveController.ShutOffValve1.transform.eulerAngles.z / 90;
        supplyVoid.transform.localPosition = initSupplyVoidPos - supplyVoidTargetPos;

        // volume = Mathf.Lerp(
        //                   supplyPsi,
        //                   0,
        //                   ShutOffValve1.transform.eulerAngles.z / 90f
        //               );
        if (shutOffValveController.IsSupplyOn == true)
        {
            foreach (var liquidVoid in VoidList)
            {
                liquidVoid.enabled = true;
            }
            foreach (var liquidCollider in ColliderList)
            {
                liquidCollider.enabled = false;
            }
            // m_ReliefDumpFF.enabled = false;
            // m_sensingLineFF.enabled = true;
            liquid.SimulationTimeScale = 60;

            supplyCollider.transform.localPosition = initSupplyColliderPos + supplyColliderTargetPos;
            mainSupplyEmitter.VolumePerSimTime = Mathf.SmoothStep(
                mainSupplyEmitter.VolumePerSimTime,
                supplyPsi,
                1f
            );
        }
        else
        {
            foreach (var liquidVoid in VoidList)
            {
                liquidVoid.enabled = false;
            }
            foreach (var liquidCollider in ColliderList)
            {
                liquidCollider.enabled = true;
            }
            liquid.SimulationTimeScale = 30;

            supplyCollider.transform.localPosition = new Vector3(1.24000001f, 2.21000004f, 0.0299999993f);
            mainSupplyEmitter.VolumePerSimTime = 0;




        }

    }
    void BleedHoseVoidRegulate()
    {
        if (HoseDetector2.currentHoseConnection == sightTube)
        {
            bleederCatchVoid.enabled = false;
        }
        else
        {
            bleederCatchVoid.enabled = true;
        }
    }
    void PressureZoneRegulate()
    {
        zone1Pressure = supplyPsi + m_detectorZone1.ParticlesInside / 10000 * 0.1f;

        if (zone1Pressure - check1SpringForce + zone2PsiChange >= 0 && m_detectorZone2.ParticlesInside >= 0)
        {

            zone2Pressure = zone1Pressure - check1SpringForce + zone2PsiChange;
        }
        else if (zone1Pressure - check2SpringForce + zone2PsiChange == 0)
        {
            zone2Pressure = 0;
            // zone2Pressure = m_detectorZone2.ParticlesInside / 1000 * 0.1f;
            // zone2Pressure = m_detectorZone2.ParticlesInside;


        }
        else
        {
            zone2Pressure = 0;
        }
        if (zone2Pressure - check2SpringForce + zone3PsiChange >= 0 && m_detectorZone3.ParticlesInside >= 0)
        {

            zone3Pressure = zone2Pressure - check2SpringForce + zone3PsiChange;

        }
        else if (zone2Pressure - check2SpringForce + zone3PsiChange == 0)
        {
            zone3Pressure = 0;
            // zone3Pressure = m_detectorZone3.ParticlesInside / 1000 * 0.1f;
            // zone3Pressure = m_detectorZone3.ParticlesInside;



        }
        else
        {
            zone3Pressure = 0;
        }


        //scaling zone pressure value to gauge reading, this gauge has a max value of 10 and is incremented by 1
        zone1to2PsiDiff = (zone1Pressure - zone2Pressure) / maxGaugeReading;
        zone2to3PsiDiff = (zone2Pressure - zone3Pressure) / maxGaugeReading;
        // reliefValveOpeningPoint = zone1Pressure - zone2Pressure - 2;

    }
    void CheckValveRegulate()
    {


        {
            if (zone1Pressure < check1SpringForce)
            {

                //While device is empty, supply pressure must overcome check#1
                check1Rb.AddForce(new Vector3(-1, -1, 0) * inputForce, ForceMode.Force);
                m_Collider_Check1Close.transform.localScale = Vector3.SmoothDamp(m_Collider_Check1Close.transform.localScale, maxCheck1CloseColliderSize, ref check1ColliderClose, 0.5f, 1f);


            }
            else
            {


                m_Collider_Check1Close.transform.localScale = Vector3.SmoothDamp(m_Collider_Check1Close.transform.localScale, Vector3.zero, ref check1ColliderClose, 2f, 2f);
                m_VoidCheck1.transform.localScale = Vector3.SmoothDamp(m_VoidCheck1.transform.localScale, Vector3.zero, ref check1VoidRef, 2f, 1f);



            }
            if (zone1Pressure <= zone2Pressure)
            {



                //close #1 if zone2 is higher than supply or if relief opened
                check1Rb.AddForce(new Vector3(-1, -1, 0) * inputForce, ForceMode.Force);

                // //check if closed, scale up void just before scaling up collider. This eliminates unatural spash that occurs when collider scaled up and compresses water into check housing wall
                if (Check1Status.GetComponent<CheckValveCollision>().isCheckClosed == true)
                {
                    m_VoidCheck1.transform.localScale = Vector3.SmoothDamp(m_VoidCheck1.transform.localScale, maxCheck1CloseColliderSize, ref check1VoidRef, 0.5f, 1f);
                    if (m_VoidCheck1.transform.localScale.x >= maxCheck1CloseColliderSize.x / 2)
                    {


                        m_Collider_Check1Close.transform.localScale = Vector3.SmoothDamp(m_Collider_Check1Close.transform.localScale, maxCheck1CloseColliderSize, ref check1ColliderClose, 0.5f, 1f);
                        check1housingForceField.Strength = 0;
                    }
                }

                //CloseCheckValve(checkValve1, Check1Status.GetComponent<CheckValveCollision>().isCheckClosed, m_Collider_Check1Close, m_VoidCheck1);

            }
            else
            {
                check1housingForceField.Strength = 0;
                m_Collider_Check1Close.transform.localScale = Vector3.SmoothDamp(m_Collider_Check1Close.transform.localScale, Vector3.zero, ref check1ColliderClose, 2f, 2f);
                m_VoidCheck1.transform.localScale = Vector3.SmoothDamp(m_VoidCheck1.transform.localScale, Vector3.zero, ref check1VoidRef, 2f, 1f);

            }
            if (zone2Pressure < check2SpringForce)
            {
                //While device is empty, zone2 pressure must overcome check#2
                check2Rb.AddForce(new Vector3(-1, -1, 0) * inputForce, ForceMode.Force);
                m_Collider_Check2Close.transform.localScale = Vector3.SmoothDamp(m_Collider_Check2Close.transform.localScale, maxCheck1CloseColliderSize, ref check2ColliderClose, 0.5f, 1f);
                if (shutOffValveController.IsSecondShutOffOpen == true)
                    m_VoidOutfeed.enabled = true;
            }
            else
            {
                m_Collider_Check2Close.transform.localScale = Vector3.SmoothDamp(m_Collider_Check2Close.transform.localScale, Vector3.zero, ref check2ColliderClose, 2f, 2f);
                m_VoidCheck2.transform.localScale = Vector3.SmoothDamp(m_VoidCheck2.transform.localScale, Vector3.zero, ref check2VoidRef, 2f, 1f);
                m_VoidOutfeed.enabled = false;
            }

            if (zone2Pressure <= zone3Pressure)
            {
                //close #2 if zone 3 is higher than zone2
                check2Rb.AddForce(new Vector3(-1, -1, 0) * inputForce, ForceMode.Force);
                //check if closed, scale up void just before scaling up collider. This eliminates unatural spash that occurs when collider scaled up and compresses water into check housing wall
                if (Check2Status.GetComponent<CheckValve2Collision>().isCheckClosed == true)
                {
                    m_VoidCheck2.transform.localScale = Vector3.SmoothDamp(m_VoidCheck2.transform.localScale, maxCheck2CloseVoidSize, ref check2VoidRef, 0.5f, 1f);
                    if (m_VoidCheck2.transform.localScale.x >= maxCheck1CloseColliderSize.x / 2)
                    {
                        m_Collider_Check2Close.transform.localScale = Vector3.SmoothDamp(m_Collider_Check2Close.transform.localScale, maxCheck1CloseColliderSize, ref check2ColliderClose, 0.5f, 1f);
                        check2housingForceField.Strength = 0;
                    }

                }

                // CloseCheckValve(checkValve2, Check2Status.GetComponent<CheckValve2Collision>().isCheckClosed, m_Collider_Check2Close, m_VoidCheck2);
            }
            else
            {
                check2housingForceField.Strength = 0;

                m_Collider_Check2Close.transform.localScale = Vector3.SmoothDamp(m_Collider_Check2Close.transform.localScale, Vector3.zero, ref check2ColliderClose, 2f, 2f);
                m_VoidCheck2.transform.localScale = Vector3.SmoothDamp(m_VoidCheck2.transform.localScale, Vector3.zero, ref check2VoidRef, 2f, 1f);
                //m_VoidOutfeed.enabled = false;
            }


            //sensing line regulate
            //max sensing line ff strength = 1.32f

        }

        /// <summary>
        /// END -any conditions
        /// </summary>

        /// <summary>
        /// shutoff testing conditions - with > 0 attachments
        /// </summary>

        // if (isDeviceInTestingCondititons == true && sightTubeController.currentTestCockConnection == null)
        if (isDeviceInTestingCondititons == true && sightTubeController.currentTestCockConnection == null)
        {

            if (zone1Pressure < check1SpringForce)
            {

                //While device is empty, supply pressure must overcome check#1
                check1Rb.AddForce(new Vector3(-1, -1, 0) * inputForce, ForceMode.Force);
                m_Collider_Check1Close.transform.localScale = Vector3.SmoothDamp(m_Collider_Check1Close.transform.localScale, maxCheck1CloseColliderSize, ref check1ColliderClose, 0.5f, 1f);

            }
            else
            {

                m_Collider_Check1Close.transform.localScale = Vector3.SmoothDamp(m_Collider_Check1Close.transform.localScale, Vector3.zero, ref check1ColliderClose, 2f, 2f);
                m_VoidCheck1.transform.localScale = Vector3.SmoothDamp(m_VoidCheck1.transform.localScale, Vector3.zero, ref check1VoidRef, 2f, 1f);

            }
            if (zone1Pressure <= zone2Pressure)
            {


                //close #1 if zone2 is higher than supply or if relief opened
                check1Rb.AddForce(new Vector3(-1, -1, 0) * inputForce, ForceMode.Force);

                // //check if closed, scale up void just before scaling up collider. This eliminates unatural spash that occurs when collider scaled up and compresses water into check housing wall
                if (Check1Status.GetComponent<CheckValveCollision>().isCheckClosed == true)
                {
                    m_VoidCheck1.transform.localScale = Vector3.SmoothDamp(m_VoidCheck1.transform.localScale, maxCheck1CloseColliderSize, ref check1VoidRef, 0.5f, 1f);
                    if (m_VoidCheck1.transform.localScale.x >= maxCheck1CloseColliderSize.x / 2)
                    {
                        m_Collider_Check1Close.transform.localScale = Vector3.SmoothDamp(m_Collider_Check1Close.transform.localScale, maxCheck1CloseColliderSize, ref check1ColliderClose, 0.5f, 1f);
                        check1housingForceField.Strength = 0;
                    }
                }

                //CloseCheckValve(checkValve1, Check1Status.GetComponent<CheckValveCollision>().isCheckClosed, m_Collider_Check1Close, m_VoidCheck1);

            }
            else
            {
                m_Collider_Check1Close.transform.localScale = Vector3.SmoothDamp(m_Collider_Check1Close.transform.localScale, Vector3.zero, ref check1ColliderClose, 2f, 2f);
                m_VoidCheck1.transform.localScale = Vector3.SmoothDamp(m_VoidCheck1.transform.localScale, Vector3.zero, ref check1VoidRef, 2f, 1f);

            }
            if (zone2Pressure < check2SpringForce)
            {
                //While device is empty, zone2 pressure must overcome check#2
                check2Rb.AddForce(new Vector3(-1, -1, 0) * inputForce, ForceMode.Force);
                m_Collider_Check2Close.transform.localScale = Vector3.SmoothDamp(m_Collider_Check2Close.transform.localScale, maxCheck1CloseColliderSize, ref check2ColliderClose, 0.5f, 1f);
                if (shutOffValveController.IsSecondShutOffOpen == true)
                    m_VoidOutfeed.enabled = true;
            }
            else
            {
                m_Collider_Check2Close.transform.localScale = Vector3.SmoothDamp(m_Collider_Check2Close.transform.localScale, Vector3.zero, ref check2ColliderClose, 2f, 2f);
                m_VoidCheck2.transform.localScale = Vector3.SmoothDamp(m_VoidCheck2.transform.localScale, Vector3.zero, ref check2VoidRef, 2f, 1f);
                m_VoidOutfeed.enabled = false;

            }

            if (zone2Pressure <= zone3Pressure)
            {
                //close #2 if zone 3 is higher than zone2
                check2Rb.AddForce(new Vector3(-1, -1, 0) * inputForce, ForceMode.Force);
                //check if closed, scale up void just before scaling up collider. This eliminates unatural spash that occurs when collider scaled up and compresses water into check housing wall
                if (Check2Status.GetComponent<CheckValve2Collision>().isCheckClosed == true)
                {
                    m_VoidCheck2.transform.localScale = Vector3.SmoothDamp(m_VoidCheck2.transform.localScale, maxCheck2CloseVoidSize, ref check2VoidRef, 0.5f, 1f);
                    if (m_VoidCheck2.transform.localScale.x >= maxCheck1CloseColliderSize.x / 2)
                    {
                        m_Collider_Check2Close.transform.localScale = Vector3.SmoothDamp(m_Collider_Check2Close.transform.localScale, maxCheck1CloseColliderSize, ref check2ColliderClose, 0.5f, 1f);
                        check2housingForceField.Strength = 0;
                    }

                }

                // CloseCheckValve(checkValve2, Check2Status.GetComponent<CheckValve2Collision>().isCheckClosed, m_Collider_Check2Close, m_VoidCheck2);
            }
            else
            {

                m_Collider_Check2Close.transform.localScale = Vector3.SmoothDamp(m_Collider_Check2Close.transform.localScale, Vector3.zero, ref check2ColliderClose, 2f, 2f);
                m_VoidCheck2.transform.localScale = Vector3.SmoothDamp(m_VoidCheck2.transform.localScale, Vector3.zero, ref check2VoidRef, 2f, 1f);
                //m_VoidOutfeed.enabled = false;
            }

            // //close check valves if the device is full and no water is being used downstream (ie. shutOff #2 is closed)
            if (testCockController.isTestCock2Open == false && testCockController.isTestCock3Open == false && testCockController.isTestCock4Open == false)
            {
                check1Rb.AddForce(new Vector3(-1, -1, 0) * inputForce, ForceMode.Force);
                check2Rb.AddForce(new Vector3(-1, -1, 0) * inputForce, ForceMode.Force);


            }
            // if (testCockController.isTestCock2Open == true && testCockController.isTestCock3Open == false && testCockController.isTestCock4Open == false)
            // {
            //     check1Rb.AddForce(new Vector3(-1, -1, 0) * inputForce, ForceMode.Force);
            //     check2Rb.AddForce(new Vector3(-1, -1, 0) * inputForce, ForceMode.Force);


            // }
            // if (testCockController.isTestCock2Open == false && testCockController.isTestCock3Open == true && testCockController.isTestCock4Open == false)
            // {
            //     check1Rb.AddForce(new Vector3(1, 1, 0) * inputForce, ForceMode.Force);
            //     check2Rb.AddForce(new Vector3(-1, -1, 0) * inputForce, ForceMode.Force);

            // }
            // if (testCockController.isTestCock2Open == false && testCockController.isTestCock3Open == false && testCockController.isTestCock4Open == true)
            // {
            //     check1Rb.AddForce(new Vector3(1, 1, 0) * inputForce, ForceMode.Force);
            //     check2Rb.AddForce(new Vector3(1, 1, 0) * inputForce, ForceMode.Force);

            // }



        }

        else if (isDeviceInTestingCondititons == true && sightTubeController.currentTestCockConnection != null)
        {




            //test #1

            if (testCockController.isTestCock2Open == true && testCockController.isTestCock3Open == false && testCockController.isTestCock4Open == false)
            {

                check1Rb.AddForce(new Vector3(-1, -1, 0) * inputForce, ForceMode.Force);
                check2Rb.AddForce(new Vector3(-1, -1, 0) * inputForce, ForceMode.Force);


            }
            if (testCockController.isTestCock2Open == true && testCockController.isTestCock3Open == true && testCockController.isTestCock4Open == false)
            {


                // Debug.Log($"zone1to2PsiDiff: {zone1to2PsiDiff}; check1SpringForce: {check1SpringForce}");

                if (zone1Pressure - zone2Pressure < check1SpringForce)
                {
                    // check1Rb.AddForce(new Vector3(1, 1, 0) * inputForce, ForceMode.Force);


                    // check1housingForceField.Strength = Mathf.SmoothDamp(
                    //    check1housingForceField.Strength,
                    //    1f,
                    //    ref check1FFref.x,
                    //    0.5f);
                    // check1Rb.AddForce(new Vector3(1, 1, 0) * inputForce, ForceMode.Force);
                    // check2Rb.AddForce(new Vector3(-1, -1, 0) * inputForce, ForceMode.Force);
                }



            }
            //end test #1


            //test #2

            if (testCockController.isTestCock2Open == false && testCockController.isTestCock3Open == true && testCockController.isTestCock4Open == false)
            {

                check1Rb.AddForce(new Vector3(-1, -1, 0) * inputForce, ForceMode.Force);
                check2Rb.AddForce(new Vector3(-1, -1, 0) * inputForce, ForceMode.Force);



            }
            if (testCockController.isTestCock2Open == false && testCockController.isTestCock3Open == true && testCockController.isTestCock4Open == true)
            {

                if (zone2Pressure - zone3Pressure > check2SpringForce)
                {
                    // check1Rb.AddForce(new Vector3(1, 1, 0) * inputForce, ForceMode.Force);

                    // check1Rb.AddForce(new Vector3(-1, -1, 0) * inputForce, ForceMode.Force);
                    // check2Rb.AddForce(new Vector3(-1, -1, 0) * inputForce, ForceMode.Force);

                }
                else
                {


                    // check1Rb.AddForce(new Vector3(1, 1, 0) * inputForce, ForceMode.Force);
                    // check2Rb.AddForce(new Vector3(1, 1, 0) * inputForce, ForceMode.Force);


                }


            }
            //end test #2



        }

        /// <summary>
        /// END - shutoff testing conditions - with > 0 attachments
        /// </summary>



        {
            if (isDeviceInTestingCondititons == false)
            {



                if (zone3Pressure < zone2Pressure)
                {
                    check2housingForceField.Strength = Mathf.SmoothDamp(
                                        check2housingForceField.Strength,
                                        check2FFStrength,
                                        ref check2FFref.x,
                                        0.5f
                                    );
                }
                else
                {

                    check2housingForceField.Strength = 0;
                }
                //close check valves if the device is full and no water is being used downstream (ie. shutOff #2 is closed)
                if (testCockController.isTestCock3Open == false && testCockController.isTestCock4Open == false && shutOffValveController.IsSecondShutOffOpen == false)
                {

                    if (m_detectorZone2.ParticlesInside > zone2primedParticleCount)
                    {
                        check1Rb.AddForce(new Vector3(-1, -1, 0) * inputForce, ForceMode.Force);

                    }
                    else
                    {
                        check1housingForceField.Strength = Mathf.SmoothDamp(
                          check1housingForceField.Strength,
                          check1FFStrength,
                          ref check1FFref.x,
                          0.5f
                      );
                    }

                    if (m_detectorZone3.ParticlesInside > zone3primedParticleCount)
                    {
                        check2Rb.AddForce(new Vector3(-1, -1, 0) * inputForce, ForceMode.Force);
                        check2housingForceField.Strength = 0;
                    }
                    else
                    {
                        check2housingForceField.Strength = Mathf.SmoothDamp(
                            check2housingForceField.Strength,
                            check1FFStrength,
                            ref check2FFref.x,
                            0.5f
                        );
                    }


                }
                //open check#2 if tc#4 is opened
                if (testCockController.isTestCock3Open == false && testCockController.isTestCock4Open == true && shutOffValveController.IsSecondShutOffOpen == false)
                {



                }
                if (testCockController.isTestCock3Open == true && testCockController.isTestCock4Open == false && shutOffValveController.IsSecondShutOffOpen == false)
                {

                    if (m_detectorZone2.ParticlesInside > zone2primedParticleCount)
                    {




                        check2Rb.AddForce(new Vector3(-1, -1, 0) * inputForce, ForceMode.Force);
                        check2housingForceField.Strength = 0;



                    }

                    else
                    {
                        check1housingForceField.Strength = Mathf.SmoothDamp(
                         check1housingForceField.Strength,
                         1f,
                         ref check1FFref.x,
                         0.5f);
                    }



                }
                if (testCockController.isTestCock3Open == true && testCockController.isTestCock4Open == true && shutOffValveController.IsSecondShutOffOpen == false)
                {
                    check1housingForceField.Strength = Mathf.SmoothDamp(
                                         check1housingForceField.Strength,
                                         check1FFStrength,
                                         ref check1FFref.x,
                                         0.5f
                                     );
                    check2housingForceField.Strength = Mathf.SmoothDamp(
                            check2housingForceField.Strength,
                            check2FFStrength,
                            ref check2FFref.x,
                            0.5f
                        );

                }


            }
        }



        /// <summary>
        /// non-shutoff testing conditions - no attachments
        /// </summary>
        {
            if (shutOffValveController.IsSupplyOn == false && shutOffValveController.IsSecondShutOffOpen == true)
            {
                check1housingForceField.Strength = 0;
                check2housingForceField.Strength = 0;

            }
            else
            {

                // //only worried about check housing force field right now
                // check1housingForceField.Strength = Mathf.SmoothDamp(
                //       check1housingForceField.Strength,
                //       check1FFStrength,
                //       ref check1FFref.x,
                //       0.5f
                //   );
                // check2housingForceField.Strength = Mathf.SmoothDamp(
                //                     check2housingForceField.Strength,
                //                     check2FFStrength,
                //                     ref check2FFref.x,
                //                     0.5f
                //                 );

            }
            // if (shutOffValveController.IsSupplyOn == false && shutOffValveController.IsSecondShutOffOpen == false)
            // {
            //     check1housingForceField.Strength = 0;
            //     check2housingForceField.Strength = 0;
            // }
        }

    }
    void TestCock1Regulate()
    {

        if (testCockController.isTestCock1Open == true && TestCockHoseDetect1.isConnected == false)
        {
            TestCock1Emitter.enabled = true;
            TestCockFF1.Strength = 1f;
        }
        else
        {
            TestCock1Emitter.enabled = false;
            TestCockFF1.Strength = 0f;
        }
        // if (testCockController.isTestCock1Open == true
        //           && TestCockHoseDetect1.isConnected == false
        //           && shutOffValveController.IsSupplyOn == false
        //           && shutOffValveController.IsSecondShutOffOpen == false
        //           )
        // {
        //     //both shutoffs closed
        //     TestCockFF1.Strength = 0f;
        //     TestCock1Emitter.enabled = true;


        // }
        // else if (testCockController.isTestCock1Open == true
        //      && TestCockHoseDetect1.isConnected == false
        //      && shutOffValveController.IsSupplyOn == false
        //     )
        // {
        //     // only shutoff #2 closed
        //     TestCock1Emitter.enabled = true;

        // }
        // else if (testCockController.isTestCock1Open == false
        //     && TestCockHoseDetect1.isConnected == false
        //     && shutOffValveController.IsSupplyOn == false
        //    )
        // {
        //     TestCock1Emitter.enabled = false;


        // }
        // else if (testCockController.isTestCock1Open == true
        //    && TestCockHoseDetect1.isConnected == false
        //    && shutOffValveController.IsSupplyOn == true
        //   )
        // {
        //     TestCockFF1.Strength = 2f;
        // }
        // else if (testCockController.isTestCock1Open == false
        //    && TestCockHoseDetect1.isConnected == false
        //    && shutOffValveController.IsSupplyOn == true
        //   )
        // {
        //     TestCockFF1.Strength = 0f;
        // }
    }
    void WaterOperations()
    {

        if (shutOffValveController.IsSupplyOn == false && shutOffValveController.IsSecondShutOffOpen == false)
        {
            isDeviceInTestingCondititons = true;
        }
        // if (shutOffValveController.IsSupplyOn == true && shutOffValveController.IsSecondShutOffOpen == true && m_DoubleCheckTestKitController.AttachedHoseList.Count == 0)
        if (shutOffValveController.IsSupplyOn == true && shutOffValveController.IsSecondShutOffOpen == true)
        {


            ///Determine if testing conditions are met ---------------------------------------------------
            isDeviceInTestingCondititons = false;

            if (
                    testCockController.isTestCock2Open == true
                    && TestCockHoseDetect2.isConnected == false

                )
            {

                if (m_detectorZone1.ParticlesInside > 5000)
                {
                    TestCockFF2.Strength = Mathf.SmoothDamp(
                        TestCockFF2.Strength,
                        Mathf.Clamp(check1Detector.ParticlesInside, 0, testCock2MaxStr),
                        ref testCockFF2Ref.x,
                        0.005f
                    );

                }

            }
            else
            {

                TestCockFF2.Strength = 0;
            }

            if (
                   testCockController.isTestCock3Open == true
                   && TestCockHoseDetect3.isConnected == false
               // && sightTubeController.currentTestCockConnection != hoseDetector2
               )
            {

                if (check1Detector.ParticlesInside > Zone1TcMinParticleCount)
                {
                    TestCockFF3.Strength = Mathf.SmoothDamp(
                        TestCockFF3.Strength,
                        Mathf.Clamp(check1Detector.ParticlesInside, 0, testCock3MaxStr),
                        ref testCockFF3Ref.x,
                        0.005f
                    );

                }

            }
            else
            {

                TestCockFF3.Strength = 0;
            }

            //tc4 non-static condition pressure


            if (
                    testCockController.isTestCock4Open == true
                    && TestCockHoseDetect4.isConnected == false
               )
            {

                if (check2Detector.ParticlesInside > Zone1TcMinParticleCount)
                {
                    TestCockFF4.Strength = Mathf.SmoothDamp(
                        TestCockFF4.Strength,
                        Mathf.Clamp(check2Detector.ParticlesInside, 0, testCock4MaxStr),
                        ref testCockFF4Ref.x,
                        0.005f
                    );

                }

            }
            else
            {

                TestCockFF4.Strength = 0;
            }

        }


        else if (shutOffValveController.IsSupplyOn == true && shutOffValveController.IsSecondShutOffOpen == false && m_DoubleCheckTestKitController.AttachedHoseList.Count == 0)
        {
            ///Determine if testing conditions are met ---------------------------------------------------
            isDeviceInTestingCondititons = false;

            foreach (GameObject testCock in m_DoubleCheckTestKitController.StaticTestCockList)
            {
                testCock.GetComponent<AssignTestCockManipulators>().testCockVoid.enabled = true;
                testCock.GetComponent<AssignTestCockManipulators>().testCockCollider.enabled = false;
            }

            /// <summary>
            /// No hose(s) connected - water ops
            /// </summary>

            if (
                    testCockController.isTestCock2Open == true
                    && m_detectorZone1.ParticlesInside > 0
                    && TestCockHoseDetect2.isConnected == true

                )
            {

                if (m_DoubleCheckTestKitController.hosePressure - 0.01f > zone1to2PsiDiff)
                {
                    TestCockFF2.Strength = Mathf.SmoothDamp(
                        TestCockFF2.Strength,
                        Mathf.Clamp(check1Detector.ParticlesInside, 0, testCock2MaxStr),
                        ref testCockFF2Ref.x,
                        0.005f
                    );

                }
                else
                {
                    TestCockFF2.Strength = 0;
                }

            }
            else
            {

                TestCockFF2.Strength = 0;
            }

            //tc3 static condition pressure


            if (
                   testCockController.isTestCock3Open == true
                   && TestCockHoseDetect3.isConnected == true
               )
            {

                if (m_DoubleCheckTestKitController.hosePressure - 0.01f > zone1to2PsiDiff)
                {
                    TestCockFF3.Strength = Mathf.SmoothDamp(
                        TestCockFF3.Strength,
                        Mathf.Clamp(check1Detector.ParticlesInside, 0, testCock3MaxStr),
                        ref testCockFF3Ref.x,
                        0.005f
                    );

                }

            }
            else
            {

                TestCockFF3.Strength = 0;
            }

            if (
                     testCockController.isTestCock4Open == true
                     && TestCockHoseDetect4.isConnected == true

                 )
            {

                if (m_DoubleCheckTestKitController.hosePressure - 0.01f > zone2to3PsiDiff)
                {
                    TestCockFF4.Strength = Mathf.SmoothDamp(
                        TestCockFF4.Strength,
                        Mathf.Clamp(check2Detector.ParticlesInside, 0, testCock4MaxStr),
                        ref testCockFF4Ref.x,
                        0.005f
                    );

                }

            }
            else
            {

                TestCockFF4.Strength = 0;
            }

            /// <summary>
            /// END - No hose(s) connected - water ops
            /// </summary>


            /// <summary>
            /// No hose(s) connected
            /// </summary>
            {
                if (
                        testCockController.isTestCock2Open == true
                        && TestCockHoseDetect2.isConnected == false
                    )
                {

                    if (check1Detector.ParticlesInside > Zone1TcMinParticleCount)
                    {
                        TestCockFF2.Strength = Mathf.SmoothDamp(
                            TestCockFF2.Strength,
                            Mathf.Clamp(check1Detector.ParticlesInside, 0, testCock2MaxStr),
                            ref testCockFF2Ref.x,
                            0.005f
                        );

                    }

                }
                else
                {

                    TestCockFF2.Strength = 0;
                }




                if (
                       testCockController.isTestCock3Open == true
                       && TestCockHoseDetect3.isConnected == false

                   )
                {

                    if (check1Detector.ParticlesInside > Zone1TcMinParticleCount)
                    {
                        TestCockFF3.Strength = Mathf.SmoothDamp(
                            TestCockFF3.Strength,
                            Mathf.Clamp(check1Detector.ParticlesInside, 0, testCock3MaxStr),
                            ref testCockFF3Ref.x,
                            0.005f
                        );

                    }

                }
                else
                {

                    TestCockFF3.Strength = 0;
                }




                if (
                       testCockController.isTestCock4Open == true
                       && TestCockHoseDetect4.isConnected == false
                   )
                {

                    if (check2Detector.ParticlesInside > Zone1TcMinParticleCount)
                    {
                        TestCockFF4.Strength = Mathf.SmoothDamp(
                            TestCockFF4.Strength,
                            Mathf.Clamp(check2Detector.ParticlesInside, 0, testCock4MaxStr),
                            ref testCockFF4Ref.x,
                            0.005f
                        );

                    }

                }
                else
                {

                    TestCockFF4.Strength = 0;
                }
                /// <summary>
                ///END - No hose(s) connected
                /// </summary>


            }
        }

        else if (shutOffValveController.IsSupplyOn == true && shutOffValveController.IsSecondShutOffOpen == false && m_DoubleCheckTestKitController.AttachedHoseList.Count > 0)
        {
            ///Determine if testing conditions are met ---------------------------------------------------
            isDeviceInTestingCondititons = false;

            foreach (GameObject testCock in m_DoubleCheckTestKitController.StaticTestCockList)
            {
                testCock.GetComponent<AssignTestCockManipulators>().testCockVoid.enabled = true;
                testCock.GetComponent<AssignTestCockManipulators>().testCockCollider.enabled = false;
            }

            /// <summary>
            /// hose(s) connected - water ops
            /// </summary>

            if (
                    testCockController.isTestCock2Open == true
                    && m_detectorZone1.ParticlesInside > 0
                    && TestCockHoseDetect2.isConnected == true

                )
            {

                if (m_DoubleCheckTestKitController.hosePressure - 0.01f > zone1to2PsiDiff)
                {
                    TestCockFF2.Strength = Mathf.SmoothDamp(
                        TestCockFF2.Strength,
                        Mathf.Clamp(check1Detector.ParticlesInside, 0, testCock2MaxStr),
                        ref testCockFF2Ref.x,
                        0.005f
                    );

                }
                else
                {
                    TestCockFF2.Strength = 0;
                }

            }
            else
            {

                TestCockFF2.Strength = 0;
            }

            //tc3 static condition pressure


            if (
                   testCockController.isTestCock3Open == true
                   && TestCockHoseDetect3.isConnected == true

               )
            {

                if (m_DoubleCheckTestKitController.hosePressure - 0.01f > zone1to2PsiDiff)
                {
                    TestCockFF3.Strength = Mathf.SmoothDamp(
                        TestCockFF3.Strength,
                        Mathf.Clamp(check1Detector.ParticlesInside, 0, testCock3MaxStr),
                        ref testCockFF3Ref.x,
                        0.005f
                    );

                }

            }
            else
            {

                TestCockFF3.Strength = 0;
            }

            if (
                     testCockController.isTestCock4Open == true
                     && TestCockHoseDetect4.isConnected == true

                 )
            {

                if (m_DoubleCheckTestKitController.hosePressure - 0.01f > zone2to3PsiDiff)
                {
                    TestCockFF4.Strength = Mathf.SmoothDamp(
                        TestCockFF4.Strength,
                        Mathf.Clamp(check2Detector.ParticlesInside, 0, testCock4MaxStr),
                        ref testCockFF4Ref.x,
                        0.005f
                    );

                }

            }
            else
            {

                TestCockFF4.Strength = 0;
            }

            /// <summary>
            /// END - hose(s) connected - water ops
            /// </summary>


            /// <summary>
            /// No hose(s) connected
            /// </summary>
            {
                if (
                        testCockController.isTestCock2Open == true
                        && TestCockHoseDetect2.isConnected == false
                    )
                {

                    if (check1Detector.ParticlesInside > Zone1TcMinParticleCount)
                    {
                        TestCockFF2.Strength = Mathf.SmoothDamp(
                            TestCockFF2.Strength,
                            Mathf.Clamp(check1Detector.ParticlesInside, 0, testCock2MaxStr),
                            ref testCockFF2Ref.x,
                            0.005f
                        );

                    }

                }
                else
                {

                    TestCockFF2.Strength = 0;
                }




                if (
                       testCockController.isTestCock3Open == true
                       && TestCockHoseDetect3.isConnected == false

                   )
                {

                    if (check1Detector.ParticlesInside > Zone1TcMinParticleCount)
                    {
                        TestCockFF3.Strength = Mathf.SmoothDamp(
                            TestCockFF3.Strength,
                            Mathf.Clamp(check1Detector.ParticlesInside, 0, testCock3MaxStr),
                            ref testCockFF3Ref.x,
                            0.005f
                        );

                    }

                }
                else
                {

                    TestCockFF3.Strength = 0;
                }




                if (
                       testCockController.isTestCock4Open == true
                       && TestCockHoseDetect4.isConnected == false
                   )
                {

                    if (check2Detector.ParticlesInside > Zone1TcMinParticleCount)
                    {
                        TestCockFF4.Strength = Mathf.SmoothDamp(
                            TestCockFF4.Strength,
                            Mathf.Clamp(check2Detector.ParticlesInside, 0, testCock4MaxStr),
                            ref testCockFF4Ref.x,
                            0.005f
                        );

                    }

                }
                else
                {

                    TestCockFF4.Strength = 0;
                }
                /// <summary>
                ///END - No hose(s) connected
                /// </summary>

            }
        }

    }
    void SightTubeControl()
    {
        //if device is not in conditions
        if (!isDeviceInTestingCondititons && m_detectorZone1.ParticlesInside > 1000)
        {
            if (
                 TestCockHoseDetect1.isConnected == true
                && sightTubeController.currentTestCockConnection == hoseDetector1
               )
            {
                TestCock1Emitter.enabled = false;
                if (testCockController.isTestCock1Open == true)
                {
                    sightTubeEmitter.enabled = true;
                }
                else
                {
                    sightTubeEmitter.enabled = false;
                }
            }

            if (
               TestCockHoseDetect2.isConnected == true
              && sightTubeController.currentTestCockConnection == hoseDetector2
             )
            {
                if (testCockController.isTestCock2Open == true)
                {
                    sightTubeEmitter.enabled = true;
                }
                else
                {
                    sightTubeEmitter.enabled = false;
                }
            }
            if (
               TestCockHoseDetect3.isConnected == true
              && sightTubeController.currentTestCockConnection == hoseDetector3
             )
            {
                if (testCockController.isTestCock3Open == true)
                {
                    sightTubeEmitter.enabled = true;
                }
                else
                {
                    sightTubeEmitter.enabled = false;
                }
            }
            if (
               TestCockHoseDetect4.isConnected == true
              && sightTubeController.currentTestCockConnection == hoseDetector4
             )
            {
                if (testCockController.isTestCock4Open == true)
                {
                    sightTubeEmitter.enabled = true;
                }
                else
                {
                    sightTubeEmitter.enabled = false;
                }
            }
        }
        else
        {
            //if device is in testing conditions
            if (m_SightTubeController.currentTestCockConnection == hoseDetector3 && testCockController.isTestCock3Open && zone1Pressure >= zone2Pressure)
            {
                if (m_detectorZone1.ParticlesInside > 1000)
                {
                    sightTubeEmitter.enabled = true;


                }

            }

            else if (m_SightTubeController.currentTestCockConnection == hoseDetector4 && testCockController.isTestCock4Open && zone2Pressure >= zone3Pressure)
            {
                if (m_detectorZone2.ParticlesInside > 1000)
                {
                    sightTubeEmitter.enabled = true;
                }

            }
            else
            {
                sightTubeEmitter.enabled = false;


            }
        }

    }
    void Update()
    {

        WaterOperations();

        /// <summary>
        /// Regulate Supply Collider
        /// </summary>
        SupplyRegulate();

        /// <summary>
        /// Regulate Pressure Zones
        /// </summary>
        PressureZoneRegulate();

        /// <summary>
        /// Regulate Check Valves
        /// </summary>
        CheckValveRegulate();

        /// <summary>
        /// Idividually regulating test cock #1 pressure
        /// </summary>
        TestCock1Regulate();

        /// <summary>
        /// regulate bleeder hose void catch to allow water to fill sight tube when connected to test cock #2
        /// </summary>
        BleedHoseVoidRegulate();

        /// <summary>
        /// Regulate sight tube
        /// </summary>

        SightTubeControl();

    }
    void FixedUpdate()
    {
    }

}
