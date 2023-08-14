using System;
using System.Collections;
using UnityEditor.Presets;
using UnityEngine;

public class HoseSpring : MonoBehaviour
{
    public ConfigurableJoint configurableJoint;
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
    private GameObject currentHoseBibObj;
    private OperableComponentDescription currentHoseDescription;
    private GameObject currentTestCock;
    Rigidbody HoseRb;
    public Preset CongfigurableJointPreset;
    bool pointerDown;
    WaitForFixedUpdate waitForFixedUpdate = new WaitForFixedUpdate();
    private bool isAttaching;
    Vector3 testCockPosition;
    Vector3 testCockTransform;
    public HoseDetector hoseDetector;

    private void OnEnable()
    {
        Actions.onHoseBibGrab += GrabHoseBib;
        Actions.onHoseBibDrop += DropHoseBib;
        Actions.onHoseBibConnect += AttachHoseBib;
        Actions.onHoseContact += CheckTestCockContact;
    }

    public void OnDisable()
    {
        Actions.onHoseBibGrab -= GrabHoseBib;
        Actions.onHoseBibDrop -= DropHoseBib;
        Actions.onHoseBibConnect -= AttachHoseBib;
        Actions.onHoseContact += CheckTestCockContact;
    }

    /// <summary>
    /// "descrption" is the collider that enters -> the test cocks collider
    /// </summary>
    /// <param name="testCock"></param>
    /// <param name="description"></param>
    private void CheckTestCockContact(GameObject testCock, OperableComponentDescription description)
    {
        // Debug.Log(
        //     $" testCock.name = {testCock.name} |  testCock.transform.position = {testCock.transform.position} | description = {description.componentId}"
        // );
    }

    private void AttachHoseBib(GameObject testCock, OperableComponentDescription description)
    {
        isAttaching = true;

        switch (description.componentId)
        {
            case OperableComponentDescription.ComponentId.HighHose:
                Destroy(HighHoseBib.GetComponent<ConfigurableJoint>());
                Debug.Log($"High config joint destroyed");
                break;
            case OperableComponentDescription.ComponentId.LowHose:
                Destroy(LowHoseBib.GetComponent<ConfigurableJoint>());
                Debug.Log($"Low config joint destroyed");
                break;
            case OperableComponentDescription.ComponentId.BypassHose:
                Destroy(BypassHoseBib.GetComponent<ConfigurableJoint>());
                Debug.Log($"Bypass config joint destroyed");
                break;
            default:
                Debug.Log($"No config. joint to destroy");
                break;
        }
        currentHoseBibObj.transform.position = testCock.transform.position;
        Debug.Log($"connection attempt");
    }

    public void GrabHoseBib(GameObject gameObject, OperableComponentDescription description)
    {
        Debug.Log($"grabbing hose..");
        switch (description.componentId)
        {
            case OperableComponentDescription.ComponentId.HighHose:
                currentHoseBibObj = HighHoseBib;
                break;
            case OperableComponentDescription.ComponentId.LowHose:
                currentHoseBibObj = LowHoseBib;
                break;
            case OperableComponentDescription.ComponentId.BypassHose:
                currentHoseBibObj = BypassHoseBib;
                break;
            default:
                Debug.Log($"Not the HoseBib you're looking for");
                break;
        }

        configurableJoint = currentHoseBibObj.GetComponent<ConfigurableJoint>();
        Destroy(configurableJoint);
        HoseRb = currentHoseBibObj.GetComponent<Rigidbody>();
        HoseRb.isKinematic = true;
        DetectHoseBibManipulation = StartCoroutine(MoveAnchor());

        isAttaching = false;
    }

    private void DropHoseBib(GameObject gameObject, OperableComponentDescription description)
    {
        // isAttaching = false;
        if (isAttaching != true)
        {
            configurableJoint = currentHoseBibObj.AddComponent<ConfigurableJoint>();
            CongfigurableJointPreset.ApplyTo(configurableJoint);
            configurableJoint.autoConfigureConnectedAnchor = false;
            switch (description.componentId)
            {
                case OperableComponentDescription.ComponentId.HighHose:
                    configurableJoint.connectedAnchor = initAnchorPos_highHose;
                    configurableJoint.connectedBody = HighHoseConfigJointConnectedBody;
                    break;
                case OperableComponentDescription.ComponentId.LowHose:
                    configurableJoint.connectedAnchor = initAnchorPos_lowHose;
                    configurableJoint.connectedBody = LowHoseConfigJointConnectedBody;
                    break;
                case OperableComponentDescription.ComponentId.BypassHose:
                    configurableJoint.connectedAnchor = initAnchorPos_bypassHose;
                    configurableJoint.connectedBody = BypassHoseConfigJointConnectedBody;
                    break;
                default:
                    Debug.Log($"Not the HoseBib you're looking for");
                    break;
            }
        }
        isAttaching = false;
        HoseRb.isKinematic = false;
        //Debug.Log($"hose dropped");
    }

    Vector3 GetPointerPos()
    {
        Vector3 screenPosition = Input.mousePosition;

        return Camera.main.ScreenToWorldPoint(screenPosition);
    }

    IEnumerator MoveAnchor()
    {
        //check is mouse left button or screen is being pressed down
        while (playerController.primaryTouchStarted > 0 && isAttaching == false)
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

    // Start is called before the first frame update
    void Start()
    {
        //configurableJoint.autoConfigureConnectedAnchor = true;
        initAnchorPos_highHose = HighHoseBib.GetComponent<ConfigurableJoint>().connectedAnchor;
        initAnchorPos_lowHose = LowHoseBib.GetComponent<ConfigurableJoint>().connectedAnchor;
        initAnchorPos_bypassHose = BypassHoseBib.GetComponent<ConfigurableJoint>().connectedAnchor;
    }

    // Update is called once per frame
    void Update() { }
}
