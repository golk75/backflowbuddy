
using System.Collections;
using com.zibra.liquid.DataStructures;
using com.zibra.liquid.Manipulators;
using com.zibra.liquid.Solver;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;

public class WaterController : MonoBehaviour
{
    public CheckValveCollision checkValveCollision;

    [SerializeField]
    GameObject testCockManager;

    [SerializeField]
    GameObject shutOffValveManager;
    [SerializeField]
    GameObject sightTube;
    public CheckValveStatus checkValveStatus;

    TestCockController testCockController;
    ShutOffValveController shutOffValveController;
    public TestKitManager testKitManager;

    [SerializeField]
    public GameObject ShutOffValve1;

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

    [SerializeField]
    public ZibraLiquidForceField check1housingForceField;

    [SerializeField]
    public ZibraLiquidForceField check2housingForceField;

    // public bool isCheck1Closed;
    // public bool isCheck2Closed;

    [SerializeField]
    ZibraLiquidVoid Void_Check1;

    [SerializeField]
    ZibraLiquidVoid Void_Check2;

    Coroutine MaxParticleNumberRegulation;


    float tc4ffScaleUpSpeed;
    float tc4ffScaleDownSpeed;

    //Vector3 Zone2VoidMaxSize = new Vector3(0.045f, 0.035f, 0.02f);
    public Vector3 check1VoidMaxSize = new Vector3(0.045f, 0.0354f, 0.0201f);
    public Vector3 check2VoidMaxSize = new Vector3(0.045f, 0.0354f, 0.0201f);
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

    public GameObject CheckValve1;
    public GameObject CheckValve2;


    Vector3 CheckValve1StartingPos;
    Vector3 CheckValve2StartingPos;
    Vector3 supplyColliderPos;
    Vector3 supplyColliderClosedPos;
    Vector3 initSupplyColliderPos;
    Vector3 supplyColliderTargetPos = new Vector3(-15f, 0, 0);
    Vector3 supplyVoidTargetPos = new Vector3(-9.5f, 0, 0);
    Vector3 initSupplyVoidPos;


    Vector3 initSupplyVoidScale;
    Vector3 currentSupplyVoidScale;
    Vector3 targetSupplyVoidScale;
    Vector3 check1VoidRef = Vector3.zero;
    Vector3 check2VoidRef = Vector3.zero;
    Vector3 check1VoidTC1Ref = Vector3.zero;
    Vector3 check1FFref = Vector3.zero;
    Vector3 check2FFref = Vector3.zero;
    Vector3 testCockFF1Ref = Vector3.zero;
    Vector3 testCockFF2Ref = Vector3.zero;
    Vector3 testCockFF3Ref = Vector3.zero;
    Vector3 testCockFF4Ref = Vector3.zero;

    public float testCock1MaxStr;

    public float testCock2MaxStr;

    public float testCock3MaxStr;

    public float testCock3MinStr;

    public float testCock3Str;

    public int testCock4MinStr;
    public int testCock4MaxStr;
    public float testCock4Str;
    public GameObject checkValve1;
    public GameObject checkValve2;
    Rigidbody check1Rb;
    Rigidbody check2Rb;
    bool oneTime = false;
    public ZibraLiquid liquid;
    ZibraLiquidSolverParameters liquidSolverParameters;
    public bool randomizePressure = true;
    [SerializeField]
    bool isAttachedToGauge;
    public bool isTeachingModeEnabled = false;
    //check 2 open= Vector3(-0.18547225,1.49199536e-06,-0.178497031)
    //check 1 open= Vector3(-0.0897347033, 1.77071377e-06, -0.085896723)
    Vector3 check1OpenPos = new Vector3(-0.0897347033f, 1.77071377e-06f, -0.085896723f);
    Vector3 check2openPos = new Vector3(-0.18547225f, 1.49199536e-06f, -0.178497031f);
    public float initialCheck1Mass;
    public float initialCheck2Mass;
    public float inputForce;
    public float zone1Pressure;
    public float zone2Pressure;
    public float zone3Pressure;
    public float check1SpringForce;
    public float check2SpringForce;


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
    }

    /// <summary>
    /// This function is called when the object becomes enabled and active.
    /// </summary>
    private void OnEnable()
    {
        testCock4Str = Random.Range(testCock4MinStr, testCock4MaxStr);
        testCock3Str = Random.Range(testCock3MinStr, testCock3MaxStr);
        CheckValve1StartingPos = checkValve1.transform.position;
        CheckValve2StartingPos = checkValve2.transform.position;
        liquidSolverParameters = liquid.GetComponent<ZibraLiquidSolverParameters>();
    }

    /// <summary>
    /// This function is called when the behaviour becomes disabled or inactive.
    /// </summary>
    private void OnDisable()
    {

    }
    void PressureZoneRegulate()
    {
        zone2Pressure = zone1Pressure - check1SpringForce;
        zone3Pressure = zone2Pressure - check2SpringForce;
    }
    /// <summary>
    /// Contains logic for test cocks and check valved if the sight tube is disconnected
    /// </summary>
    void NoSightTubeConnected()
    {
        /// <summary>
        /// Sight Tube NOT connected----------------------------------------
        /// </summary>
        foreach (GameObject testCock in testKitManager.StaticTestCockList)
        {
            testCock.GetComponent<AssignTestCockManipulators>().testCockVoid.enabled = true;
            testCock.GetComponent<AssignTestCockManipulators>().testCockCollider.enabled =
                false;
        }

        //while shutoff valve is open regulate ff in check housing according to amount of water being supplied supply
        check1housingForceField.Strength = Mathf.SmoothDamp(
            check1housingForceField.Strength,
            1.2f,
            ref check1FFref.x,
            0.2f
        );
        check2housingForceField.Strength = Mathf.SmoothDamp(
            check2housingForceField.Strength,
            1f,
            ref check2FFref.x,
            0.5f
        );

        Void_Check1.transform.localScale = Vector3.zero;

        Void_Check2.transform.localScale = Vector3.zero;
        //tc2 non-static condition pressure
        if (
                testCockController.isTestCock2Open == true
                && TestCockHoseDetect2.isConnected == false
            )
        {
            if (check1Detector.ParticlesInside > 3000)
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

        //tc3 non-static condition pressure
        if (
                testCockController.isTestCock3Open
                && TestCockHoseDetect3.isConnected == false
            )
        {
            if (check1Detector.ParticlesInside > 3000)
            {
                TestCockFF3.Strength = Mathf.SmoothDamp(
                    TestCockFF3.Strength,
                    Mathf.Clamp(check1Detector.ParticlesInside, 0, testCock3Str),
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
                testCockController.isTestCock4Open
                && TestCockHoseDetect4.isConnected == false
            )
        {
            if (check2Detector.ParticlesInside > 3000)
            {
                TestCockFF4.Strength = Mathf.SmoothDamp(
                 TestCockFF4.Strength,
                 Mathf.Clamp(check2Detector.ParticlesInside, 0, testCock4Str),
                 ref testCockFF4Ref.x,
                 tc4ffScaleUpSpeed
             );

            }

        }
        else
        {
            TestCockFF4.Strength = 0;
        }
        if (shutOffValveController.IsSupplyOn == false && shutOffValveController.IsSecondShutOffOpen == true)
        {

            foreach (GameObject testCock in testKitManager.StaticTestCockList)
            {
                testCock.GetComponent<AssignTestCockManipulators>().testCockVoid.enabled = false;
                testCock.GetComponent<AssignTestCockManipulators>().testCockCollider.enabled = true;
            }
            Void_Check1.transform.localScale = Vector3.SmoothDamp(
                Void_Check1.transform.localScale,
                check1VoidMaxSize * TestCockFF3.Strength,
                ref check1VoidRef,
                Check1VoidGrowSpeed
            );

            Void_Check2.transform.localScale = Vector3.SmoothDamp(
                Void_Check2.transform.localScale,
                check2VoidMaxSize * TestCockFF4.Strength,
                ref check2VoidRef,
                Check2VoidGrowSpeed
            );
            if (
             testCockController.isTestCock2Open == true
             && TestCockHoseDetect2.isConnected == false
         )
            {
                if (check1Detector.ParticlesInside > 3000)
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

            //tc3 non-static condition pressure
            if (
                    testCockController.isTestCock3Open
                    && TestCockHoseDetect3.isConnected == false
                )
            {
                if (check1Detector.ParticlesInside > 3000)
                {
                    TestCockFF3.Strength = Mathf.SmoothDamp(
                        TestCockFF3.Strength,
                        Mathf.Clamp(check1Detector.ParticlesInside, 0, testCock3Str),
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
                    testCockController.isTestCock4Open
                    && TestCockHoseDetect4.isConnected == false
                )
            {
                if (check2Detector.ParticlesInside > 3000)
                {
                    TestCockFF4.Strength = Mathf.SmoothDamp(
                     TestCockFF4.Strength,
                     Mathf.Clamp(check2Detector.ParticlesInside, 0, testCock4Str),
                     ref testCockFF4Ref.x,
                     tc4ffScaleUpSpeed
                 );

                }

            }
            else
            {
                TestCockFF4.Strength = 0;
            }
        }
        else if (shutOffValveController.IsSupplyOn == true && shutOffValveController.IsSecondShutOffOpen == false)
        {

            foreach (GameObject testCock in testKitManager.StaticTestCockList)
            {
                testCock.GetComponent<AssignTestCockManipulators>().testCockVoid.enabled = true;
                testCock.GetComponent<AssignTestCockManipulators>().testCockCollider.enabled =
                    false;
            }
            //while shutoff valve is open regulate ff in check housing according to amount of water being supplied supply
            check1housingForceField.Strength = Mathf.SmoothDamp(
                check1housingForceField.Strength,
                1.2f,
                ref check1FFref.x,
                0.2f
            );
            check2housingForceField.Strength = Mathf.SmoothDamp(
                check2housingForceField.Strength,
                1f,
                ref check2FFref.x,
                0.5f
            );

            Void_Check1.transform.localScale = Vector3.zero;

            Void_Check2.transform.localScale = Vector3.zero;
            if (
                 testCockController.isTestCock2Open == true
                 && TestCockHoseDetect2.isConnected == false
             )
            {
                if (check1Detector.ParticlesInside > 3000)
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

            //tc3 non-static condition pressure
            if (
                    testCockController.isTestCock3Open
                    && TestCockHoseDetect3.isConnected == false
                )
            {
                if (check1Detector.ParticlesInside > 3000)
                {
                    TestCockFF3.Strength = Mathf.SmoothDamp(
                        TestCockFF3.Strength,
                        Mathf.Clamp(check1Detector.ParticlesInside, 0, testCock3Str),
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
                    testCockController.isTestCock4Open
                    && TestCockHoseDetect4.isConnected == false
                )
            {
                if (check2Detector.ParticlesInside > 3000)
                {
                    TestCockFF4.Strength = Mathf.SmoothDamp(
                     TestCockFF4.Strength,
                     Mathf.Clamp(check2Detector.ParticlesInside, 0, testCock4Str),
                     ref testCockFF4Ref.x,
                     tc4ffScaleUpSpeed
                 );

                }

            }
            else
            {
                TestCockFF4.Strength = 0;
            }
        }
    }
    /// <summary>
    /// Contains logic for test cocks and check valved if the sight tube is connected
    /// </summary>
    void SightTubeConnected()
    {
        foreach (GameObject testCock in testKitManager.StaticTestCockList)
        {
            testCock.GetComponent<AssignTestCockManipulators>().testCockVoid.enabled = true;
            testCock.GetComponent<AssignTestCockManipulators>().testCockCollider.enabled =
                false;
        }

        //while shutoff valve is open regulate ff in check housing according to amount of water being supplied supply
        check1housingForceField.Strength = Mathf.SmoothDamp(
            check1housingForceField.Strength,
            1.2f,
            ref check1FFref.x,
            0.2f
        );
        check2housingForceField.Strength = Mathf.SmoothDamp(
            check2housingForceField.Strength,
            1f,
            ref check2FFref.x,
            0.5f
        );

        Void_Check1.transform.localScale = Vector3.zero;

        Void_Check2.transform.localScale = Vector3.zero;
        //tc2 non-static condition pressure
        if (
                testCockController.isTestCock2Open == true
                && TestCockHoseDetect2.isConnected == false
            )
        {
            if (check1Detector.ParticlesInside > 3000)
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

        //tc3 non-static condition pressure
        if (
                testCockController.isTestCock3Open
                && TestCockHoseDetect3.isConnected == false
            )
        {
            if (check1Detector.ParticlesInside > 3000)
            {
                TestCockFF3.Strength = Mathf.SmoothDamp(
                    TestCockFF3.Strength,
                    Mathf.Clamp(check1Detector.ParticlesInside, 0, testCock3Str),
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
                testCockController.isTestCock4Open
                && TestCockHoseDetect4.isConnected == false
            )
        {
            if (check2Detector.ParticlesInside > 3000)
            {
                TestCockFF4.Strength = Mathf.SmoothDamp(
                 TestCockFF4.Strength,
                 Mathf.Clamp(check2Detector.ParticlesInside, 0, testCock4Str),
                 ref testCockFF4Ref.x,
                 tc4ffScaleUpSpeed
             );

            }

        }
        else
        {
            TestCockFF4.Strength = 0;
        }
        if (shutOffValveController.IsSupplyOn == false && shutOffValveController.IsSecondShutOffOpen == true)
        {

            foreach (GameObject testCock in testKitManager.StaticTestCockList)
            {
                testCock.GetComponent<AssignTestCockManipulators>().testCockVoid.enabled = false;
                testCock.GetComponent<AssignTestCockManipulators>().testCockCollider.enabled = true;
            }
            Void_Check1.transform.localScale = Vector3.SmoothDamp(
                Void_Check1.transform.localScale,
                check1VoidMaxSize * TestCockFF3.Strength,
                ref check1VoidRef,
                Check1VoidGrowSpeed
            );

            Void_Check2.transform.localScale = Vector3.SmoothDamp(
                Void_Check2.transform.localScale,
                check2VoidMaxSize * TestCockFF4.Strength,
                ref check2VoidRef,
                Check2VoidGrowSpeed
            );
            if (
             testCockController.isTestCock2Open == true
             && TestCockHoseDetect2.isConnected == false
         )
            {
                if (check1Detector.ParticlesInside > 3000)
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

            //tc3 non-static condition pressure
            if (
                    testCockController.isTestCock3Open
                    && TestCockHoseDetect3.isConnected == false
                )
            {
                if (check1Detector.ParticlesInside > 3000)
                {
                    TestCockFF3.Strength = Mathf.SmoothDamp(
                        TestCockFF3.Strength,
                        Mathf.Clamp(check1Detector.ParticlesInside, 0, testCock3Str),
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
                    testCockController.isTestCock4Open
                    && TestCockHoseDetect4.isConnected == false
                )
            {
                if (check2Detector.ParticlesInside > 3000)
                {
                    TestCockFF4.Strength = Mathf.SmoothDamp(
                     TestCockFF4.Strength,
                     Mathf.Clamp(check2Detector.ParticlesInside, 0, testCock4Str),
                     ref testCockFF4Ref.x,
                     tc4ffScaleUpSpeed
                 );

                }

            }
            else
            {
                TestCockFF4.Strength = 0;
            }
        }
        else if (shutOffValveController.IsSupplyOn == true && shutOffValveController.IsSecondShutOffOpen == false)
        {

            foreach (GameObject testCock in testKitManager.StaticTestCockList)
            {
                testCock.GetComponent<AssignTestCockManipulators>().testCockVoid.enabled = true;
                testCock.GetComponent<AssignTestCockManipulators>().testCockCollider.enabled =
                    false;
            }
            //while shutoff valve is open regulate ff in check housing according to amount of water being supplied supply
            check1housingForceField.Strength = Mathf.SmoothDamp(
                check1housingForceField.Strength,
                1.2f,
                ref check1FFref.x,
                0.2f
            );
            check2housingForceField.Strength = Mathf.SmoothDamp(
                check2housingForceField.Strength,
                1f,
                ref check2FFref.x,
                0.5f
            );

            Void_Check1.transform.localScale = Vector3.zero;

            Void_Check2.transform.localScale = Vector3.zero;
            if (
                 testCockController.isTestCock2Open == true
                 && TestCockHoseDetect2.isConnected == false
             )
            {
                if (check1Detector.ParticlesInside > 3000)
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

            //tc3 non-static condition pressure
            if (
                    testCockController.isTestCock3Open
                    && TestCockHoseDetect3.isConnected == false
                )
            {
                if (check1Detector.ParticlesInside > 3000)
                {
                    TestCockFF3.Strength = Mathf.SmoothDamp(
                        TestCockFF3.Strength,
                        Mathf.Clamp(check1Detector.ParticlesInside, 0, testCock3Str),
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
                    testCockController.isTestCock4Open
                    && TestCockHoseDetect4.isConnected == false
                )
            {
                if (check2Detector.ParticlesInside > 3000)
                {
                    TestCockFF4.Strength = Mathf.SmoothDamp(
                     TestCockFF4.Strength,
                     Mathf.Clamp(check2Detector.ParticlesInside, 0, testCock4Str),
                     ref testCockFF4Ref.x,
                     tc4ffScaleUpSpeed
                 );

                }

            }
            else
            {
                TestCockFF4.Strength = 0;
            }
        }
    }

    // Update is called once per frame
    void TestingProceduresNonTeaching()
    {

        //test cock #2 pressure regulation
        check1housingForceField.Strength = 0;
        check2housingForceField.Strength = 0;

        foreach (GameObject testCock in testKitManager.StaticTestCockList)
        {
            testCock.GetComponent<AssignTestCockManipulators>().testCockVoid.enabled = false;
            testCock.GetComponent<AssignTestCockManipulators>().testCockCollider.enabled = true;
        }
        Void_Check1.transform.localScale = Vector3.SmoothDamp(
            Void_Check1.transform.localScale,
            check1VoidMaxSize * TestCockFF3.Strength,
            ref check1VoidRef,
            Check1VoidGrowSpeed
        );

        Void_Check2.transform.localScale = Vector3.SmoothDamp(
            Void_Check2.transform.localScale,
            check2VoidMaxSize * TestCockFF4.Strength,
            ref check2VoidRef,
            Check2VoidGrowSpeed
        );

        if (
             testCockController.isTestCock2Open == true
             && TestCockHoseDetect2.isConnected == false
      )
        {


            check1housingForceField.Strength = Mathf.SmoothDamp(
               check1housingForceField.Strength,
               1.2f,
               ref check1FFref.x,
               0.2f
           );
            check2housingForceField.Strength = Mathf.SmoothDamp(
                check2housingForceField.Strength,
                1f,
                ref check2FFref.x,
                1f
            );


            //release initial pressure
            if (check1Detector.ParticlesInside > 3000)
            {
                TestCockFF2.Strength = Mathf.SmoothDamp(
                    TestCockFF2.Strength,
                    Mathf.Clamp(check1Detector.ParticlesInside, 0, testCock2MaxStr),
                    ref testCockFF2Ref.x,
                    0.005f
                );

            }
            //pressure decrease
            else
            {
                TestCockFF2.Strength = Mathf.SmoothDamp(
                    TestCockFF2.Strength,
                    0,
                    ref testCockFF2Ref.x,
                    3f
                );
                check1housingForceField.Strength = 0;
                check2housingForceField.Strength = 0;

            }
        }
        else
        {

            TestCockFF2.Strength = 0;
        }
        //test cock #3 pressure regulation
        //static conditions and testkit hooked up
        if (
          testCockController.isTestCock3Open == true
          && TestCockHoseDetect3.isConnected == false
         )
        {

            check1housingForceField.Strength = Mathf.SmoothDamp(
                   check1housingForceField.Strength,
                   1.2f,
                   ref check1FFref.x,
                   0.2f
               );
            check2housingForceField.Strength = Mathf.SmoothDamp(
                check2housingForceField.Strength,
                1f,
                ref check2FFref.x,
                1f
            );
            if (!oneTime)
            {
                check1Rb.AddForce(new Vector3(1.0f, 1.0f, 0) * 10, ForceMode.Impulse);
                oneTime = !oneTime;
            }


            //release initial pressure
            if (check1Detector.ParticlesInside > 3000)
            {
                TestCockFF3.Strength = Mathf.SmoothDamp(
                    TestCockFF3.Strength,
                    Mathf.Clamp(check1Detector.ParticlesInside, 0, testCock3Str),
                    ref testCockFF3Ref.x,
                    0.005f
                );
                //Open check valve when tc is open and testing
                check1Rb.mass = 10;
                check1Rb.AddForce(new Vector3(1, 1, 0) * 100, ForceMode.Impulse);
            }
            //pressure decrease
            else
            {
                TestCockFF3.Strength = Mathf.SmoothDamp(
                    TestCockFF3.Strength,
                    0,
                    ref testCockFF3Ref.x,
                    1f
                );

                check1housingForceField.Strength = 0;
                check2housingForceField.Strength = 0;

                //pressure stop
                if (checkValveStatus.isCheck1Closed == true)
                {
                    //after opening for test, reset check valve mass to its initial value and keep check valve closed using initial position
                    check1Rb.mass = initialCheck1Mass;
                    TestCockFF3.Strength = 0;
                    checkValve1.transform.position = CheckValve1StartingPos;
                }


            }
        }
        else
        {

            TestCockFF3.Strength = 0;
        }
        //test cock #4 pressure regulation
        //static conditions and testkit hooked up
        if (
                   testCockController.isTestCock4Open == true
                   && TestCockHoseDetect4.isConnected == false
                  )
        {
            check1housingForceField.Strength = Mathf.SmoothDamp(
                check1housingForceField.Strength,
                1.2f,
                ref check1FFref.x,
                0.2f
            );
            check2housingForceField.Strength = Mathf.SmoothDamp(
                check2housingForceField.Strength,
                1f,
                ref check2FFref.x,
                1f
            );
            if (!oneTime)
            {
                check2Rb.AddForce(new Vector3(1.0f, 1.0f, 0) * 8, ForceMode.Impulse);
                oneTime = !oneTime;

            }

            //release initial pressure
            if (check2Detector.ParticlesInside > 3000)
            {
                TestCockFF4.Strength = Mathf.SmoothDamp(
              TestCockFF4.Strength,
              Mathf.Clamp(check2Detector.ParticlesInside, 0, testCock4Str),
              ref testCockFF4Ref.x,
              tc4ffScaleUpSpeed
              );
                //Open check valve when tc is open and testing
                check2Rb.mass = 10;
                check2Rb.AddForce(new Vector3(1, 1, 0) * 100, ForceMode.Impulse);
            }

            else
            {
                TestCockFF4.Strength = Mathf.SmoothDamp(
                TestCockFF4.Strength,
                0,
                ref testCockFF4Ref.x,
                tc4ffScaleDownSpeed
                );

                check1housingForceField.Strength = 0;
                check2housingForceField.Strength = 0;

                //pressure stop
                if (checkValveStatus.isCheck2Closed == true)
                {
                    //after opening for test, reset check valve mass to its initial value and keep check valve closed using initial position
                    check2Rb.mass = initialCheck2Mass;
                    TestCockFF4.Strength = 0;
                    checkValve2.transform.position = CheckValve2StartingPos;
                }

            }
        }
        else
        {

            TestCockFF4.Strength = 0;
        }


        /// <summary>
        ///End testing procedures------------------------------------------------------
        /// </summary>
    }
    void NonTestingProceduresNonTeaching()
    {

        /// <summary>
        /// Sight Tube NOT connected----------------------------------------
        /// </summary>
        if (!testKitManager.AttachedHoseList.Contains(sightTube))
        {
            NoSightTubeConnected();
        }
        else
        {
            Debug.Log($"sightTube is present");
            SightTubeConnected();
        }

    }
    void WaterOperations()
    {
        /// <summary>
        ///Testing procedures---------------------------------------------------------------------------------------------
        /// Static conditions and testkit hooked up || not hooked up------------------------------------------------------
        /// </summary>

        if (shutOffValveController.IsSupplyOn == false && shutOffValveController.IsSecondShutOffOpen == false)
        {
            TestingProceduresNonTeaching();
        }

        /// <summary>
        ///Non-Testing conditions operation---------------------------------------------------
        /// </summary>
        else if (shutOffValveController.IsSupplyOn == true && shutOffValveController.IsSecondShutOffOpen == true)
        {
            NonTestingProceduresNonTeaching();
        }

    }
    void TeachingWaterOperations()
    {
        if (zone1Pressure < (check1SpringForce + zone2Pressure))
        {
            // check1Rb.AddForce(new Vector3(-1, -1, 0) * inputForce, ForceMode.Force);
            check1Rb.AddForce(new Vector3(-1, -1, 0) * inputForce, ForceMode.Force);
        }
        if (zone2Pressure < (check2SpringForce + zone3Pressure))
        {
            // check1Rb.AddForce(new Vector3(-1, -1, 0) * inputForce, ForceMode.Force);
            check2Rb.AddForce(new Vector3(-1, -1, 0) * inputForce, ForceMode.Force);
        }

        /// <summary>
        ///Testing procedures---------------------------------------------------------------------------------------------
        /// Static conditions and testkit hooked up || not hooked up------------------------------------------------------
        /// </summary>

        if (shutOffValveController.IsSupplyOn == false && shutOffValveController.IsSecondShutOffOpen == false)
        {
            //test cock #2 pressure regulation
            check1housingForceField.Strength = 0;
            check2housingForceField.Strength = 0;

            foreach (GameObject testCock in testKitManager.StaticTestCockList)
            {
                testCock.GetComponent<AssignTestCockManipulators>().testCockVoid.enabled = false;
                testCock.GetComponent<AssignTestCockManipulators>().testCockCollider.enabled = true;
            }
            Void_Check1.transform.localScale = Vector3.SmoothDamp(
                Void_Check1.transform.localScale,
                check1VoidMaxSize * TestCockFF3.Strength,
                ref check1VoidRef,
                Check1VoidGrowSpeed
            );

            Void_Check2.transform.localScale = Vector3.SmoothDamp(
                Void_Check2.transform.localScale,
                check2VoidMaxSize * TestCockFF4.Strength,
                ref check2VoidRef,
                Check2VoidGrowSpeed
            );

            if (
                 testCockController.isTestCock2Open == true
                 && TestCockHoseDetect2.isConnected == false
          )
            {

                check1housingForceField.Strength = Mathf.SmoothDamp(
                   check1housingForceField.Strength,
                   1.2f,
                   ref check1FFref.x,
                   0.2f
               );
                check2housingForceField.Strength = Mathf.SmoothDamp(
                    check2housingForceField.Strength,
                    1f,
                    ref check2FFref.x,
                    1f
                );


                //release initial pressure
                if (check1Detector.ParticlesInside > 3000)
                {
                    TestCockFF2.Strength = Mathf.SmoothDamp(
                        TestCockFF2.Strength,
                        Mathf.Clamp(check1Detector.ParticlesInside, 0, testCock2MaxStr),
                        ref testCockFF2Ref.x,
                        0.005f
                    );
                }
                //pressure decrease
                else
                {
                    TestCockFF2.Strength = Mathf.SmoothDamp(
                        TestCockFF2.Strength,
                        0,
                        ref testCockFF2Ref.x,
                        3f
                    );
                    check1housingForceField.Strength = 0;
                    check2housingForceField.Strength = 0;

                }
            }
            else
            {

                TestCockFF2.Strength = 0;
            }
            //test cock #3 pressure regulation
            //static conditions and testkit hooked up
            if (
              testCockController.isTestCock3Open == true
              && TestCockHoseDetect3.isConnected == false
             )
            {

                check1housingForceField.Strength = Mathf.SmoothDamp(
                       check1housingForceField.Strength,
                       1.2f,
                       ref check1FFref.x,
                       0.2f
                   );
                check2housingForceField.Strength = Mathf.SmoothDamp(
                    check2housingForceField.Strength,
                    1f,
                    ref check2FFref.x,
                    1f
                );
                if (!oneTime)
                {
                    check1Rb.AddForce(new Vector3(1.0f, 1.0f, 0) * 10, ForceMode.Impulse);
                    oneTime = !oneTime;
                }


                //release initial pressure
                if (check1Detector.ParticlesInside > 3000)
                {
                    TestCockFF3.Strength = Mathf.SmoothDamp(
                        TestCockFF3.Strength,
                        Mathf.Clamp(check1Detector.ParticlesInside, 0, testCock3Str),
                        ref testCockFF3Ref.x,
                        0.005f
                    );
                }
                //pressure decrease
                else
                {
                    TestCockFF3.Strength = Mathf.SmoothDamp(
                        TestCockFF3.Strength,
                        0,
                        ref testCockFF3Ref.x,
                        1f
                    );

                    check1housingForceField.Strength = 0;
                    check2housingForceField.Strength = 0;

                    //pressure stop
                    if (checkValveStatus.isCheck1Closed == true)
                    {
                        TestCockFF3.Strength = 0;
                    }


                }
            }
            else
            {

                TestCockFF3.Strength = 0;
            }
            //test cock #4 pressure regulation
            //static conditions and testkit hooked up
            if (
                       testCockController.isTestCock4Open == true
                       && TestCockHoseDetect4.isConnected == false
                      )
            {
                check1housingForceField.Strength = Mathf.SmoothDamp(
                    check1housingForceField.Strength,
                    1.2f,
                    ref check1FFref.x,
                    0.2f
                );
                check2housingForceField.Strength = Mathf.SmoothDamp(
                    check2housingForceField.Strength,
                    1f,
                    ref check2FFref.x,
                    1f
                );
                if (!oneTime)
                {
                    check2Rb.AddForce(new Vector3(1.0f, 1.0f, 0) * 8, ForceMode.Impulse);
                    oneTime = !oneTime;

                }

                //release initial pressure
                if (check2Detector.ParticlesInside > 3000)
                {
                    TestCockFF4.Strength = Mathf.SmoothDamp(
                  TestCockFF4.Strength,
                  Mathf.Clamp(check2Detector.ParticlesInside, 0, testCock4Str),
                  ref testCockFF4Ref.x,
                  tc4ffScaleUpSpeed
                  );
                }
                else
                {
                    TestCockFF4.Strength = Mathf.SmoothDamp(
                    TestCockFF4.Strength,
                    0,
                    ref testCockFF4Ref.x,
                    tc4ffScaleDownSpeed
                    );

                    check1housingForceField.Strength = 0;
                    check2housingForceField.Strength = 0;

                    //pressure stop
                    if (checkValveStatus.isCheck2Closed == true)
                    {
                        TestCockFF4.Strength = 0;
                        checkValve2.transform.position = CheckValve2StartingPos;

                    }

                }
            }
            else
            {

                TestCockFF4.Strength = 0;
            }

        }
        /// <summary>
        ///End testing procedures------------------------------------------------------
        /// </summary>


        /// <summary>
        ///Non-Testing conditions operation---------------------------------------------------
        /// </summary>
        else if (shutOffValveController.IsSupplyOn == true && shutOffValveController.IsSecondShutOffOpen == true)
        {

            foreach (GameObject testCock in testKitManager.StaticTestCockList)
            {
                testCock.GetComponent<AssignTestCockManipulators>().testCockVoid.enabled = true;
                testCock.GetComponent<AssignTestCockManipulators>().testCockCollider.enabled =
                    false;
            }
            //while shutoff valve is open regulate ff in check housing according to amount of water being supplied supply
            check1housingForceField.Strength = Mathf.SmoothDamp(
                check1housingForceField.Strength,
                1.2f,
                ref check1FFref.x,
                0.2f
            );
            check2housingForceField.Strength = Mathf.SmoothDamp(
                check2housingForceField.Strength,
                1f,
                ref check2FFref.x,
                0.5f
            );

            Void_Check1.transform.localScale = Vector3.zero;

            Void_Check2.transform.localScale = Vector3.zero;
            //tc2 non-static condition pressure
            if (
                    testCockController.isTestCock2Open == true
                    && TestCockHoseDetect2.isConnected == false
                )
            {
                if (check1Detector.ParticlesInside > 3000)
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

            //tc3 non-static condition pressure
            if (
                    testCockController.isTestCock3Open
                    && TestCockHoseDetect3.isConnected == false
                )
            {
                if (check1Detector.ParticlesInside > 3000)
                {
                    TestCockFF3.Strength = Mathf.SmoothDamp(
                        TestCockFF3.Strength,
                        Mathf.Clamp(check1Detector.ParticlesInside, 0, testCock3Str),
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
                    testCockController.isTestCock4Open
                    && TestCockHoseDetect4.isConnected == false
                )
            {
                if (check2Detector.ParticlesInside > 3000)
                {
                    TestCockFF4.Strength = Mathf.SmoothDamp(
                     TestCockFF4.Strength,
                     Mathf.Clamp(check2Detector.ParticlesInside, 0, testCock4Str),
                     ref testCockFF4Ref.x,
                     tc4ffScaleUpSpeed
                 );

                }

            }
            else
            {
                TestCockFF4.Strength = 0;
            }
        }
        else if (shutOffValveController.IsSupplyOn == false && shutOffValveController.IsSecondShutOffOpen == true)
        {

            foreach (GameObject testCock in testKitManager.StaticTestCockList)
            {
                testCock.GetComponent<AssignTestCockManipulators>().testCockVoid.enabled = false;
                testCock.GetComponent<AssignTestCockManipulators>().testCockCollider.enabled = true;
            }
            Void_Check1.transform.localScale = Vector3.SmoothDamp(
                Void_Check1.transform.localScale,
                check1VoidMaxSize * TestCockFF3.Strength,
                ref check1VoidRef,
                Check1VoidGrowSpeed
            );

            Void_Check2.transform.localScale = Vector3.SmoothDamp(
                Void_Check2.transform.localScale,
                check2VoidMaxSize * TestCockFF4.Strength,
                ref check2VoidRef,
                Check2VoidGrowSpeed
            );
            if (
             testCockController.isTestCock2Open == true
             && TestCockHoseDetect2.isConnected == false
         )
            {
                if (check1Detector.ParticlesInside > 3000)
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

            //tc3 non-static condition pressure
            if (
                    testCockController.isTestCock3Open
                    && TestCockHoseDetect3.isConnected == false
                )
            {
                if (check1Detector.ParticlesInside > 3000)
                {
                    TestCockFF3.Strength = Mathf.SmoothDamp(
                        TestCockFF3.Strength,
                        Mathf.Clamp(check1Detector.ParticlesInside, 0, testCock3Str),
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
                    testCockController.isTestCock4Open
                    && TestCockHoseDetect4.isConnected == false
                )
            {
                if (check2Detector.ParticlesInside > 3000)
                {
                    TestCockFF4.Strength = Mathf.SmoothDamp(
                     TestCockFF4.Strength,
                     Mathf.Clamp(check2Detector.ParticlesInside, 0, testCock4Str),
                     ref testCockFF4Ref.x,
                     tc4ffScaleUpSpeed
                 );

                }

            }
            else
            {
                TestCockFF4.Strength = 0;
            }
        }
        else if (shutOffValveController.IsSupplyOn == true && shutOffValveController.IsSecondShutOffOpen == false)
        {

            foreach (GameObject testCock in testKitManager.StaticTestCockList)
            {
                testCock.GetComponent<AssignTestCockManipulators>().testCockVoid.enabled = true;
                testCock.GetComponent<AssignTestCockManipulators>().testCockCollider.enabled =
                    false;
            }
            //while shutoff valve is open regulate ff in check housing according to amount of water being supplied supply
            check1housingForceField.Strength = Mathf.SmoothDamp(
                check1housingForceField.Strength,
                1.2f,
                ref check1FFref.x,
                0.2f
            );
            check2housingForceField.Strength = Mathf.SmoothDamp(
                check2housingForceField.Strength,
                1f,
                ref check2FFref.x,
                0.5f
            );

            Void_Check1.transform.localScale = Vector3.zero;

            Void_Check2.transform.localScale = Vector3.zero;
            if (
                 testCockController.isTestCock2Open == true
                 && TestCockHoseDetect2.isConnected == false
             )
            {
                if (check1Detector.ParticlesInside > 3000)
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

            //tc3 non-static condition pressure
            if (
                    testCockController.isTestCock3Open
                    && TestCockHoseDetect3.isConnected == false
                )
            {
                if (check1Detector.ParticlesInside > 3000)
                {
                    TestCockFF3.Strength = Mathf.SmoothDamp(
                        TestCockFF3.Strength,
                        Mathf.Clamp(check1Detector.ParticlesInside, 0, testCock3Str),
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
                    testCockController.isTestCock4Open
                    && TestCockHoseDetect4.isConnected == false
                )
            {
                if (check2Detector.ParticlesInside > 3000)
                {
                    TestCockFF4.Strength = Mathf.SmoothDamp(
                     TestCockFF4.Strength,
                     Mathf.Clamp(check2Detector.ParticlesInside, 0, testCock4Str),
                     ref testCockFF4Ref.x,
                     tc4ffScaleUpSpeed
                 );

                }

            }
            else
            {
                TestCockFF4.Strength = 0;
            }
        }
    }

    void Update()
    {

        /// <summary>
        /// Regulate pressure zones
        /// </summary>

        //PressureZoneRegulate();

        /// <summary>
        /// Regulate supply pressure-------------------------------------------------------
        /// </summary>
        supplyColliderTargetPos.x =
            shutOffValveController.ShutOffValve1.transform.eulerAngles.z / 90;
        supplyCollider.transform.localPosition = initSupplyColliderPos + supplyColliderTargetPos;
        supplyVoidTargetPos.x = shutOffValveController.ShutOffValve1.transform.eulerAngles.z / 90;
        supplyVoid.transform.localPosition = initSupplyVoidPos - supplyVoidTargetPos;
        /// <summary>
        /// End Regulate supply pressure----------------------------------------------------
        /// </summary>
        /// <summary>
        /// test cock #1 pressure regulation------------------------------------------------
        /// </summary>

        if (testCockController.isTestCock1Open == true
            && TestCockHoseDetect1.isConnected == false
            && shutOffValveController.IsSupplyOn == false
            && shutOffValveController.IsSecondShutOffOpen == false
            )
        {
            //both shutoffs closed
            Debug.Log($"here");
            TestCockFF1.Strength = 0f;
            TestCock1Emitter.enabled = true;


        }
        else if (testCockController.isTestCock1Open == true
             && TestCockHoseDetect1.isConnected == false
             && shutOffValveController.IsSupplyOn == false
            )
        {
            // only shutoff #2 closed
            TestCock1Emitter.enabled = true;

        }
        else if (testCockController.isTestCock1Open == false
            && TestCockHoseDetect1.isConnected == false
            && shutOffValveController.IsSupplyOn == false
           )
        {
            TestCock1Emitter.enabled = false;


        }
        else if (testCockController.isTestCock1Open == true
           && TestCockHoseDetect1.isConnected == false
           && shutOffValveController.IsSupplyOn == true
          )
        {
            TestCockFF1.Strength = 2f;
        }
        else if (testCockController.isTestCock1Open == false
           && TestCockHoseDetect1.isConnected == false
           && shutOffValveController.IsSupplyOn == true
          )
        {
            TestCockFF1.Strength = 0f;
        }

        /// <summary>
        /// test cock #1 pressure regulation--------------------------------------------------
        /// </summary>






    }
    void FixedUpdate()
    {

        if (isTeachingModeEnabled)
        {
            TeachingWaterOperations();
        }
        else
        {
            WaterOperations();
        }
    }
}
