using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    PlayerInputAction playerInput;
    public Vector3 touchStart;

    public Vector2 primaryTouchPos;

    public static event Action onZoomStart;
    public static event Action onZoomStop;

    public static event Action onPanStart;
    public static event Action onPanCanceled;

    public bool isOperableObject = false;
    public bool primaryTouchStarted = false;
    public bool secondaryTouchStarted = false;

    // Start is called before the first frame update
    void Awake()
    {
        playerInput = new PlayerInputAction();
        /*

        
        Checkvalve1InitPos = CheckValve1.transform.localPosition;
        Checkvalve2InitPos = CheckValve2.transform.localPosition;
        testCockClosedScale = TestCockValve1.transform.localScale;
        testCockClosedYScale = testCockClosedScale.y;
        

        //touch input


        */
        //Touch0
        playerInput.Touchscreen.Touch0Contact.started += Touch0Contact_started;
        playerInput.Touchscreen.Touch0Contact.canceled += Touch0Contact_canceled;
        playerInput.Touchscreen.Touch0Delta.started += Touch0Delta_started;
        playerInput.Touchscreen.Touch0Delta.canceled += Touch0Delta_canceled;
        playerInput.Touchscreen.Touch1Contact.started += Touch1Contact_started;
        playerInput.Touchscreen.Touch1Contact.canceled += Touch1Contact_canceled;

        //Touch1
        //playerInput.Touchscreen.Touch1Contact.started += Zoom_started;
        //playerInput.Touchscreen.Touch1Contact.canceled += Zoom_canceled;
    }

    void OnEnable()
    {
        playerInput.Enable();
    }

    void OnDisable()
    {
        playerInput.Disable();
    }

    /// <summary>
    ///
    /// </summary>
    ///

    private void Touch0Contact_started(InputAction.CallbackContext context)
    {
        touchStart = Camera.main.ScreenToWorldPoint(
            playerInput.Touchscreen.Touch0Position.ReadValue<Vector2>()
        );
        primaryTouchStarted = context.ReadValueAsButton();

        // DetectObjectWithRaycast();
    }

    private void Touch0Contact_canceled(InputAction.CallbackContext context)
    {
        primaryTouchStarted = context.ReadValueAsButton();
        onPanCanceled?.Invoke();
    }

    private void Touch1Contact_started(InputAction.CallbackContext context)
    {
        Debug.Log($"Touch1 started");
        secondaryTouchStarted = context.ReadValueAsButton();
    }

    private void Touch1Contact_canceled(InputAction.CallbackContext context)
    {
        secondaryTouchStarted = context.ReadValueAsButton();
        Debug.Log($"Touch1 canceled");
        onZoomStop?.Invoke();
    }

    private void Touch0Delta_started(InputAction.CallbackContext context) { }

    private void Touch0Delta_canceled(InputAction.CallbackContext context) { }

    public void DetectObjectWithRaycast()
    {
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;
        Physics.Raycast(ray, out hit);

        ///check if anything is hit, then if something was hit, check whether it is an operable component or not
        /// (if it has an OperableComponentDescription component, then it is operable)
        if (hit.collider != null)
        {
            if (
                hit.collider.transform.TryGetComponent<OperableComponentDescription>(
                    out OperableComponentDescription component
                )
            )
            {
                isOperableObject = true;
            }
            else
            {
                isOperableObject = false;
            }
        }
    }

    void Start() { }

    // Update is called once per frame
    void Update() { }
}
