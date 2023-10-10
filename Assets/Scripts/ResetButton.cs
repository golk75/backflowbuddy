using System.Collections;
using System.Collections.Generic;
using com.zibra.liquid.Manipulators;
using com.zibra.liquid.Solver;
using Unity.Mathematics;
using UnityEngine;
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
    OperableComponentDescription ShutOff1OperableDescription;
    OperableComponentDescription Tc1OperableDescription;
    public PlayerController playerController;
    public ShutOffValveController shutOffValveController;
    public TestKitController testKitController;

    public ZibraLiquidForceField check1HousingFF;
    public ZibraLiquidForceField check2HousingFF;
    [SerializeField]
    List<ResetableObject> objectsToReset;
    ResetableObject resetableObject;
    ZibraLiquid resetVoid;

    public List<GameObject> HoseList;


    [System.Serializable]
    public class ResetableObject
    {


        public GameObject resetThis;
        public Quaternion initRotation;



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
        resetableObject.initRotation = resetableObject.resetThis.transform.rotation;


    }
    private void ResetTransforms()
    {
        foreach (ResetableObject item in objectsToReset)
        {
            item.resetThis.transform.rotation = item.initRotation;
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





        Check1.transform.localPosition = new Vector3(-0.101f, 0, -0.08f);
        Check2.transform.localPosition = new Vector3(-0.201f, -2.25f, -0.17f);

        //shutOffValveController.ShutOffValve1.transform.eulerAngles = new Vector3(0, 180, 360);

        // playerController.operableComponentDescription = ShutOff1OperableDescription;

        ResetTransforms();
        // playerController._operableObjectRotation.z = 90;
        // playerController._operableObjectRotation.y = 180;


        // playerController.operableObject = Tc1;
        // playerController.operableComponentDescription = Tc1OperableDescription;

        // playerController._operableObjectRotation.z = 0;
        // playerController._operableObjectRotation.y = 180;


        //remove attached hoses
        for (int i = 0; i < HoseList.Count; i++)
        {
            Actions.onHoseBibDrop?.Invoke(HoseList[i], HoseList[i].GetComponent<OperableComponentDescription>());
        }



        // Actions.onHoseBibDrop?.Invoke(operableObject, operableComponentDescription);

        // foreach (GameObject testCock in testCockController.TestCockList)
        // {
        //     testCock.GetComponent<AssignTestCockManipulators>().testCockVoid.enabled = false;
        //     testCock.GetComponent<AssignTestCockManipulators>().testCockCollider.enabled = true;
        // }
        // hoseSpring.DropHoseBib(GameObject gameObject, OperableComponentDescription description)
        // testKitController.DetachHoseBib();
    }

    // Update is called once per frame
    void Update() { }
}
