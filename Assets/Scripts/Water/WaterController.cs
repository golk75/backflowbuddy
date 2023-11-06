
using com.zibra.liquid.DataStructures;
using com.zibra.liquid.Manipulators;
using com.zibra.liquid.Solver;
using Unity.VisualScripting;
using UnityEngine;

public class WaterController : MonoBehaviour
{
    public CheckValveCollision checkValveCollision;

    [SerializeField]
    GameObject testCockManager;

    [SerializeField]
    GameObject shutOffValveManager;

    public CheckValveStatus checkValveStatus;

    TestCockController testCockController;
    ShutOffValveController shutOffValveController;
    public TestKitController testKitController;

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

    [SerializeField]


    public float Void_check2ScaleUpSpeed;
    public float Void_check2ScaleDownSpeed;

    //Vector3 Zone2VoidMaxSize = new Vector3(0.045f, 0.035f, 0.02f);
    Vector3 check1VoidMaxSize = new Vector3(0.045f, 0.0354f, 0.0201f);

    Vector3 check2VoidMaxSize = new Vector3(0.045f, 0.0354f, 0.0201f);

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

    public ZibraLiquid liquid;
    ZibraLiquidSolverParameters liquidSolverParameters;

    [SerializeField]
    bool isAttachedToGauge;

    // Start is called before the first frame update
    void Start()
    {
        testCockController = testCockManager.GetComponent<TestCockController>();
        shutOffValveController = shutOffValveManager.GetComponent<ShutOffValveController>();
        initSupplyVoidPos = supplyVoid.transform.localPosition;
        initSupplyColliderPos = supplyCollider.transform.localPosition;
        check1Rb = checkValve1.GetComponent<Rigidbody>();
        check2Rb = checkValve2.GetComponent<Rigidbody>();
    }

    /// <summary>
    /// This function is called when the object becomes enabled and active.
    /// </summary>
    private void OnEnable()
    {
        //Actions.onCheckClosed += DetectCheckClosure;
        //Actions.onCheckOpened += DetectCheckOpening;
        Actions.onHoseBibConnect += DetectHoseAttachment;
        Actions.onHoseDetach += DetectHoseDetachment;
        testCock4Str = Random.Range(testCock4MinStr, testCock4MaxStr);
        testCock3Str = Random.Range(testCock3MinStr, testCock3MaxStr);

        liquidSolverParameters = liquid.GetComponent<ZibraLiquidSolverParameters>();
    }

    /// <summary>
    /// This function is called when the behaviour becomes disabled or inactive.
    /// </summary>
    private void OnDisable()
    {
        //Actions.onCheckClosed -= DetectCheckClosure;
        //Actions.onCheckOpened -= DetectCheckOpening;
        Actions.onHoseAttach -= DetectHoseAttachment;
        Actions.onHoseDetach -= DetectHoseDetachment;
    }



    private void DetectHoseAttachment(
        GameObject gameObject,
        OperableComponentDescription description
    )
    {
        isAttachedToGauge = true;
        testCock4Str = Random.Range(testCock4MinStr, testCock4MaxStr);
        testCock3Str = Random.Range(testCock3MinStr, testCock3MaxStr);

    }

    private void DetectHoseDetachment(
        GameObject gameObject,
        OperableComponentDescription description
    )
    {
        isAttachedToGauge = false;
    }

    // Update is called once per frame
    void Update()
    {




        /// <summary>
        /// Regulate supply pressure
        /// </summary>
        supplyColliderTargetPos.x =
            shutOffValveController.ShutOffValve1.transform.eulerAngles.z / 90;
        supplyCollider.transform.localPosition = initSupplyColliderPos + supplyColliderTargetPos;
        supplyVoidTargetPos.x = shutOffValveController.ShutOffValve1.transform.eulerAngles.z / 90;
        supplyVoid.transform.localPosition = initSupplyVoidPos - supplyVoidTargetPos;

        /// <summary>
        /// Test cock force fields
        /// </summary>


        //test cock #1 pressure regulation
        if (testCockController.isTestCock1Open && TestCockHoseDetect1.isConnected == false)
        {
            TestCock1Emitter.enabled = true;

        }
        else
        {
            TestCock1Emitter.enabled = false;

        }

        //test cock #2 pressure regulation
        if (
            testCockController.isTestCock2Open
            && checkValveStatus.isCheck1Closed == false
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
            else
            {
                TestCockFF2.Strength = Mathf.SmoothDamp(
                    TestCockFF2.Strength,
                    0,
                    ref testCockFF2Ref.x,
                    1f
                );
            }
        }
        else
        {
            TestCockFF2.Strength = 0;
        }
        //test cock #3 pressure regulation
        if (
            testCockController.isTestCock3Open
            && checkValveStatus.isCheck1Closed == false
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
            else
            {
                TestCockFF3.Strength = Mathf.SmoothDamp(
                    TestCockFF3.Strength,
                    0,
                    ref testCockFF3Ref.x,
                    3f
                );
            }
        }
        else
        {
            TestCockFF3.Strength = 0;
        }
        //test cock #4 pressure regulation
        if (
            testCockController.isTestCock4Open
            && checkValveStatus.isCheck2Closed == false
            && TestCockHoseDetect4.isConnected == false
        )
        {

            if (check2Detector.ParticlesInside > 3000)
            {
                TestCockFF4.Strength = Mathf.SmoothDamp(
                 TestCockFF4.Strength,
                 Mathf.Clamp(check2Detector.ParticlesInside, 0, testCock4Str),
                 ref testCockFF4Ref.x,
                 Void_check2ScaleUpSpeed
             );

            }
            else
            {

                TestCockFF4.Strength = Mathf.SmoothDamp(
                    TestCockFF4.Strength,
                    0,
                    ref testCockFF4Ref.x,
                    Void_check2ScaleDownSpeed
                );
            }
        }
        else
        {
            TestCockFF4.Strength = 0;
        }

        if (shutOffValveController.IsSupplyOn == false)
        {
            foreach (GameObject testCock in testCockController.TestCockList)
            {
                testCock.GetComponent<AssignTestCockManipulators>().testCockVoid.enabled = false;
                testCock.GetComponent<AssignTestCockManipulators>().testCockCollider.enabled = true;
            }
            //     if (liquid.UseFixedTimestep == true)
            //     {
            //         Void_Check1.transform.localScale = Vector3.SmoothDamp(
            //       Void_Check1.transform.localScale,
            //       check1VoidMaxSize * TestCockFF3.Strength,
            //       ref check1VoidRef,
            //       6f
            //   );
            //     }
            Void_Check1.transform.localScale = Vector3.SmoothDamp(
                Void_Check1.transform.localScale,
                check1VoidMaxSize * TestCockFF3.Strength,
                ref check1VoidRef,
                4f
            );

            Void_Check2.transform.localScale = Vector3.SmoothDamp(
                Void_Check2.transform.localScale,
                check2VoidMaxSize * TestCockFF4.Strength,
                ref check2VoidRef,
                6f
            );

            //Regulate check housing force fields while shut off is closed (or testing is in progress)

            if (checkValveStatus.isCheck1Closed || checkValveStatus.isCheck2Closed)
            {

                check1housingForceField.Strength = 0;
                check2housingForceField.Strength = 0;



            }

            else
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

            }



        }
        else if (shutOffValveController.IsSupplyOn == true)
        {
            foreach (GameObject testCock in testCockController.TestCockList)
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


        }
        // else if ()

    }

}
