using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using com.zibra.liquid.Manipulators;

public class WaterController : MonoBehaviour
{
    [SerializeField]
    GameObject testCockManager;

    TestCockController testCockController;

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
    ZibraLiquidVoid Zone1Void;

    [SerializeField]
    ZibraLiquidVoid Zone2Void;
    Vector3 Zone2VoidMaxSize = new Vector3(0.04f, 0.02f, 0.02f);

    [SerializeField]
    ZibraLiquidVoid Zone3Void;
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

    // Start is called before the first frame update
    void Start()
    {
        testCockController = testCockManager.GetComponent<TestCockController>();
    }

    // Update is called once per frame
    void Update()
    {
        /// <summary>
        /// Test cock force fields
        /// </summary>

        if (testCockController.isTestCock3Open)
        {
            TestCockFF3.Strength = Mathf.SmoothStep(0, 1, (check1Detector.ParticlesInside * 0.01f));
        }
        else
        {
            TestCockFF3.Strength = 0;
        }
        //may need to figure out what other factors should influence this  void's size
        Zone2Void.transform.localScale = Vector3.Lerp(
            Vector3.zero,
            Zone2VoidMaxSize,
            TestCockFF3.Strength
        );
        Zone3Void.transform.localScale = Vector3.Lerp(
            Vector3.zero,
            Zone3VoidMaxSize,
            TestCockFF3.Strength
        );
        check1housing.Strength = TestCockFF3.Strength;
    }
}
