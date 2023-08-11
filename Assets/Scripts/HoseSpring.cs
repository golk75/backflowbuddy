using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.Serialization.Formatters;
using Unity.VisualScripting;
using UnityEditor.Presets;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UIElements;

public class HoseSpring : MonoBehaviour
{
    ConfigurableJoint configurableJoint;
    public PlayerController playerController;
    private Vector3 initHighHosePos;
    private Vector3 initAnchorPos;
    private Vector3 targetAnchorPos;
    private Coroutine DetectHoseBibManipulation;
    public GameObject HighHoseBib;
    public Rigidbody HighHoseConfigJointConnectedBody;
    public Preset CongfigurableJointPreset;
    bool pointerDown;

    private void OnEnable()
    {
        Actions.onHoseBibGrab += GrabHoseBib;
        Actions.onHoseBibDrop += DropHoseBib;
    }

    public void OnDisable()
    {
        Actions.onHoseBibGrab -= GrabHoseBib;
        Actions.onHoseBibDrop -= DropHoseBib;
    }

    public void GrabHoseBib()
    {
        DetectHoseBibManipulation = StartCoroutine(MoveAnchor());
        pointerDown = true;
    }

    private void DropHoseBib()
    {
        configurableJoint = gameObject.AddComponent<ConfigurableJoint>();
        CongfigurableJointPreset.ApplyTo(configurableJoint);
        configurableJoint.autoConfigureConnectedAnchor = false;
        configurableJoint.connectedAnchor = initAnchorPos;
        configurableJoint.connectedBody = HighHoseConfigJointConnectedBody;

        Debug.Log($"hose dropped");
    }

    IEnumerator MoveAnchor()
    {
        while (playerController.primaryTouchStarted == true)
        {
            //targetAnchorPos -= playerController.touchStart
            // configurableJoint.connectedAnchor = new Vector3(
            //     playerController.primaryTouchPos.x,
            //     playerController.primaryTouchPos.y,
            //     0
            // );
            //  Debug.Log($"configurableJoint.connectedAnchor = {configurableJoint.connectedAnchor}");
            // targetAnchorPos = initAnchorPos - Camera.main.ScreenToWorldPoint(Input.mousePosition);
            // /targetAnchorPos = initAnchorPos - Camera.main.ScreenToWorldPoint(Input.mousePosition);
            // Debug.Log($"targetAnchorPos = {targetAnchorPos}");

            // configurableJoint.connectedAnchor = new Vector3(
            //     -playerController.primaryFingerDelta.x + initAnchorPos.x,
            //     -playerController.primaryFingerDelta.y + initAnchorPos.y,
            //     initAnchorPos.z
            // );

            // transform.localPosition += new Vector3(
            //     playerController.primaryFingerDelta.x,
            //     playerController.primaryFingerDelta.y,
            //     initHighHosePos.z
            // );


            Destroy(configurableJoint);

            transform.position =
                Camera.main.ScreenToWorldPoint(Input.mousePosition) * Time.deltaTime;
            yield return null;
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        configurableJoint = GetComponent<ConfigurableJoint>();
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
