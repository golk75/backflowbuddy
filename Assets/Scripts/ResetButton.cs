using System.Collections;
using System.Collections.Generic;
using com.zibra.liquid.Manipulators;
using com.zibra.liquid.Solver;
using UnityEngine;
using UnityEngine.UIElements;

public class ResetButton : MonoBehaviour
{

    Button resetButton;
    public ZibraLiquid liquid;
    public GameObject Check1;
    public GameObject Check2;
    public GameObject ShutOff1;
    public TestCockController testCockController;
    OperableComponentDescription ShutOff1OperableDescription;
    public PlayerController playerController;
    public ShutOffValveController shutOffValveController;
    public TestKitController testKitController;

    public ZibraLiquidForceField check1HousingFF;
    public ZibraLiquidForceField check2HousingFF;
    float initShutOffRot;

    private float checkffVelref;

    [SerializeField]
    ZibraLiquidCollider supplyCollider;

    [SerializeField]
    ZibraLiquidVoid supplyVoid;

    Vector3 initSupplyColliderPos;
    Vector3 supplyColliderTargetPos = new Vector3(-15f, 0, 0);
    Vector3 supplyVoidTargetPos = new Vector3(-9.5f, 0, 0);
    Vector3 initSupplyVoidPos;
    HoseSpring hoseSpring;
    /// <summary>
    /// Awake is called when the script instance is being loaded.
    /// </summary>
    private void Awake() { }

    // Start is called before the first frame update
    void Start()
    {
        var root = GetComponent<UIDocument>();
        resetButton = root.rootVisualElement.Q<Button>("ResetButton");
        resetButton.clicked += ResetDevice;

        ShutOff1OperableDescription = ShutOff1.GetComponent<OperableComponentDescription>();
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
        liquid.ReleaseSimulation();

        liquid.enabled = false;


        Check1.transform.localPosition = new Vector3(-0.101f, 0, -0.08f);
        Check2.transform.localPosition = new Vector3(-0.201f, -2.25f, -0.17f);

        //shutOffValveController.ShutOffValve1.transform.eulerAngles = new Vector3(0, 180, 360);
        playerController.operableObject = ShutOff1;
        playerController.operableComponentDescription = ShutOff1OperableDescription;

        playerController._operableObjectRotation.z = 90;

        foreach (GameObject testCock in testCockController.TestCockList)
        {
            testCock.GetComponent<AssignTestCockManipulators>().testCockVoid.enabled = false;
            testCock.GetComponent<AssignTestCockManipulators>().testCockCollider.enabled = true;
        }
        // hoseSpring.DropHoseBib(GameObject gameObject, OperableComponentDescription description)
        // testKitController.DetachHoseBib();
    }

    // Update is called once per frame
    void Update() { }
}
