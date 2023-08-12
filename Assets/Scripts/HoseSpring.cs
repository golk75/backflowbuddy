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
    public Rigidbody HighHoseConfigJointConnectedBody;
    Rigidbody highHoseRb;
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
        HighHoseBib.transform.position = gameObject.transform.position;
        // StopCoroutine(MoveAnchor());
    }

    public void GrabHoseBib(GameObject gameObject, OperableComponentDescription description)
    {
        Destroy(configurableJoint);

        DetectHoseBibManipulation = StartCoroutine(MoveAnchor());
        isAttaching = false;
    }

    private void DropHoseBib(GameObject gameObject, OperableComponentDescription description)
    {
        //isAttaching = false;
        if (isAttaching != true)
        {
            configurableJoint = HighHoseBib.AddComponent<ConfigurableJoint>();
            CongfigurableJointPreset.ApplyTo(configurableJoint);
            configurableJoint.autoConfigureConnectedAnchor = false;
            configurableJoint.connectedAnchor = initAnchorPos;
            configurableJoint.connectedBody = HighHoseConfigJointConnectedBody;
        }
        Debug.Log($"hose dropped");
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
            //move object: HighHoseBib to mouse position: Camera.main.ScreenToWorldPoint(Input.mousePosition)
            Vector3 direction =
                Camera.main.ScreenToWorldPoint(Input.mousePosition)
                - HighHoseBib.transform.localPosition;

            //Works, although rb is not Kinematic?-->
            // highHoseRb.MovePosition(
            //     new Vector3(direction.x, direction.y, HighHoseBib.transform.position.z)
            // );
            highHoseRb.Move(
                new Vector3(direction.x, direction.y, HighHoseBib.transform.position.z),
                Quaternion.Euler(
                    HighHoseBib.transform.eulerAngles.x,
                    HighHoseBib.transform.eulerAngles.y,
                    HighHoseBib.transform.eulerAngles.z
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
        highHoseRb = HighHoseBib.GetComponent<Rigidbody>();
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
