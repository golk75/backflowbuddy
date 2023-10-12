using System;
using System.Collections;
using Unity.VisualScripting;
using UnityEditor.Presets;
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
    private GameObject currentHoseBibObj;
    public ConfigurableJoint currentJoint;
    public ConfigurableJoint jointPreset;
    public GameObject jointPresetParent;
    private OperableComponentDescription currentHoseDescription;
    private GameObject currentTestCock;
    Rigidbody HoseRb;
    public Preset CongfigurableJointPreset;
    bool pointerDown;
    WaitForFixedUpdate waitForFixedUpdate = new WaitForFixedUpdate();
    public bool isAttaching;
    Vector3 testCockPosition;
    Vector3 testCockTransform;
    public HoseDetector hoseDetector;

    private void OnEnable()
    {
        Actions.onHoseBibGrab += GrabHoseBib;
        Actions.onHoseBibDrop += DropHoseBib;
        Actions.onHoseBibConnect += AttachHoseBib;

    }

    public void OnDisable()
    {
        Actions.onHoseBibGrab -= GrabHoseBib;
        Actions.onHoseBibDrop -= DropHoseBib;
        Actions.onHoseBibConnect -= AttachHoseBib;

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
        isAttaching = false;
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

        currentConfigurableJoint = currentHoseBibObj.GetComponent<ConfigurableJoint>();
        Destroy(currentConfigurableJoint);
        HoseRb = currentHoseBibObj.GetComponent<Rigidbody>();
        HoseRb.isKinematic = true;
        DetectHoseBibManipulation = StartCoroutine(MoveAnchor());


    }

    public void DropHoseBib(GameObject gameObject, OperableComponentDescription description)
    {
        // isAttaching = false;
        if (isAttaching != true)
        {
            if (currentHoseBibObj)
            {
                currentConfigurableJoint = currentHoseBibObj.AddComponent<ConfigurableJoint>();





            }
            // currentConfigurableJoint = currentHoseBibObj.AddComponent<ConfigurableJoint>();

            // CongfigurableJointPreset.ApplyTo(currentConfigurableJoint);

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
                    Debug.Log($"Not the HoseBib you're looking for");
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
            Debug.Log($"{currentConfigurableJoint.connectedAnchor}");
        }
        isAttaching = false;
        if (HoseRb != null)
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

    // Start is called before the first frame update
    void Start()
    {
        //configurableJoint.autoConfigureConnectedAnchor = true;
        initAnchorPos_highHose = HighHoseBib.GetComponent<ConfigurableJoint>().connectedAnchor;
        initAnchorPos_lowHose = LowHoseBib.GetComponent<ConfigurableJoint>().connectedAnchor;
        initAnchorPos_bypassHose = BypassHoseBib.GetComponent<ConfigurableJoint>().connectedAnchor;

        jointPreset = jointPresetParent.GetComponent<ConfigurableJoint>();


    }

    // Update is called once per frame
    void Update() { }
}
