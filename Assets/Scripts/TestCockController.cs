using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using com.zibra.liquid.Manipulators;

public class TestCockController : MonoBehaviour
{
    private PlayerController playerController;

    [SerializeField]
    GameObject PlayerManager;

    public GameObject WaterManager;
    RelaxWater relaxWater;

    [SerializeField]
    ZibraLiquidVoid TestCockVoid1;

    [SerializeField]
    ZibraLiquidVoid TestCockVoid2;

    [SerializeField]
    ZibraLiquidVoid TestCockVoid3;

    [SerializeField]
    ZibraLiquidVoid TestCockVoid4;

    [SerializeField]
    ZibraLiquidCollider TestCockCollider1;

    [SerializeField]
    ZibraLiquidCollider TestCockCollider2;

    [SerializeField]
    ZibraLiquidCollider TestCockCollider3;

    [SerializeField]
    ZibraLiquidCollider TestCockCollider4;

    ZibraLiquidVoid _operableTestCockVoid;
    ZibraLiquidCollider _operableTestCockCollider;

    GameObject _operatingTestCock;
    public GameObject OperatingTestCock
    {
        get { return _operatingTestCock; }
        private set { value = _operatingTestCock; }
    }

    [SerializeField]
    public GameObject TestCock1;

    [SerializeField]
    public GameObject TestCock2;

    [SerializeField]
    public GameObject TestCock3;

    [SerializeField]
    public GameObject TestCock4;

    [SerializeField]
    ZibraLiquidEmitter TestCockEmitter1;

    [SerializeField]
    ZibraLiquidEmitter TestCockEmitter2;

    [SerializeField]
    ZibraLiquidEmitter TestCockEmitter3;

    [SerializeField]
    ZibraLiquidEmitter TestCockEmitter4;

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

    OperableComponentDescription testCockComponentDescription;

    public float testCockValveScaleFactor;

    //Vector3 testCockClosedScale = new Vector3(0.00886263326f,0.0028f,0.000265074486f);
    Vector3 testCockClosedScale;

    //Open Vector3(0.010200086,0.00762913516,0.000314917503)
    public Vector3 testCockOpenScale = new Vector3(0.010200086f, 0.00762913516f, 0.000314917503f);

    Vector3 _operableTestCockVoidScale;
    Vector3 _operableTestCockColliderScale;

    public bool isCurrentTestCockOpen { get; private set; } = false;
    public bool isTestCock1Open { get; private set; } = false;
    public bool isTestCock2Open { get; private set; } = false;
    public bool isTestCock3Open { get; private set; } = false;
    public bool isTestCock4Open { get; private set; } = false;

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

    [SerializeField]
    public List<GameObject> TestCockList;

    // Start is called before the first frame update
    void Start()
    {
        playerController = PlayerManager.GetComponent<PlayerController>();
        relaxWater = WaterManager.GetComponent<RelaxWater>();
        testCockClosedScale = TestCockVoid1.transform.localScale;
        // TestCockList = new List<GameObject>();
    }

    private void TestCockValveOperationCheck()
    {
        if (playerController.OperableObject != null)
        {
            if (
                playerController.OperableObject.TryGetComponent<OperableComponentDescription>(
                    out OperableComponentDescription component
                )
            )
                if (
                    playerController.operableComponentDescription.partsType
                    == OperableComponentDescription.PartsType.TestCock
                )
                {
                    testCockComponentDescription =
                        playerController.OperableObject.GetComponent<OperableComponentDescription>();

                    switch (testCockComponentDescription.componentId)
                    {
                        case OperableComponentDescription.ComponentId.TestCock1:
                            _operatingTestCock = TestCock1;
                            _operableTestCockVoid = TestCockVoid1;
                            _operableTestCockCollider = TestCockCollider1;
                            _operatingTestCock.transform.eulerAngles =
                                playerController.OperableObjectRotation;

                            break;
                        case OperableComponentDescription.ComponentId.TestCock2:
                            _operatingTestCock = TestCock2;
                            _operableTestCockVoid = TestCockVoid2;
                            _operableTestCockCollider = TestCockCollider2;
                            _operatingTestCock.transform.eulerAngles =
                                playerController.OperableObjectRotation;
                            break;
                        case OperableComponentDescription.ComponentId.TestCock3:
                            _operatingTestCock = TestCock3;
                            _operableTestCockVoid = TestCockVoid3;
                            _operableTestCockCollider = TestCockCollider3;
                            TestCockDetector = TestCockDetector3;
                            _checkZoneDetector = check1Detector;
                            _operatingTestCock.transform.eulerAngles =
                                playerController.OperableObjectRotation;
                            break;
                        case OperableComponentDescription.ComponentId.TestCock4:
                            _operatingTestCock = TestCock4;
                            _operableTestCockVoid = TestCockVoid4;
                            _operableTestCockCollider = TestCockCollider4;
                            _operatingTestCock.transform.eulerAngles =
                                playerController.OperableObjectRotation;
                            break;
                    }
                    //assign the associated test cock valve/collider to currently operating test cock;

                    _operableTestCockColliderScale = _operableTestCockVoidScale =
                        _operableTestCockVoid.transform.localScale;

                    _operableTestCockVoidScale.y = Mathf.Lerp(
                        testCockClosedScale.y,
                        testCockOpenScale.y,
                        _operatingTestCock.transform.eulerAngles.z / 90 * testCockValveScaleFactor
                    );

                    _operableTestCockCollider.transform.localScale = _operableTestCockVoid
                        .transform
                        .localScale = _operableTestCockVoidScale;
                    //cache test cock status
                    if (TestCock1.transform.eulerAngles.z > 0)
                    {
                        isTestCock1Open = true;
                        Actions.onTestCock1Opened?.Invoke();
                    }
                    else
                    {
                        isTestCock1Open = false;
                        Actions.onTestCock1Closed?.Invoke();
                    }

                    if (TestCock2.transform.eulerAngles.z > 0)
                    {
                        isTestCock2Open = true;
                        Actions.onTestCock2Opened?.Invoke();
                    }
                    else
                    {
                        isTestCock2Open = false;
                        Actions.onTestCock2Closed?.Invoke();
                    }

                    if (TestCock3.transform.eulerAngles.z > 0)
                    {
                        isTestCock3Open = true;
                        Actions.onTestCock3Opened?.Invoke();
                    }
                    else
                    {
                        isTestCock3Open = false;
                        Actions.onTestCock3Closed?.Invoke();
                    }

                    if (TestCock4.transform.eulerAngles.z > 0)
                    {
                        isTestCock4Open = true;
                        Actions.onTestCock4Opened?.Invoke();
                    }
                    else
                    {
                        isTestCock4Open = false;
                        Actions.onTestCock4Closed?.Invoke();
                    }
                }
        }
    }

    // Update is called once per frame
    void Update()
    {
        TestCockValveOperationCheck();
    }
}
