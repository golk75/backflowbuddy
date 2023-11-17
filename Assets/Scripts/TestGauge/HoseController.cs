using System;
using System.Collections;
using Unity.VisualScripting;
// using UnityEditor.Presets;
using UnityEngine;
using UnityEngine.EventSystems;

public class HoseController : MonoBehaviour
{
    public ConfigurableJoint currentConfigurableJoint;
    public PlayerController playerController;
    public CameraController cameraController;
    public TestKitManager testKitManager;
    private Vector3 initHighHosePos;
    private Vector3 initAnchorPos_highHose;
    private Vector3 initAnchorPos_lowHose;
    private Vector3 initAnchorPos_bypassHose;
    private Vector3 targetAnchorPos;
    public Vector3 connectionPoint;
    public Vector3 sightTubeHomePos;
    private Coroutine DetectHoseBibManipulation;
    private Coroutine AttachHose;
    public GameObject HighHoseBib;
    public GameObject LowHoseBib;
    public GameObject BypassHoseBib;
    public Rigidbody HighHoseConfigJointConnectedBody;
    public Rigidbody LowHoseConfigJointConnectedBody;
    public Rigidbody BypassHoseConfigJointConnectedBody;
    public GameObject currentHoseBibObj;
    public ConfigurableJoint currentJoint;
    public ConfigurableJoint jointPreset;
    public GameObject jointPresetParent;
    private OperableComponentDescription currentHoseDescription;
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

    private void OnEnable()
    {
        Actions.onHoseBibGrab += GrabHoseBib;
        Actions.onHoseBibDrop += DropHoseBib;
        // Actions.onHoseBibConnect += AttachHoseBib;
        Actions.onHoseConnect += HoseBibConnectionAttempt;
        Actions.onSightTubeConnect += SightTubeConnectionAttempt;
        Actions.onTestCockColliderEnter += GetCurrentTestCockColliderEntry;
        Actions.onSightTubeGrab += GrabSightTube;
        Actions.onSightTubeDrop += DropSightTube;

    }

    public void OnDisable()
    {
        Actions.onSightTubeGrab -= GrabSightTube;
        Actions.onSightTubeDrop -= DropSightTube;
        Actions.onHoseBibGrab -= GrabHoseBib;
        Actions.onHoseBibDrop -= DropHoseBib;
        //Actions.onHoseBibConnect -= AttachHoseBib;
        Actions.onHoseConnect -= HoseBibConnectionAttempt;
        Actions.onSightTubeConnect -= SightTubeConnectionAttempt;
        Actions.onTestCockColliderEnter -= GetCurrentTestCockColliderEntry;
    }

    /// <summary>
    /// "descrption" is the collider that enters -> the test cocks collider
    /// </summary>
    /// <param name="testCock"></param>
    /// <param name="description"></param>
    private void GetCurrentTestCockColliderEntry(GameObject testCockDetector, OperableComponentDescription description)
    {
        //identifying current test cock being hooked up to, for connection positioning
        currentTestCock = testCockDetector;

    }


    private void HoseBibConnectionAttempt(GameObject testCock, OperableComponentDescription description)
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
            default:

                break;
        }
        if (currentHoseBibObj != null)
        {
            currentHoseBibObj.transform.position = currentTestCock.transform.position;
        }


    }

    public void GrabHoseBib(GameObject gameObject, OperableComponentDescription description)
    {


        isAttaching = false;

        switch (description.componentId)
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
        DetectHoseBibManipulation = StartCoroutine(MoveAnchor());
        currentTestCock = null;
        /// Moved to HoseDetector.cs
        // Actions.onRemoveHoseFromList?.Invoke(currentHoseBibObj, description);
        // Actions.onRemoveTestCockFromList?.Invoke(testCockToRemove, testCockToRemove.GetComponent<OperableComponentDescription>());





    }

    public void DropHoseBib(GameObject gameObject, OperableComponentDescription description)
    {

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

        }
        isAttaching = false;
        if (HoseRb != null)
            HoseRb.isKinematic = false;
        currentHoseBibObj = null;
        currentTipHandle = null;
        currentTestCock = null;


    }



    IEnumerator MoveAnchor()
    {

        //check is mouse left button or screen is being pressed down
        while (
            playerController.primaryTouchStarted > 0 && isAttaching == false
            || playerController.primaryClickStarted > 0 && isAttaching == false
        )
        {
            //move object: currentHoseBibObj to -> mouse position: Camera.main.ScreenToWorldPoint(Input.mousePosition)
            Vector3 direction =
                Camera.main.ScreenToWorldPoint(Input.mousePosition)
                - currentHoseBibObj.transform.localPosition;

            //Works, although rb is not Kinematic?-->
            // highHoseRb.MovePosition(
            //     new Vector3(direction.x, direction.y, currentHoseBibObj.transform.position.z)
            // );
            HoseRb.Move(
                new Vector3(direction.x, direction.y, currentHoseBibObj.transform.position.z),
                Quaternion.Euler(
                    currentHoseBibObj.transform.eulerAngles.x,
                    currentHoseBibObj.transform.eulerAngles.y,
                    currentHoseBibObj.transform.eulerAngles.z
                )
            );

            yield return null;
        }
    }
    /// <summary>
    /// Sight Tube
    /// </summary>
    /// <param name="obj"></param>
    /// <param name="description"></param>
    private void SightTubeConnectionAttempt(GameObject sightTubeObj, OperableComponentDescription description)
    {
        connectionPoint = new Vector3(currentTestCock.transform.position.x, currentTestCock.transform.position.y + testCockPositionOffset, currentTestCock.transform.position.z);

        if (sightTube.GetComponent<OperableComponentDescription>().componentId == OperableComponentDescription.ComponentId.SightTube)
        {
            Vector3 currentDampVelocity = Vector3.zero;
            Actions.onAddTestCockToList?.Invoke(currentTestCock, description);
            Actions.onAddHoseToList?.Invoke(sightTubeObj, sightTubeObj.GetComponent<OperableComponentDescription>());
            // connectionPoint = new Vector3(currentTestCock.transform.position.x, currentTestCock.transform.position.y + testCockPositionOffset, currentTestCock.transform.position.z);
            sightTubeObj.transform.position = connectionPoint;
            isSightTubeConnected = true;

        }



    }

    private void DropSightTube(GameObject sightTubeObj)
    {

        sightTubeGrabbed = false;
        isAttaching = false;
        sightTubeObj.transform.localPosition = sightTubeHomePos;
        StopCoroutine(MovingSightTube(sightTubeObj));


    }

    private void GrabSightTube(GameObject sightTubeObj)
    {

        sightTubeGrabbed = true;
        isAttaching = false;

        SightTubeMovement = StartCoroutine(MovingSightTube(sightTubeObj));
        if (!cameraController.isPanning)
        {
            if (currentTestCock != null)
            {
                if (testKitManager.AttachedHoseList.Contains(sightTubeObj) && testKitManager.AttachedTestCockList.Contains(currentTestCock))
                    //add check for panning camera since sight tube floats a little offset from test cock if camera is panned aggressively / fast
                    Actions.onRemoveTestCockFromList?.Invoke(currentTestCock, currentTestCock.GetComponent<OperableComponentDescription>());
                Actions.onRemoveHoseFromList?.Invoke(sightTubeObj, sightTube.GetComponent<OperableComponentDescription>());
            }
        }


    }

    IEnumerator MovingSightTube(GameObject go)
    {

        isSightTubeConnected = false;
        while (
            playerController.primaryTouchStarted > 0 && sightTubeGrabbed == true && isSightTubeConnected == false
            || playerController.primaryClickStarted > 0 && sightTubeGrabbed == true && isSightTubeConnected == false
        )
        {
            Vector3 direction =
              Camera.main.ScreenToWorldPoint(Input.mousePosition);



            go.transform.position = new Vector3(direction.x, direction.y, go.transform.position.z);

            yield return null;
        }

    }
    // Start is called before the first frame update
    void Start()
    {
        //configurableJoint.autoConfigureConnectedAnchor = true;
        initAnchorPos_highHose = HighHoseBib.GetComponent<ConfigurableJoint>().connectedAnchor;
        initAnchorPos_lowHose = LowHoseBib.GetComponent<ConfigurableJoint>().connectedAnchor;
        initAnchorPos_bypassHose = BypassHoseBib.GetComponent<ConfigurableJoint>().connectedAnchor;
        sightTubeHomePos = sightTube.transform.localPosition;
        jointPreset = jointPresetParent.GetComponent<ConfigurableJoint>();


    }

    void Update()
    {
        if (isSightTubeConnected)
        {
            sightTube.transform.position = connectionPoint;
        }
    }
}
