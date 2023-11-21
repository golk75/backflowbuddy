
using System.Collections;
using com.zibra.liquid.DataStructures;
using com.zibra.liquid.Manipulators;
using com.zibra.liquid.SDFObjects;
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
    GameObject hoseController;
    [SerializeField]
    GameObject sightTube;
    public CheckValveStatus checkValveStatus;

    TestCockController testCockController;
    ShutOffValveController shutOffValveController;
    public DoubleCheckTestKitController doubleCheckTestKitController;
    [SerializeField]
    SightTubeController sightTubeController;
    [SerializeField]
    GameObject ShutOffValve1;

    [SerializeField]
    ZibraLiquidEmitter mainSupplyEmitter;

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
    public HoseDetector hoseDetector1;
    public HoseDetector hoseDetector2;
    public HoseDetector hoseDetector3;
    public HoseDetector hoseDetector4;
    [SerializeField]
    ZibraLiquidEmitter sightTubeEmitter;
    [SerializeField]
    ZibraLiquidVoid sightTubeVoid;


    float sightTubeVoidInitSurfaceDist;
    [SerializeField]
    ZibraLiquidVoid bleederCatchVoid;
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
    public GameObject sightTubeCurrentConnection;

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
    public HoseDetector HoseDetector2;
    public int testCock4MinStr;
    public int testCock4MaxStr;
    public float testCock4Str;
    public GameObject checkValve1;
    public GameObject checkValve2;
    Rigidbody check1Rb;
    Rigidbody check2Rb;

    public ZibraLiquid liquid;
    ZibraLiquidSolverParameters liquidSolverParameters;
    public bool randomizePressure = true;

    public bool isTeachingModeEnabled = false;
    public float initialCheck1Mass;
    public float initialCheck2Mass;
    public float inputForce = 1;
    public float volume;
    public float supplyPsi = 80f;
    float zone1Pressure;
    public float zone2Pressure;
    public float zone3Pressure;
    public float check1SpringForce;
    public float check2SpringForce;
    float zone1PsiChange;
    public float zone2PsiChange;
    public float zone3PsiChange;
    public float zone2to3PsiDiff;
    public float zone3to4PsiDiff;
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
        testCock4Str = Random.Range(testCock4MinStr, testCock4MaxStr);
        testCock3Str = Random.Range(testCock3MinStr, testCock3MaxStr);
        CheckValve1StartingPos = checkValve1.transform.position;
        CheckValve2StartingPos = checkValve2.transform.position;
        liquidSolverParameters = liquid.GetComponent<ZibraLiquidSolverParameters>();
        mainSupplyEmitter.VolumePerSimTime = 0;

    }
    void SupplyRegulate()
    {
        /// <summary>
        /// Regulate supply pressure-------------------------------------------------------
        /// </summary>
        supplyColliderTargetPos.x =
            shutOffValveController.ShutOffValve1.transform.eulerAngles.z / 90;
        supplyCollider.transform.localPosition = initSupplyColliderPos + supplyColliderTargetPos;
        supplyVoidTargetPos.x = shutOffValveController.ShutOffValve1.transform.eulerAngles.z / 90;
        supplyVoid.transform.localPosition = initSupplyVoidPos - supplyVoidTargetPos;

        volume = Mathf.Lerp(
                          supplyPsi,
                          0,
                          ShutOffValve1.transform.eulerAngles.z / 90f
                      );

        mainSupplyEmitter.VolumePerSimTime = Mathf.SmoothStep(
            mainSupplyEmitter.VolumePerSimTime,
            volume,
            1f
        );

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

        zone2to3PsiDiff = zone2Pressure - (zone1Pressure - check1SpringForce) * zone2PsiChange;
        zone3to4PsiDiff = zone3Pressure - (zone2Pressure - check2SpringForce) * zone3PsiChange;
        zone1Pressure = supplyPsi;
        zone2Pressure = (zone1Pressure - check1SpringForce) * zone2PsiChange;
        zone3Pressure = (zone2Pressure - check2SpringForce) * zone3PsiChange;
        if (zone2Pressure <= 0)
        {
            zone2Pressure = 0;
        }
        if (zone3Pressure <= 0)
        {
            zone3Pressure = 0;
        }

    }
    void CheckValveRegulate()
    {
        if (zone1Pressure < (check1SpringForce + zone2Pressure))
        {
            // check1Rb.AddForce(new Vector3(-1, -1, 0) * inputForce, ForceMode.Force);
            check1Rb.AddForce(new Vector3(-1, -1, 0) * inputForce, ForceMode.Force);
        }
        if (zone2Pressure < (check2SpringForce + zone3Pressure))
        {
            // check1Rb.AddForce(new Vector3(-1, -1, 0) * inputForce, ForceMode.Force);
            check2Rb.AddForce(
                new Vector3(-1, -1, 0) * inputForce, ForceMode.Force
                );
        }

        if (shutOffValveController.IsSupplyOn == true)
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
                0.5f
            );
        }
        else
        {
            check1housingForceField.Strength = 0;
            check2housingForceField.Strength = 0;
        }

    }
    void TestCock1Regulate()
    {

        if (testCockController.isTestCock1Open == true
            && TestCockHoseDetect1.isConnected == false
            && shutOffValveController.IsSupplyOn == false
            && shutOffValveController.IsSecondShutOffOpen == false
            )
        {
            //both shutoffs closed
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
    }
    void WaterOperations()
    {


        /// <summary>
        ///Non-Testing conditions operation---------------------------------------------------
        /// </summary>
        if (shutOffValveController.IsSupplyOn == true && shutOffValveController.IsSecondShutOffOpen == true)
        {

            foreach (GameObject testCock in doubleCheckTestKitController.StaticTestCockList)
            {
                testCock.GetComponent<AssignTestCockManipulators>().testCockVoid.enabled = true;
                testCock.GetComponent<AssignTestCockManipulators>().testCockCollider.enabled =
                    false;
            }


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
        else if (shutOffValveController.IsSupplyOn == true && shutOffValveController.IsSecondShutOffOpen == false)
        {

            foreach (GameObject testCock in doubleCheckTestKitController.StaticTestCockList)
            {
                testCock.GetComponent<AssignTestCockManipulators>().testCockVoid.enabled = true;
                testCock.GetComponent<AssignTestCockManipulators>().testCockCollider.enabled =
                    false;
            }
            //while shutoff valve is open regulate ff in check housing according to amount of water being supplied supply


            // Void_Check1.transform.localScale = Vector3.zero;

            // Void_Check2.transform.localScale = Vector3.zero;
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

            foreach (GameObject testCock in doubleCheckTestKitController.StaticTestCockList)
            {
                testCock.GetComponent<AssignTestCockManipulators>().testCockVoid.enabled = false;
                testCock.GetComponent<AssignTestCockManipulators>().testCockCollider.enabled = true;
            }
            if (
             testCockController.isTestCock2Open == true
             && TestCockHoseDetect2.isConnected == false
            )
            {
                check1housingForceField.Strength = 0;
                check2housingForceField.Strength = 0;
                Void_Check1.transform.localScale = Vector3.SmoothDamp(
                Void_Check1.transform.localScale,
                check1VoidMaxSize * TestCockFF1.Strength,
                ref check1VoidRef,
                Check1VoidGrowSpeed
                );
                if (check1Detector.ParticlesInside > 3000)
                {
                    TestCockFF2.Strength = Mathf.SmoothDamp(
                        TestCockFF2.Strength,
                        Mathf.Clamp(check1Detector.ParticlesInside, 0, testCock2MaxStr),
                        ref testCockFF2Ref.x,
                        0.005f
                    );
                }
                if (checkValveStatus.isCheck1Closed)
                {
                    TestCockFF2.Strength = 0;
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
                check1housingForceField.Strength = 0;
                check2housingForceField.Strength = 0;
                Void_Check1.transform.localScale = Vector3.SmoothDamp(
                Void_Check1.transform.localScale,
                check1VoidMaxSize * TestCockFF3.Strength,
                ref check1VoidRef,
                Check1VoidGrowSpeed
                );
                if (check1Detector.ParticlesInside > 3000)
                {
                    TestCockFF3.Strength = Mathf.SmoothDamp(
                        TestCockFF3.Strength,
                        Mathf.Clamp(check1Detector.ParticlesInside, 0, testCock3Str),
                        ref testCockFF3Ref.x,
                        0.005f
                    );
                }

                if (checkValveStatus.isCheck1Closed)
                {
                    TestCockFF3.Strength = 0;
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
                check1housingForceField.Strength = 0;
                check2housingForceField.Strength = 0;
                Void_Check2.transform.localScale = Vector3.SmoothDamp(
                Void_Check2.transform.localScale,
                check2VoidMaxSize * TestCockFF4.Strength,
                ref check2VoidRef,
                Check2VoidGrowSpeed
                );
                if (check2Detector.ParticlesInside > 3000)
                {
                    TestCockFF4.Strength = Mathf.SmoothDamp(
                     TestCockFF4.Strength,
                     Mathf.Clamp(check2Detector.ParticlesInside, 0, testCock4Str),
                     ref testCockFF4Ref.x,
                     tc4ffScaleUpSpeed
                 );

                }
                if (checkValveStatus.isCheck1Closed)
                {
                    TestCockFF4.Strength = 0;
                }

            }
            else
            {
                TestCockFF4.Strength = 0;
            }
        }

        /// <summary>
        ///Testing procedures---------------------------------------------------------------------------------------------
        /// Static conditions and testkit hooked up || not hooked up------------------------------------------------------
        /// </summary>
        else if (shutOffValveController.IsSupplyOn == false && shutOffValveController.IsSecondShutOffOpen == false)
        {


            foreach (GameObject testCock in doubleCheckTestKitController.StaticTestCockList)
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

                    //pressure stop
                    if (checkValveStatus.isCheck2Closed == true)
                    {
                        TestCockFF4.Strength = 0;

                    }

                }
            }
            else
            {

                TestCockFF4.Strength = 0;
            }

        }
    }
    void TeachingWaterOperations()
    {


        /// <summary>
        ///Testing procedures---------------------------------------------------------------------------------------------
        /// Static conditions and testkit hooked up || not hooked up------------------------------------------------------
        /// </summary>

        if (shutOffValveController.IsSupplyOn == false && shutOffValveController.IsSecondShutOffOpen == false)
        {

            foreach (GameObject testCock in doubleCheckTestKitController.StaticTestCockList)
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

            foreach (GameObject testCock in doubleCheckTestKitController.StaticTestCockList)
            {
                testCock.GetComponent<AssignTestCockManipulators>().testCockVoid.enabled = true;
                testCock.GetComponent<AssignTestCockManipulators>().testCockCollider.enabled =
                    false;
            }


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

            foreach (GameObject testCock in doubleCheckTestKitController.StaticTestCockList)
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

            foreach (GameObject testCock in doubleCheckTestKitController.StaticTestCockList)
            {
                testCock.GetComponent<AssignTestCockManipulators>().testCockVoid.enabled = true;
                testCock.GetComponent<AssignTestCockManipulators>().testCockCollider.enabled =
                    false;
            }
            //while shutoff valve is open regulate ff in check housing according to amount of water being supplied supply


            // Void_Check1.transform.localScale = Vector3.zero;

            // Void_Check2.transform.localScale = Vector3.zero;
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
        if (isTeachingModeEnabled)
        {
            TeachingWaterOperations();
        }
        else
        {
            WaterOperations();
        }
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
        /// Regulate pressure zones
        /// </summary>
        //PressureZoneRegulate();

    }
    void FixedUpdate()
    {
    }

}
