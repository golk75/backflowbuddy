using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using com.zibra.liquid.Manipulators;

public class WaterController : MonoBehaviour
{
    [SerializeField]
    GameObject testCockManager;

    [SerializeField]
    GameObject shutOffValveManager;

    TestCockController testCockController;
    ShutOffValveController shutOffValveController;

    [SerializeField]
    public GameObject ShutOffValve1;

    [SerializeField]
    GameObject ShutOffValve2;

    [SerializeField]
    ZibraLiquidCollider supplyCollider;

    [SerializeField]
    ZibraLiquidVoid supplyVoid;

    [SerializeField]
    ZibraLiquidDetector TestCockDetector1;

    [SerializeField]
    ZibraLiquidDetector TestCockDetector2;

    [SerializeField]
    ZibraLiquidDetector TestCockDetector3;

    [SerializeField]
    ZibraLiquidDetector TestCockDetector4;

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

    [SerializeField]
    ZibraLiquidVoid Void_Check1;

    [SerializeField]
    ZibraLiquidVoid Void_Check2;
    public float Void_check2ScaleUpSpeed;
    public float Void_check2ScaleDownSpeed;

    //Vector3 Zone2VoidMaxSize = new Vector3(0.045f, 0.035f, 0.02f);
    Vector3 check1VoidMaxSize = new Vector3(0.045f, 0.0354f, 0.0201f);

    Vector3 check2VoidMaxSize = new Vector3(0.045f, 0.0354f, 0.0201f);
    private ZibraLiquidVoid _zoneVoid;
    public ZibraLiquidVoid ZoneVoid
    {
        get { return _zoneVoid; }
        private set { value = _zoneVoid; }
    }
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
    Vector3 testCockFF1Ref = Vector3.zero;
    Vector3 testCockFF2Ref = Vector3.zero;
    Vector3 testCockFF3Ref = Vector3.zero;
    Vector3 testCockFF4Ref = Vector3.zero;
    public float testCock1MaxStr;
    public float testCock2MaxStr;
    public float testCock3MaxStr;
    public float testCock4MaxStr;

    // Start is called before the first frame update
    void Start()
    {
        //TestCockList = new List<GameObject>();
        testCockController = testCockManager.GetComponent<TestCockController>();
        shutOffValveController = shutOffValveManager.GetComponent<ShutOffValveController>();
        initSupplyVoidPos = supplyVoid.transform.localPosition;
        initSupplyColliderPos = supplyCollider.transform.localPosition;
    }

    // Update is called once per frame
    void Update()
    {
        supplyColliderTargetPos.x =
            shutOffValveController.ShutOffValve1.transform.eulerAngles.z / 90;
        supplyCollider.transform.localPosition = initSupplyColliderPos + supplyColliderTargetPos;
        supplyVoidTargetPos.x = shutOffValveController.ShutOffValve1.transform.eulerAngles.z / 90;
        supplyVoid.transform.localPosition = initSupplyVoidPos - supplyVoidTargetPos;
        /// <summary>
        /// Test cock force fields
        /// </summary>

        //check if device is primed
        if (testCockController.isTestCock1Open)
        {
            if (TestCockDetector2.ParticlesInside > 5500)
            {
                TestCockFF1.Strength = Mathf.SmoothDamp(
                    TestCockFF1.Strength,
                    Mathf.Clamp(BodyDetectorZone1.ParticlesInside, 0, testCock1MaxStr),
                    ref testCockFF1Ref.x,
                    0.005f
                );
            }
            else
            {
                TestCockFF1.Strength = Mathf.SmoothDamp(
                    TestCockFF1.Strength,
                    0,
                    ref testCockFF1Ref.x,
                    1f
                );
            }
        }
        else
        {
            TestCockFF1.Strength = 0;
        }
        if (testCockController.isTestCock2Open)
        {
            if (TestCockDetector2.ParticlesInside > 5500)
            {
                TestCockFF2.Strength = Mathf.SmoothDamp(
                    TestCockFF2.Strength,
                    Mathf.Clamp(BodyDetectorZone1.ParticlesInside, 0, testCock2MaxStr),
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
                    2f
                );
            }
        }
        else
        {
            TestCockFF2.Strength = 0;
        }
        if (testCockController.isTestCock3Open)
        {
            if (check1Detector.ParticlesInside > 3000)
            {
                TestCockFF3.Strength = Mathf.SmoothDamp(
                    TestCockFF3.Strength,
                    Mathf.Clamp(check1Detector.ParticlesInside, 0, testCock3MaxStr),
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
        if (testCockController.isTestCock4Open)
        {
            if (check2Detector.ParticlesInside > 3000)
            {
                TestCockFF4.Strength = Mathf.SmoothDamp(
                    TestCockFF4.Strength,
                    Mathf.Clamp(check2Detector.ParticlesInside, 0, testCock4MaxStr),
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

        //Remove/release pressure from static device state, upon opening testcock.

        if (shutOffValveController.IsSupplyOn == false)
        {
            foreach (GameObject testCock in testCockController.TestCockList)
            {
                testCock.GetComponent<AssignTestCockManipulators>().testCockVoid.enabled = false;
                testCock.GetComponent<AssignTestCockManipulators>().testCockCollider.enabled = true;
            }
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
            if (
                testCockController.isTestCock1Open == true
                || testCockController.isTestCock2Open == true
                || testCockController.isTestCock3Open == true
                || testCockController.isTestCock4Open == true
            )
            {
                {
                    check1housingForceField.enabled = false;
                    check2housingForceField.enabled = false;
                }
            }
            else
            {
                check1housingForceField.enabled = true;
                check2housingForceField.enabled = true;
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
            check1housingForceField.enabled = true;
            check2housingForceField.enabled = true;
            Void_Check1.transform.localScale = Vector3.zero;

            Void_Check2.transform.localScale = Vector3.zero;
        }
    }
}
