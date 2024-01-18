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
    public UiClickFilter uiClickFilter;
    public DoubleCheckTestKitController doubleCheckTestKitController;
    private Vector3 initHighHosePos;
    private Vector3 initAnchorPos_highHose;
    private Vector3 initAnchorPos_lowHose;
    private Vector3 initAnchorPos_bypassHose;
    private Vector3 targetAnchorPos;
    public Vector3 hoseConnectionPoint;
    public Vector3 sightTubeConnectionPoint;
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
            Debug.Log($"description: {description}");
            hoseConnectionPoint = @object.transform.position;



            currentHoseBibObj.transform.position = hoseConnectionPoint;
            //Debug.Log($"currentHoseBibObj: {currentHoseBibObj.transform.position} | @object.transform.position: {@object.transform.position}");


        }

        //handle sight tube connect
        else
        {
            isAttaching = true;
            sightTubeConnectionPoint = new Vector3(@object.transform.position.x, @object.transform.position.y + testCockPositionOffset, @object.transform.position.z);
            isSightTubeConnected = true;
            // m_ComponentToConnect.transform.position = connectionPoint;
        }


    }


    private void DropComponent(GameObject @object, OperableComponentDescription description)
    {
        componentGrabbed = false;
        //check if its not the sight tube
        if (@object != sightTube)
        {

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
            }
            isAttaching = false;
        }

        else
        {

            if (!isAttaching)
            {
                @object.transform.localPosition = sightTubeHomePos;
                isSightTubeConnected = false;
            }
            isAttaching = false;
        }

    }


    private void GrabComponent(GameObject @object, OperableComponentDescription description)
    {

        if (uiClickFilter.isUiClicked == false)
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
    }


    private void HandleSightTubeGrab(GameObject obj)
    {
        SightTubeMovement = StartCoroutine(MoveHoseAnchor(obj));

    }


    private void HandleHoseGrab(GameObject obj)
    {



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

                // Vector3 direction = Camera.main.ScreenToWorldPoint(Input.mousePosition) - obj.transform.localPosition;
                Vector3 direction = Camera.main.ScreenToWorldPoint(Input.mousePosition);

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


    void FixedUpdate()
    {
        if (isSightTubeConnected)
        {
            sightTube.transform.position = sightTubeConnectionPoint;
        }

    }
}
