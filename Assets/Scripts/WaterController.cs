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
    ZibraLiquidForceField TestCockFF1;

    [SerializeField]
    ZibraLiquidForceField TestCockFF2;

    [SerializeField]
    ZibraLiquidForceField TestCockFF3;

    [SerializeField]
    ZibraLiquidForceField TestCockFF4;

    [SerializeField]
    ZibraLiquidForceField check1housing;

    [SerializeField]
    ZibraLiquidForceField check2housing;

    [SerializeField]
    ZibraLiquidVoid Void_Check1;

    [SerializeField]
    ZibraLiquidVoid Void_Check2;

    Vector3 Zone2VoidMaxSize = new Vector3(0.04f, 0.02f, 0.02f);

    Vector3 Zone3VoidMaxSize = new Vector3(0.04f, 0.02f, 0.02f);
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
    Vector3 supplyVoidRef = Vector3.zero;

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

        if (testCockController.isTestCock3Open)
        {
            TestCockFF3.Strength = Mathf.SmoothStep(0, 1, (check1Detector.ParticlesInside * 0.01f));
            check1housing.Strength = TestCockFF3.Strength;
            check2housing.Strength = TestCockFF3.Strength;
        }
        else
        {
            TestCockFF3.Strength = 0;
        }
        //may need to figure out what other factors should influence this  void's size
        if (shutOffValveController.IsSupplyOn == false)
        {
            Void_Check1.transform.localScale = Vector3.Lerp(
                Vector3.zero,
                Zone2VoidMaxSize,
                TestCockFF3.Strength
            );
            Void_Check2.transform.localScale = Vector3.Lerp(
                Vector3.zero,
                Zone3VoidMaxSize,
                TestCockFF3.Strength
            );
        }
    }
}
