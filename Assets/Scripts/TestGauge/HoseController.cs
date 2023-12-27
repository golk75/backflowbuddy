using System;
using System.Collections;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Rendering;


public class HoseController : MonoBehaviour
{
    public ConfigurableJoint currentConfigurableJoint;
    public PlayerController playerController;
    public CameraController cameraController;
    public DoubleCheckTestKitController doubleCheckTestKitController;
    private Vector3 initHighHosePos;
    private Vector3 initAnchorPos_highHose;
    private Vector3 initAnchorPos_lowHose;
    private Vector3 initAnchorPos_bypassHose;
    private Vector3 targetAnchorPos;
    public Vector3 connectionPoint;
    public Vector3 sightTubeHomePos;
    private Coroutine DetectHoseBibManipulation;
    private Coroutine HoseMovement;
    public GameObject HighHoseBib;
    public GameObject LowHoseBib;
    public GameObject BypassHoseBib;
    public GameObject m_ComponentToConnect;
    public Rigidbody HighHoseConfigJointConnectedBody;
    public Rigidbody LowHoseConfigJointConnectedBody;
    public Rigidbody BypassHoseConfigJointConnectedBody;
    public GameObject currentHoseBibObj;
    public ConfigurableJoint currentJoint;
    public ConfigurableJoint jointPreset;
    public GameObject jointPresetParent;
    private OperableComponentDescription currentHoseDescription;
    public GameObject sightTubeCurrentConnection;
    public GameObject currentTestCock;
    public GameObject sightTube;
    public GameObject highHoseBibTipHandle;
    public GameObject lowHoseBibTipHandle;
    public GameObject bypassHoseBibTipHandle;
    public GameObject currentTipHandle;
    Rigidbody HoseRb;
    bool pointerDown;
    public bool isAttaching;
    public bool isCurrentTestCockAttached;
    public bool sightTubeGrabbed = false;
    public bool isSightTubeConnected = false;

    public GameObject currentHoseTipCollider;
    public GameObject lastHoseTipCollider;
    public GameObject testCockToRemove;
    public Coroutine SightTubeMovement;


    Vector3 testCockPosition;
    Vector3 testCockTransform;
    public float testCockPositionOffset;
    public HoseDetector hoseDetector;
    public bool componentGrabbed;
    private void OnEnable()
    {
        Actions.onComponentGrab += GrabComponent;
        Actions.onComponentDrop += DropComponent;
        Actions.onComponentConnect += ComponentConnectionAttempt;
        Actions.onTestCockColliderEnter += GetCurrentTestCockColliderEntry;

    }

    public void OnDisable()
    {
        Actions.onComponentGrab -= GrabComponent;
        Actions.onComponentDrop -= DropComponent;
        Actions.onComponentConnect -= ComponentConnectionAttempt;
        Actions.onTestCockColliderEnter -= GetCurrentTestCockColliderEntry;
    }

    void Start()
    {
        jointPreset = jointPresetParent.GetComponent<ConfigurableJoint>();
        initAnchorPos_highHose = HighHoseBib.GetComponent<ConfigurableJoint>().connectedAnchor;
        initAnchorPos_lowHose = LowHoseBib.GetComponent<ConfigurableJoint>().connectedAnchor;
        initAnchorPos_bypassHose = BypassHoseBib.GetComponent<ConfigurableJoint>().connectedAnchor;
        sightTubeHomePos = sightTube.transform.localPosition;
    }



    private void GetCurrentTestCockColliderEntry(GameObject testCockDetector, OperableComponentDescription description)
    {
        //identifying current test cock being hooked up to, for connection positioning
        currentTestCock = testCockDetector;


    }


    private void ComponentConnectionAttempt(GameObject @object, OperableComponentDescription description, GameObject componentToConnect)
    {

        // componentToConnect.transform.position = @object.transform.position;

        m_ComponentToConnect = componentToConnect;


        //handle hose connect
        if (m_ComponentToConnect != sightTube)
        {
            Debug.Log($"2");
            isAttaching = true;
            switch (description.componentId)
            {
                case OperableComponentDescription.ComponentId.HighHose:
                    Destroy(HighHoseBib.GetComponent<ConfigurableJoint>());

                    break;
                case OperableComponentDescription.ComponentId.LowHose:
                    Destroy(LowHoseBib.GetComponent<ConfigurableJoint>());

                    break;
                case OperableComponentDescription.ComponentId.BypassHose:
                    Destroy(BypassHoseBib.GetComponent<ConfigurableJoint>());
                    break;
                    //         // case OperableComponentDescription.ComponentId.SightTube:
                    //         //     
                    //         //     break;
                    //         default:

                    break;
            }
            //sight tube will use connection point; see Update()
            connectionPoint = @object.transform.position;



            currentHoseBibObj.transform.position = connectionPoint;
            //Debug.Log($"currentHoseBibObj: {currentHoseBibObj.transform.position} | @object.transform.position: {@object.transform.position}");


        }

        //handle sight tube connect
        else
        {
            isAttaching = true;
            connectionPoint = new Vector3(@object.transform.position.x, @object.transform.position.y + testCockPositionOffset, @object.transform.position.z);
            m_ComponentToConnect.transform.position = connectionPoint;
        }


    }


    private void DropComponent(GameObject @object, OperableComponentDescription description)
    {
        componentGrabbed = false;

        if (@object != sightTube)
        {
            Debug.Log($"3");
            //check if component is attempting to connect, if so, DO NOT add config joint;
            if (isAttaching != true)
            {
                if (currentHoseBibObj)
                {

                    currentConfigurableJoint = currentHoseBibObj.AddComponent<ConfigurableJoint>();

                    currentConfigurableJoint = currentHoseBibObj.GetComponent<ConfigurableJoint>();

                    currentConfigurableJoint.autoConfigureConnectedAnchor = false;


                    switch (description.componentId)
                    {

                        case OperableComponentDescription.ComponentId.HighHose:
                            currentConfigurableJoint.connectedAnchor = initAnchorPos_highHose;
                            currentConfigurableJoint.connectedBody = HighHoseConfigJointConnectedBody;
                            break;
                        case OperableComponentDescription.ComponentId.LowHose:
                            currentConfigurableJoint.connectedAnchor = initAnchorPos_lowHose;
                            currentConfigurableJoint.connectedBody = LowHoseConfigJointConnectedBody;
                            break;
                        case OperableComponentDescription.ComponentId.BypassHose:
                            currentConfigurableJoint.connectedAnchor = initAnchorPos_bypassHose;
                            currentConfigurableJoint.connectedBody = BypassHoseConfigJointConnectedBody;
                            break;
                        case OperableComponentDescription.ComponentId.SightTube:

                            break;
                        default:


                            break;
                    }
                    currentConfigurableJoint.xMotion = jointPreset.xMotion;
                    currentConfigurableJoint.yMotion = jointPreset.yMotion;
                    currentConfigurableJoint.zMotion = jointPreset.zMotion;
                    currentConfigurableJoint.angularXMotion = jointPreset.angularXMotion;
                    currentConfigurableJoint.angularYMotion = jointPreset.angularYMotion;
                    currentConfigurableJoint.angularZMotion = jointPreset.angularZMotion;
                    currentConfigurableJoint.xDrive = jointPreset.xDrive;
                    currentConfigurableJoint.yDrive = jointPreset.yDrive;

                }

                if (HoseRb != null)
                    HoseRb.isKinematic = false;
                // isAttaching = false;
                // currentHoseBibObj = null;
            }
            isAttaching = false;
        }
        else
        {

            if (!isAttaching)
            {
                @object.transform.localPosition = sightTubeHomePos;

            }
            isAttaching = false;
        }

    }


    private void GrabComponent(GameObject @object, OperableComponentDescription description)
    {
        componentGrabbed = true;

        // Debug.Log($"@object: {@object}");
        switch (description.partsType)
        {
            case OperableComponentDescription.PartsType.TestKitHose:
                HandleHoseGrab(@object);
                break;
            case OperableComponentDescription.PartsType.TestKitSightTube:
                HandleSightTubeGrab(@object);
                break;
            default:
                break;
        }

    }


    private void HandleSightTubeGrab(GameObject obj)
    {
        SightTubeMovement = StartCoroutine(MoveHoseAnchor(obj));

    }


    private void HandleHoseGrab(GameObject obj)
    {

        Debug.Log($"1");

        switch (obj.GetComponent<OperableComponentDescription>().componentId)
        {
            case OperableComponentDescription.ComponentId.HighHose:
                currentHoseBibObj = HighHoseBib;
                currentTipHandle = highHoseBibTipHandle;
                break;
            case OperableComponentDescription.ComponentId.LowHose:
                currentHoseBibObj = LowHoseBib;
                currentTipHandle = lowHoseBibTipHandle;
                break;
            case OperableComponentDescription.ComponentId.BypassHose:
                currentHoseBibObj = BypassHoseBib;
                currentTipHandle = bypassHoseBibTipHandle;
                break;

            default:
                break;
        }



        currentConfigurableJoint = currentHoseBibObj.GetComponent<ConfigurableJoint>();
        Destroy(currentConfigurableJoint);
        HoseRb = currentHoseBibObj.GetComponent<Rigidbody>();
        HoseRb.isKinematic = true;
        currentTestCock = null;
        HoseMovement = StartCoroutine(MoveHoseAnchor(currentHoseBibObj));

    }


    /// <summary>
    /// "descrption" is the collider that enters -> the test cocks collider
    /// </summary>
    /// <param name="testCock"></param>
    /// <param name="description"></param>


    IEnumerator MoveHoseAnchor(GameObject obj)
    {
        //hose movement
        if (obj != sightTube)
        {
            //check is mouse left button or screen is being pressed down
            while (
                playerController.primaryClickPerformed > 0 && isAttaching == false
                || playerController.primaryClickStarted > 0 && isAttaching == false
            )
            {

                Vector3 direction =
                    Camera.main.ScreenToWorldPoint(Input.mousePosition)
                    - obj.transform.localPosition;

                HoseRb.Move(
                    new Vector3(direction.x, direction.y, obj.transform.position.z),
                    Quaternion.Euler(
                        obj.transform.eulerAngles.x,
                        obj.transform.eulerAngles.y,
                        obj.transform.eulerAngles.z
                    )
                );

                yield return null;

            }
        }
        //sight tube movement
        else
        {
            while (
                playerController.primaryClickPerformed > 0 && isAttaching == false
                || playerController.primaryClickStarted > 0 && isAttaching == false
            )
            {
                Vector3 direction = Camera.main.ScreenToWorldPoint(Input.mousePosition);

                obj.transform.position = new Vector3(direction.x, direction.y, obj.transform.position.z);
                yield return null;
            }


        }
    }
    void HandleHoming()
    {

    }

    // private void HoseBibConnectionAttempt(GameObject testCock, OperableComponentDescription description)
    // {
    //     isAttaching = true;
    //     switch (description.componentId)
    //     {
    //         case OperableComponentDescription.ComponentId.HighHose:
    //             Destroy(HighHoseBib.GetComponent<ConfigurableJoint>());

    //             break;
    //         case OperableComponentDescription.ComponentId.LowHose:
    //             Destroy(LowHoseBib.GetComponent<ConfigurableJoint>());

    //             break;
    //         case OperableComponentDescription.ComponentId.BypassHose:
    //             Destroy(BypassHoseBib.GetComponent<ConfigurableJoint>());
    //             break;
    //         // case OperableComponentDescription.ComponentId.SightTube:
    //         //     
    //         //     break;
    //         default:

    //             break;
    //     }
    //     //sight tube will use connection point; see Update()
    //     connectionPoint = testCock.transform.position;
    //     if (currentHoseBibObj == sightTube)
    //     {

    //         isSightTubeConnected = true;
    //     }
    //     else
    //     {
    //         currentHoseBibObj.transform.position = testCock.transform.position;
    //     }

    // }

    // public void GrabHoseBib(GameObject hoseBibHandle, OperableComponentDescription description)
    // {


    //     //  isAttaching = false;

    //     switch (description.componentId)
    //     {
    //         case OperableComponentDescription.ComponentId.HighHose:
    //             currentHoseBibObj = HighHoseBib;
    //             currentTipHandle = highHoseBibTipHandle;
    //             break;
    //         case OperableComponentDescription.ComponentId.LowHose:
    //             currentHoseBibObj = LowHoseBib;
    //             currentTipHandle = lowHoseBibTipHandle;
    //             break;
    //         case OperableComponentDescription.ComponentId.BypassHose:
    //             currentHoseBibObj = BypassHoseBib;
    //             currentTipHandle = bypassHoseBibTipHandle;
    //             break;
    //         case OperableComponentDescription.ComponentId.SightTube:
    //             currentHoseBibObj = sightTube;
    //             currentTipHandle = sightTube;
    //             break;
    //         default:
    //             break;
    //     }

    //     //checking for null, due to reset button call if there is at least the sight tube connected with no hoses
    //     if (currentHoseBibObj != sightTube)
    //     {

    //         // Actions.onRemoveHoseFromList?.Invoke(hoseBibHandle, hoseBibHandle.GetComponent<OperableComponentDescription>());

    //         currentConfigurableJoint = currentHoseBibObj.GetComponent<ConfigurableJoint>();
    //         Destroy(currentConfigurableJoint);
    //         HoseRb = currentHoseBibObj.GetComponent<Rigidbody>();
    //         HoseRb.isKinematic = true;
    //         if (currentTipHandle != sightTube)
    //         {
    //             DetectHoseBibManipulation = StartCoroutine(MoveAnchor(currentHoseBibObj));
    //         }


    //         currentTestCock = null;

    //     }
    //     else
    //     {


    //         DetectHoseBibManipulation = StartCoroutine(MoveAnchor(sightTube));
    //     }

    // }

    // public void DropHoseBib(GameObject gameObject, OperableComponentDescription description)
    // {

    //     if (isAttaching != true)
    //     {
    //         if (currentHoseBibObj)
    //         {
    //             currentConfigurableJoint = currentHoseBibObj.AddComponent<ConfigurableJoint>();

    //             currentConfigurableJoint = currentHoseBibObj.GetComponent<ConfigurableJoint>();

    //             currentConfigurableJoint.autoConfigureConnectedAnchor = false;


    //             switch (description.componentId)
    //             {

    //                 case OperableComponentDescription.ComponentId.HighHose:
    //                     currentConfigurableJoint.connectedAnchor = initAnchorPos_highHose;
    //                     currentConfigurableJoint.connectedBody = HighHoseConfigJointConnectedBody;
    //                     break;
    //                 case OperableComponentDescription.ComponentId.LowHose:
    //                     currentConfigurableJoint.connectedAnchor = initAnchorPos_lowHose;
    //                     currentConfigurableJoint.connectedBody = LowHoseConfigJointConnectedBody;
    //                     break;
    //                 case OperableComponentDescription.ComponentId.BypassHose:
    //                     currentConfigurableJoint.connectedAnchor = initAnchorPos_bypassHose;
    //                     currentConfigurableJoint.connectedBody = BypassHoseConfigJointConnectedBody;
    //                     break;
    //                 case OperableComponentDescription.ComponentId.SightTube:

    //                     break;
    //                 default:


    //                     break;
    //             }
    //             currentConfigurableJoint.xMotion = jointPreset.xMotion;
    //             currentConfigurableJoint.yMotion = jointPreset.yMotion;
    //             currentConfigurableJoint.zMotion = jointPreset.zMotion;
    //             currentConfigurableJoint.angularXMotion = jointPreset.angularXMotion;
    //             currentConfigurableJoint.angularYMotion = jointPreset.angularYMotion;
    //             currentConfigurableJoint.angularZMotion = jointPreset.angularZMotion;
    //             currentConfigurableJoint.xDrive = jointPreset.xDrive;
    //             currentConfigurableJoint.yDrive = jointPreset.yDrive;

    //         }

    //     }
    //     isAttaching = false;
    //     if (HoseRb != null)
    //         HoseRb.isKinematic = false;
    //     currentHoseBibObj = null;
    //     currentTipHandle = null;
    //     currentTestCock = null;
    // }



    // IEnumerator MoveAnchor(GameObject obj)
    // {

    //     //check is mouse left button or screen is being pressed down
    //     while (
    //         playerController.primaryClickPerformed > 0 && isAttaching == false
    //         || playerController.primaryClickStarted > 0 && isAttaching == false
    //     )
    //     {
    //         if (obj != sightTube)
    //         {
    //             //move object: currentHoseBibObj to -> mouse position: Camera.main.ScreenToWorldPoint(Input.mousePosition)
    //             Vector3 direction =
    //                 Camera.main.ScreenToWorldPoint(Input.mousePosition)
    //                 - currentHoseBibObj.transform.localPosition;

    //             //Works, although rb is not Kinematic?-->
    //             // highHoseRb.MovePosition(
    //             //     new Vector3(direction.x, direction.y, currentHoseBibObj.transform.position.z)
    //             // );
    //             HoseRb.Move(
    //                 new Vector3(direction.x, direction.y, currentHoseBibObj.transform.position.z),
    //                 Quaternion.Euler(
    //                     currentHoseBibObj.transform.eulerAngles.x,
    //                     currentHoseBibObj.transform.eulerAngles.y,
    //                     currentHoseBibObj.transform.eulerAngles.z
    //                 )
    //             );
    //         }
    //         else
    //         {
    //             isSightTubeConnected = false;
    //             Vector3 direction =
    //                          Camera.main.ScreenToWorldPoint(Input.mousePosition);

    //             obj.transform.position = new Vector3(direction.x, direction.y, obj.transform.position.z);
    //         }

    //         yield return null;

    //     }
    // }
    // /// <summary>
    // /// Sight Tube
    // /// </summary>
    // /// <param name="obj"></param>
    // /// <param name="description"></param>
    // // private void SightTubeConnectionAttempt(GameObject sightTubeObj, GameObject testCock, OperableComponentDescription description)
    // // {
    // //     if (currentTestCock != null)
    // //         //not allowing sight tube connection to test cock #1 (for now), since there is no reason in real life that this would be the case.
    // //         if (currentTestCock.GetComponent<OperableComponentDescription>().componentId != OperableComponentDescription.ComponentId.TestCock1)
    // //         {
    // //             connectionPoint = new Vector3(testCock.transform.position.x, testCock.transform.position.y + testCockPositionOffset, testCock.transform.position.z);

    // //             if (sightTube.GetComponent<OperableComponentDescription>().componentId == OperableComponentDescription.ComponentId.SightTube)
    // //             {
    // //                 // Vector3 currentDampVelocity = Vector3.zero;
    // //                 // Actions.onAddTestCockToList?.Invoke(currentTestCock, description);
    // //                 // Actions.onAddHoseToList?.Invoke(sightTubeObj, sightTubeObj.GetComponent<OperableComponentDescription>());
    // //                 // connectionPoint = new Vector3(currentTestCock.transform.position.x, currentTestCock.transform.position.y + testCockPositionOffset, currentTestCock.transform.position.z);
    // //                 sightTubeObj.transform.position = connectionPoint;
    // //                 isSightTubeConnected = true;

    // //             }
    // //         }



    // // }

    // // private void DropSightTube(GameObject sightTubeObj)
    // // {

    // //     sightTubeGrabbed = false;
    // //     isAttaching = false;
    // //     sightTubeObj.transform.localPosition = sightTubeHomePos;
    // //     currentHoseBibObj = null;
    // //     currentTipHandle = null;
    // //     currentTestCock = null;
    // //     // StopCoroutine(MovingSightTube(sightTubeObj));


    // // }

    // // private void GrabSightTube(GameObject sightTubeObj)
    // // {

    // //     sightTubeGrabbed = true;
    // //     isAttaching = false;

    // //     SightTubeMovement = StartCoroutine(MovingSightTube(sightTubeObj));
    // //     // if (!cameraController.isPanning)
    // //     // {
    // //     //     if (currentTestCock != null)
    // //     //     {
    // //     //         if (doubleCheckTestKitController.AttachedHoseList.Contains(sightTubeObj) && doubleCheckTestKitController.AttachedTestCockList.Contains(currentTestCock))
    // //     //         {
    // //     //             //add check for panning camera since sight tube floats a little offset from test cock if camera is panned aggressively / fast
    // //     //             //     Actions.onRemoveTestCockFromList?.Invoke(currentTestCock, currentTestCock.GetComponent<OperableComponentDescription>());
    // //     //             // Actions.onRemoveHoseFromList?.Invoke(sightTubeObj, sightTube.GetComponent<OperableComponentDescription>());
    // //     //         }

    // //     //     }
    // //     // }


    // // }

    // // IEnumerator MovingSightTube(GameObject go)
    // // {

    // //     isSightTubeConnected = false;
    // //     while (
    // //         playerController.primaryClickPerformed > 0 && sightTubeGrabbed == true && isSightTubeConnected == false
    // //         || playerController.primaryClickStarted > 0 && sightTubeGrabbed == true && isSightTubeConnected == false
    // //     )
    // //     {
    // //         Vector3 direction =
    // //           Camera.main.ScreenToWorldPoint(Input.mousePosition);



    // //         go.transform.position = new Vector3(direction.x, direction.y, go.transform.position.z);

    // //         yield return null;
    // //     }

    // // }
    // // Start is called before the first frame update
    // void Start()
    // {
    //     //configurableJoint.autoConfigureConnectedAnchor = true;
    //     initAnchorPos_highHose = HighHoseBib.GetComponent<ConfigurableJoint>().connectedAnchor;
    //     initAnchorPos_lowHose = LowHoseBib.GetComponent<ConfigurableJoint>().connectedAnchor;
    //     initAnchorPos_bypassHose = BypassHoseBib.GetComponent<ConfigurableJoint>().connectedAnchor;
    //     sightTubeHomePos = sightTube.transform.localPosition;
    //     jointPreset = jointPresetParent.GetComponent<ConfigurableJoint>();


    // }

    void Update()
    {
        // if (isSightTubeConnected)
        // {
        //     sightTube.transform.position = connectionPoint;
        // }


    }
}
