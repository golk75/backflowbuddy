using System;
using System.Collections;
using System.Collections.Generic;
using com.zibra.liquid.DataStructures;
using com.zibra.liquid.Manipulators;

using UnityEngine;


public class RelaxWater : MonoBehaviour
{
    [SerializeField]
    ZibraLiquidEmitter supplyEmitter;

    [SerializeField]
    ZibraLiquidForceField checkValve1ForceField;

    [SerializeField]
    ZibraLiquidForceField checkValve2ForceField;

    [SerializeField]
    ZibraLiquidSolverParameters liquidSolverParameters;

    [SerializeField]
    ZibraLiquidVoid supplyVoid;

    float initSupplyVolume;

    [SerializeField]
    GameObject TestCockManager;
    TestCockController testCockController;

    [SerializeField]
    GameObject TestCock1;

    [SerializeField]
    GameObject TestCock2;

    [SerializeField]
    GameObject TestCock3;

    [SerializeField]
    GameObject TestCock4;

    [SerializeField]
    ZibraLiquidForceField testCock1FF;

    [SerializeField]
    ZibraLiquidForceField testCock2FF;

    [SerializeField]
    ZibraLiquidForceField testCock3FF;

    [SerializeField]
    ZibraLiquidForceField testCock4FF;

    [SerializeField]
    ZibraLiquidDetector testCock1Detector;

    [SerializeField]
    ZibraLiquidDetector testCock2Detector;

    [SerializeField]
    ZibraLiquidDetector testCock3Detector;

    [SerializeField]
    ZibraLiquidDetector testCock4Detector;

    [SerializeField]
    ZibraLiquidDetector zone1Detector;

    [SerializeField]
    ZibraLiquidDetector zone2Detector;

    [SerializeField]
    ZibraLiquidDetector zone3Detector;

    [SerializeField]
    ZibraLiquidDetector check1Detector;

    [SerializeField]
    ZibraLiquidDetector check2Detector;

    [SerializeField]
    ShutOffValveController shutOffValveController;

    [SerializeField]
    ZibraLiquidCollider supplyCollider;

    [SerializeField]
    ZibraLiquidSolverParameters waterMaxVelocity;

    [SerializeField]
    ConfigurableJoint check1Spring;

    [SerializeField]
    ConfigurableJoint check2Spring;

    float initialWaterMaxVelocity;
    Vector3 supplyColliderClosedPos;
    Vector3 initSupplyColliderPos;
    Vector3 supplyColliderTargetPos = new Vector3(-15f, 0, 0);
    Vector3 supplyVoidTargetPos = new Vector3(-9.5f, 0, 0);
    Vector3 initSupplyVoidPos;

    Vector3 initSupplyVoidScale;
    Vector3 currentSupplyVoidScale;
    Vector3 targetSupplyVoidScale;
    Vector3 supplyVoidRef = Vector3.zero;
    float floatRef = 0;

    public GameObject playerManager;

    private PlayerController playerController;

    public float supplyVolume;

    public float currentVelocity = 0;

    [SerializeField]
    private List<GameObject> TestCockList;

    [SerializeField]
    private GameObject _operatingTestCock;

    public GameObject OperatingTestCock
    {
        get { return _operatingTestCock; }
        private set { value = _operatingTestCock; }
    }
    Vector3 ZoneVoidMaxSize = new Vector3(0.04f, 0.02f, 0.02f);
    public bool isZone1Primed;
    public bool isZone2Primed;
    public bool isZone3Primed;

    [Range(1000, 5000)]
    public float check1ClosingThreshold;

    /// <summary>
    ///
    /// Need to find a way to constantly monitor values of x's, and adjust y's accordingly
    ///
    /// --find x's and y's
    /// Y: These should be that I want to be reactive to persistant change.
    /// things like: force fields, voids, colliders
    ///
    /// X: This might only be the water..Or supplyVolume.
    /// </summary>
    private void checkRelax()
    {
        //supplyVolume = shutOffValveController.mainSupplyEmitter.VolumePerSimTime;
        //_operatingTestCock = testCockController.OperatingTestCock;

        /// <summary>
        /// //close supply end with collider if shutoff is closed, to keep current volume of water at time of shutoff (protect water from supply void)
        /// </summary>
        supplyColliderTargetPos.x =
            shutOffValveController.ShutOffValve1.transform.eulerAngles.z / 90;
        supplyCollider.transform.localPosition = initSupplyColliderPos + supplyColliderTargetPos;
        supplyVoidTargetPos.x = shutOffValveController.ShutOffValve1.transform.eulerAngles.z / 90;
        supplyVoid.transform.localPosition = initSupplyVoidPos - supplyVoidTargetPos;
    }

    void Awake()
    {
        initSupplyVoidPos = supplyVoid.transform.localPosition;
        initSupplyColliderPos = supplyCollider.transform.localPosition;
    }

    // Start is called before the first frame update
    void Start()
    {
        playerController = playerManager.GetComponent<PlayerController>();
        testCockController = TestCockManager.GetComponent<TestCockController>();
        initSupplyVolume = shutOffValveController.supplyVolume;
        initSupplyVoidScale = supplyVoid.transform.localScale;
    }

    // Update is called once per frame
    void Update()
    {
        checkRelax();
    }
}
