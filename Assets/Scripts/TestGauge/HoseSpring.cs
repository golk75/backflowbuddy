using System;
using System.Collections;
using Unity.VisualScripting;
// using UnityEditor.Presets;
using UnityEngine;

public class HoseSpring : MonoBehaviour
{
    public ConfigurableJoint currentConfigurableJoint;
    public PlayerController playerController;
    private Vector3 initHighHosePos;
    private Vector3 initAnchorPos_highHose;
    private Vector3 initAnchorPos_lowHose;
    private Vector3 initAnchorPos_bypassHose;
    private Vector3 targetAnchorPos;
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
    private GameObject currentTestCock;
    public GameObject sightTube;
    public GameObject highHoseBibTipHandle;
    public GameObject lowHoseBibTipHandle;
    public GameObject bypassHoseBibTipHandle;
    public GameObject currentTipHandle;
    Rigidbody HoseRb;
    // public Preset CongfigurableJointPreset;
    bool pointerDown;

    public bool isAttaching;

    Vector3 testCockPosition;
    Vector3 testCockTransform;
    public HoseDetector hoseDetector;

    private void OnEnable()
    {
        Actions.onHoseBibGrab += GrabHoseBib;
        Actions.onHoseBibDrop += DropHoseBib;
        // Actions.onHoseBibConnect += AttachHoseBib;
        Actions.onObjectConnect += HoseBibConnectionAttempt;

    }

    public void OnDisable()
    {
        Actions.onHoseBibGrab -= GrabHoseBib;
        Actions.onHoseBibDrop -= DropHoseBib;
        //Actions.onHoseBibConnect -= AttachHoseBib;
        Actions.onObjectConnect -= HoseBibConnectionAttempt;

    }

    /// <summary>
    /// "descrption" is the collider that enters -> the test cocks collider
    /// </summary>
    /// <param name="testCock"></param>
    /// <param name="description"></param>

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
            currentHoseBibObj.transform.position = testCock.transform.position;
            // currentTipHandle.transform.position = testCock.transform.position;
            //identifying current test cock being hooked up to, for tracking/listing purposes

            currentTestCock = testCock;
            //add test cock thats being connected to, to TestCockList @TestCockController
            Actions.onAddTestCockToList?.Invoke(testCock, GetComponent<OperableComponentDescription>());
            Actions.onAddHoseToList?.Invoke(currentHoseBibObj, description);
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
        Actions.onRemoveHoseFromList?.Invoke(currentHoseBibObj, description);


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
            if (currentTestCock)
            {
                Actions.onRemoveTestCockFromList?.Invoke(currentTestCock, GetComponent<OperableComponentDescription>());
            }
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

        jointPreset = jointPresetParent.GetComponent<ConfigurableJoint>();


    }


}
