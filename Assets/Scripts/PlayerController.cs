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
        playerInput.Touchscreen.Touch1Contact.started += _ => Touch1Contact_started();
        playerInput.Touchscreen.Touch1Contact.canceled += _ => Touch1Contact_canceled();

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
    private void Touch1Contact_canceled()
    {
        Debug.Log($"Touch1 canceled");
        onZoomStop?.Invoke();
    }

    private void Touch1Contact_started()
    {
        Debug.Log($"Touch1 started");
        onZoomStart?.Invoke();
    }

    private void Touch0Delta_started(InputAction.CallbackContext context) { }

    private void Touch0Delta_canceled(InputAction.CallbackContext context)
    {
        //checking if a panningCoroutine was created yet (i.e. if an operable component is pressed before anything else)
    }

    private void Touch0Contact_started(InputAction.CallbackContext context)
    {
        touchStart = Camera.main.ScreenToWorldPoint(
            playerInput.Touchscreen.Touch0Position.ReadValue<Vector2>()
        );

        // DetectObjectWithRaycast();
    }

    private void Touch0Contact_canceled(InputAction.CallbackContext context) { }

    /*
        public void DetectObjectWithRaycast()
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;
            Physics.Raycast(ray, out hit);
            ///TODO----------->> create boundry for panning camera; differntiate between zooming and panning->
    
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
                    _operableObject = hit.collider.transform.gameObject;
                    _operableObjectRotation = _operableObject.transform.rotation.eulerAngles;
                    IsOperable = true;
                    //Debug.Log(_operableObject);
                }
            }
            else
            {
                if (!isZooming)
                {
                    isPanningCamera = true;
                }
                //Debug.Log($"Nothing hit!");
            }
        }
        */

    void Start() { }

    // Update is called once per frame
    void Update() { }
}
