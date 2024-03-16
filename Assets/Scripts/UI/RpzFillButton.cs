
using System.Security;
using com.zibra.liquid.Manipulators;
using com.zibra.liquid.Solver;
using UnityEngine;
using UnityEngine.UIElements;

public class RpzFillButton : MonoBehaviour
{

    Button m_FillButton;
    public ZibraLiquid liquid;
    public GameObject Check1;
    public GameObject Check2;
    public GameObject ShutOff1;

    public RPZWaterController rPZWaterController;
    OperableComponentDescription ShutOff1OperableDescription;
    public PlayerController playerController;
    public RpzTestKitController rpzTestKitController;
    [SerializeField]
    ZibraLiquidCollider supplyCollider;

    [SerializeField]
    ZibraLiquidVoid supplyVoid;




    // Start is called before the first frame update
    void Start()
    {
        var root = GetComponent<UIDocument>().rootVisualElement.Q<Button>("FillButton");
        m_FillButton = root.Q<Button>("FillButton");
        m_FillButton.clicked += FillDevice;

        ShutOff1OperableDescription = ShutOff1.GetComponent<OperableComponentDescription>();
    }

    public void FillDevice()
    {
        if (rPZWaterController.supplyPsi >= 0)
        {
            liquid.ReleaseSimulation();
            liquid.InitialState = ZibraLiquid.InitialStateType.BakedLiquidState;
            liquid.enabled = false;
            liquid.enabled = true;
            liquid.InitializeSimulation();

            // playerController.operableObject = ShutOff1;
            // playerController.operableComponentDescription = ShutOff1OperableDescription;



            // playerController._operableObjectRotation.z = 0;
            // playerController._operableObjectRotation.y = 180;


            foreach (GameObject testCock in rpzTestKitController.StaticTestCockList)
            {
                testCock.GetComponent<AssignTestCockManipulators>().testCockVoid.enabled = true;
                testCock.GetComponent<AssignTestCockManipulators>().testCockCollider.enabled = false;
            }
            // hoseController.DropHoseBib(GameObject gameObject, OperableComponentDescription description)
            // doubleCheckTestKitController.DetachHoseBib();
        }
    }

    // Update is called once per frame
    void Update() { }
}
