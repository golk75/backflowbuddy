using System;
using System.Collections;
using UnityEditor.Presets;
using UnityEngine;

public class HoseSpring : MonoBehaviour
{
    public ConfigurableJoint configurableJoint;
    public PlayerController playerController;
    private Vector3 initHighHosePos;
    private Vector3 initAnchorPos;
    private Vector3 targetAnchorPos;
    private Coroutine DetectHoseBibManipulation;
    private Coroutine AttachHose;
    public GameObject HighHoseBib;
    public GameObject LowHoseBib;
    public GameObject BypassHoseBib;
    public Rigidbody HighHoseConfigJointConnectedBody;
    private GameObject currentHoseBibObj;
    private OperableComponentDescription currentHoseDescription;
    private GameObject currentTestCock;
    Rigidbody HoseRb;
    public Preset CongfigurableJointPreset;
    bool pointerDown;
    WaitForFixedUpdate waitForFixedUpdate = new WaitForFixedUpdate();
    private bool isAttaching;
    Vector3 testCockPosition;

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

    private void AttachHoseBib(GameObject gameObject, OperableComponentDescription description)
    {
        isAttaching = true;
        if (currentHoseBibObj != null)
        {
            currentHoseBibObj.transform.position = gameObject.transform.position;
        }
        Debug.Log($"connection attempt");
        // StopCoroutine(MoveAnchor());
    }

    public void GrabHoseBib(GameObject gameObject, OperableComponentDescription description)
    {
        currentHoseDescription = description;

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

        HoseRb = currentHoseBibObj.GetComponent<Rigidbody>();
        //Debug.Log($"currentHoseBibObj = {currentHoseBibObj}");
        DetectHoseBibManipulation = StartCoroutine(MoveAnchor());
        isAttaching = false;
    }

    private void DropHoseBib(GameObject gameObject, OperableComponentDescription description)
    {
        //isAttaching = false;
        if (isAttaching != true)
        {
            configurableJoint = currentHoseBibObj.AddComponent<ConfigurableJoint>();
            CongfigurableJointPreset.ApplyTo(configurableJoint);
            configurableJoint.autoConfigureConnectedAnchor = false;
            configurableJoint.connectedAnchor = initAnchorPos;
            configurableJoint.connectedBody = HighHoseConfigJointConnectedBody;
        }
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
        while (playerController.primaryTouchStarted > 0 && isAttaching != true)
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
        initAnchorPos = configurableJoint.connectedAnchor;
    }

    // Update is called once per frame
    void Update()
    {
        // if (playerController.OperableObject != null)
        // {
        //     Debug.Log(playerController.OperableObject.tag);
        // }
        // else
        // {
        //     Debug.Log($"Not an operable object");
        // }
    }
}
