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
    ZibraLiquidForceField check1housingForceField;

    [SerializeField]
    ZibraLiquidForceField check2housingForceField;

    [SerializeField]
    ZibraLiquidVoid Void_Check1;

    [SerializeField]
    ZibraLiquidVoid Void_Check2;

    //Vector3 Zone2VoidMaxSize = new Vector3(0.045f, 0.035f, 0.02f);
    Vector3 check1VoidMaxSize = new Vector3(0.0401f, 0.0354f, 0.0201f);

    Vector3 check2VoidMaxSize = new Vector3(0.0401f, 0.0354f, 0.0201f);
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

    // Start is called before the first frame update
    void Start()
    {
        testCockController = testCockManager.GetComponent<TestCockController>();
        shutOffValveController = shutOffValveManager.GetComponent<ShutOffValveController>();
        initSupplyVoidPos = supplyVoid.transform.localPosition;
        initSupplyColliderPos = supplyCollider.transform.localPosition;
    }

    // Update is called once per frame
    void Update()
    {
        /// <summary>
        /// Test cock force fields
        /// </summary>

        supplyColliderTargetPos.x =
            shutOffValveController.ShutOffValve1.transform.eulerAngles.z / 90;
        supplyCollider.transform.localPosition = initSupplyColliderPos + supplyColliderTargetPos;
        supplyVoidTargetPos.x = shutOffValveController.ShutOffValve1.transform.eulerAngles.z / 90;
        supplyVoid.transform.localPosition = initSupplyVoidPos - supplyVoidTargetPos;

        //test cock #3 pressure regulation
        if (testCockController.isTestCock3Open)
        {
            //TestCockFF3.Strength = Mathf.SmoothStep(0, 1, (check1Detector.ParticlesInside * 0.1f));

            TestCockFF3.Strength = Mathf.SmoothDamp(
                TestCockFF3.Strength,
                Mathf.Clamp(check1Detector.ParticlesInside, 0, 1),
                ref testCockFF3Ref.x,
                0.5f
            );

            check1housingForceField.Strength = TestCockFF3.Strength;
        }
        else
        {
            TestCockFF3.Strength = 0;
            check1housingForceField.Strength = 1;
        }

        //test cock #4 pressure regulation
        if (testCockController.isTestCock4Open)
        {
            //TestCockFF4.Strength = Mathf.SmoothStep(0, 1, (check2Detector.ParticlesInside * 0.1f));

            TestCockFF3.Strength = Mathf.SmoothDamp(
                TestCockFF3.Strength,
                Mathf.Clamp(check1Detector.ParticlesInside, 0, 1),
                ref testCockFF4Ref.x,
                0.5f
            );
            check2housingForceField.Strength = TestCockFF4.Strength;
        }
        else
        {
            TestCockFF4.Strength = 0;
            check2housingForceField.Strength = 1;
        }

        //Remove/release pressure from static device state, upon opening testcock.

        if (shutOffValveController.IsSupplyOn == false)
        {
            Void_Check1.transform.localScale = Vector3.MoveTowards(
                Void_Check1.transform.localScale,
                check1VoidMaxSize * TestCockFF3.Strength,
                0.001f
            );
            /*
            Void_Check1.transform.localScale = Vector3.Lerp(
                Vector3.zero,
                check1VoidMaxSize,
                TestCockFF3.Strength
            );
            */

            Void_Check2.transform.localScale = Vector3.Lerp(
                Vector3.zero,
                check2VoidMaxSize,
                TestCockFF4.Strength * 0.01f
            );
            /*
            Void_Check1.transform.localScale = Vector3.SmoothDamp(
                Void_Check1.transform.localScale,
                check1VoidMaxSize * TestCockFF3.Strength,
                ref check1VoidRef,
                5f
            );

            Void_Check2.transform.localScale = Vector3.SmoothDamp(
                Void_Check2.transform.localScale,
                check2VoidMaxSize * TestCockFF4.Strength,
                ref check2VoidRef,
                0.01f
            );
            */
        }
        //checkvalve housing force fields
    }
}
