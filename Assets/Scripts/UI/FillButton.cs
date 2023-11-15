using System.Collections;
using System.Collections.Generic;
using com.zibra.liquid.Manipulators;
using com.zibra.liquid.Solver;
using UnityEngine;
using UnityEngine.UIElements;

public class FillButton : MonoBehaviour
{

    Button fillButton;
    public ZibraLiquid liquid;
    public GameObject Check1;
    public GameObject Check2;
    public GameObject ShutOff1;
    public TestCockController testCockController;
    OperableComponentDescription ShutOff1OperableDescription;
    public PlayerController playerController;
    public ShutOffValveController shutOffValveController;
    public TestKitController testKitController;
    public GameObject SO1;
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
        var root = GetComponent<UIDocument>().rootVisualElement.Q<Button>("FillButton");
        fillButton = root.Q<Button>("FillButton");
        fillButton.clicked += FillDevice;

        ShutOff1OperableDescription = ShutOff1.GetComponent<OperableComponentDescription>();
    }

    public void FillDevice()
    {
        liquid.ReleaseSimulation();
        liquid.InitialState = ZibraLiquid.InitialStateType.BakedLiquidState;
        liquid.enabled = false;
        liquid.enabled = true;
        liquid.InitializeSimulation();

        Check1.transform.localPosition = new Vector3(-0.101f, 0, -0.08f);
        Check2.transform.localPosition = new Vector3(-0.201f, -2.25f, -0.17f);

        //shutOffValveController.ShutOffValve1.transform.eulerAngles = new Vector3(0, 180, 360);
        playerController.operableObject = ShutOff1;
        playerController.operableComponentDescription = ShutOff1OperableDescription;



        playerController._operableObjectRotation.z = 0;
        playerController._operableObjectRotation.y = 180;
        // SO1.transform.eulerAngles = new Vector3(90, 180, 360);

        foreach (GameObject testCock in testKitController.StaticTestCockList)
        {
            testCock.GetComponent<AssignTestCockManipulators>().testCockVoid.enabled = true;
            testCock.GetComponent<AssignTestCockManipulators>().testCockCollider.enabled = false;
        }
        // hoseSpring.DropHoseBib(GameObject gameObject, OperableComponentDescription description)
        // testKitController.DetachHoseBib();
    }

    // Update is called once per frame
    void Update() { }
}
