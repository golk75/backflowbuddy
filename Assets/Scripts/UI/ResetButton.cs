using System.Collections;
using System.Collections.Generic;
using com.zibra.liquid.Manipulators;
using com.zibra.liquid.Solver;
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
    public GameObject sightTube;
    OperableComponentDescription ShutOff1OperableDescription;
    OperableComponentDescription Tc1OperableDescription;
    public PlayerController playerController;
    public ShutOffValveController shutOffValveController;
    public DoubleCheckTestKitController doubleCheckTestKitController;

    public ZibraLiquidForceField check1HousingFF;
    public ZibraLiquidForceField check2HousingFF;
    [SerializeField]
    List<ResetableObject> objectsToReset;
    ResetableObject resetableObject;
    ZibraLiquid resetVoid;


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

        ShutOff1OperableDescription = ShutOff1.GetComponent<OperableComponentDescription>();
        Tc1OperableDescription = Tc1.GetComponent<OperableComponentDescription>();

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

        // liquid.enabled = false;
        liquid.ReleaseSimulation();
        liquid.enabled = false;

        shutOffValveController.mainSupplyEmitter.enabled = false;



        Check1.transform.localPosition = new Vector3(-0.101f, 0, -0.08f);
        Check2.transform.localPosition = new Vector3(-0.201f, -2.25f, -0.17f);

        ResetTransforms();



        //remove attached hoses
        // for (int i = 0; i < hoseList.Count; i++)
        // {
        //     Actions.onHoseBibDrop?.Invoke(hoseList[i], hoseList[i].GetComponent<OperableComponentDescription>());

        // }
        // foreach (var hose in hoseList)
        // {
        //     Actions.onHoseBibDrop?.Invoke(hose, hose.GetComponent<OperableComponentDescription>());
        // }


        foreach (GameObject testCock in doubleCheckTestKitController.StaticTestCockList)
        {
            testCock.GetComponent<AssignTestCockManipulators>().testCockVoid.enabled = false;
            testCock.GetComponent<AssignTestCockManipulators>().testCockCollider.enabled = true;

        }
        foreach (var hose in doubleCheckTestKitController.AttachedHoseList)
        {

            Actions.onHoseBibGrab?.Invoke(hose, hose.GetComponent<OperableComponentDescription>());
            Actions.onHoseBibDrop?.Invoke(hose, hose.GetComponent<OperableComponentDescription>());

        }
        Actions.onSightTubeGrab?.Invoke(sightTube);
        Actions.onSightTubeDrop?.Invoke(sightTube);
        doubleCheckTestKitController.AttachedTestCockList.Clear();
        doubleCheckTestKitController.AttachedHoseList.Clear();
        if (testKitManager.isDoubleCheckTesting)
        {
            waterController.testCock4Str = UnityEngine.Random.Range(waterController.testCock4MinStr, waterController.testCock4MaxStr);
            waterController.testCock3Str = UnityEngine.Random.Range(waterController.testCock3MinStr, waterController.testCock3MaxStr);
        }

    }

}
