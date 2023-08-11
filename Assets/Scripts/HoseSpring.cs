using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UIElements;

public class HoseSpring : MonoBehaviour
{
    ConfigurableJoint configurableJoint;
    public PlayerController playerController;
    private Vector3 initAnchorPos;
    private Vector3 targetAnchorPos;
    private Coroutine DetectHoseBibManipulation;
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
        configurableJoint.connectedAnchor = initAnchorPos;
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
            targetAnchorPos = initAnchorPos - Camera.main.ScreenToWorldPoint(Input.mousePosition);

            // Debug.Log($"targetAnchorPos = {targetAnchorPos}");
            configurableJoint.connectedAnchor = targetAnchorPos;
            yield return null;
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        configurableJoint = GetComponent<ConfigurableJoint>();
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
