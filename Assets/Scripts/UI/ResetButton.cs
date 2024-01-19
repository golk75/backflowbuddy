using System.Collections;
using System.Collections.Generic;
using com.zibra.liquid.Manipulators;
using com.zibra.liquid.Solver;
// using GoogleMobileAds.Sample;
using Unity.Mathematics;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.UIElements;

public class ResetButton : MonoBehaviour
{

    Button resetButton;
    public ZibraLiquid liquid;
    public GameObject Check1;
    public GameObject Check2;
    public GameObject ShutOff1;
    public GameObject Tc1;
    public TestCockController testCockController;
    public WaterController waterController;
    public SightTubeController sightTubeController;
    public TestKitManager testKitManager;
    public PressureZoneHUDController pressureZoneHUDController;
    public GameObject sightTube;
    public PlayerController playerController;
    public ShutOffValveController shutOffValveController;
    public DoubleCheckTestKitController doubleCheckTestKitController;
    public ZibraLiquidForceField check1HousingFF;
    public ZibraLiquidForceField check2HousingFF;
    public GameObject m_adsManager;
    // MyBannerViewController m_bannerViewController;
    // MyInterstitialAdController m_interstitialAdController;
    [SerializeField]
    List<ResetableObject> objectsToReset;
    ResetableObject resetableObject;



    [SerializeField]
    List<GameObject> testCockValveList;



    [System.Serializable]
    public class ResetableObject
    {


        public GameObject alteredObject;
        public Quaternion initRotation;
        public Vector3 initScale;
        public Vector3 initPos;



    }
    private void Awake()
    {
        for (int i = 0; i < objectsToReset.Count; i++)
        {
            SetResetables(objectsToReset[i]);
        }

        // m_bannerViewController = m_adsManager.GetComponent<MyBannerViewController>();
        // m_interstitialAdController = m_adsManager.GetComponent<MyInterstitialAdController>();
        // m_bannerViewController.LoadAd();
        // m_interstitialAdController.LoadAd();

    }
    private void SetResetables(ResetableObject resetableObject)
    {
        this.resetableObject = resetableObject;
        resetableObject.initRotation = resetableObject.alteredObject.transform.rotation;
        resetableObject.initScale = resetableObject.alteredObject.transform.localScale;
        resetableObject.initPos = resetableObject.alteredObject.transform.localPosition;

    }
    private void ResetTransforms()
    {
        foreach (ResetableObject item in objectsToReset)
        {
            item.alteredObject.transform.rotation = item.initRotation;
            item.alteredObject.transform.localScale = item.initScale;
            item.alteredObject.transform.localPosition = item.initPos;
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        var root = GetComponent<UIDocument>();
        resetButton = root.rootVisualElement.Q<Button>("ResetButton");
        resetButton.clicked += ResetDevice;



    }
    //SO OPEN -->
    // pos = Vector3(-0.0244999994,-0.0284000002,-0.0217000004)
    // Euler rot = Vector3(90,5.00895567e-06,0)
    // Quaternion rot = Quaternion(0.707106829,3.09086197e-08,3.09086197e-08,0.707106829)

    //SO CLOSED -->
    // pos = Vector3(-0.0244999994,-0.0284000002,-0.0217000004)
    // Euler rot = Vector3(90,270,0)
    // Quaternion rot = Quaternion(0.5,-0.5,0.5,0.5)
    public void ResetDevice()
    {

        // m_interstitialAdController.ShowAd();


        // liquid.enabled = false;
        liquid.ReleaseSimulation();
        liquid.enabled = false;

        shutOffValveController.mainSupplyEmitter.enabled = false;



        Check1.transform.localPosition = new Vector3(-0.101f, 0, -0.08f);
        Check2.transform.localPosition = new Vector3(-0.201f, -2.25f, -0.17f);

        ResetTransforms();

        foreach (GameObject testCock in doubleCheckTestKitController.StaticTestCockList)
        {
            testCock.GetComponent<AssignTestCockManipulators>().testCockVoid.enabled = false;
            testCock.GetComponent<AssignTestCockManipulators>().testCockCollider.enabled = true;

        }
        foreach (var hose in doubleCheckTestKitController.AttachedHoseList)
        {

            Actions.onComponentGrab?.Invoke(hose, hose.GetComponent<OperableComponentDescription>());
            Actions.onComponentDrop?.Invoke(hose, hose.GetComponent<OperableComponentDescription>());

        }
        Actions.onSightTubeGrab?.Invoke(sightTube);
        Actions.onSightTubeDrop?.Invoke(sightTube);
        doubleCheckTestKitController.AttachedTestCockList.Clear();
        doubleCheckTestKitController.AttachedHoseList.Clear();



        //reset ui pressure values to 0
        pressureZoneHUDController.check1SpringPressure = 0;
        pressureZoneHUDController.check2SpringPressure = 0;
        waterController.supplyPsi = 0;
        pressureZoneHUDController.m_SupplyPressureTextField.value = "0";

        //ads control


    }

}
