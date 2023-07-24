using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    [SerializeField]
    GameObject TestCockManager;
    TestCockController testCockController;
    OperableComponentsUsed list;
    PlayerInputAction playerInput;
    public InputAction Touch0Position;
    public Vector3 touchStart;

    public Vector2 primaryTouchPos;

    //Camera events
    public static event Action onZoomStop;

    public static event Action onPanCanceled;

    //Operable component events
    public static event Action onTestCockOperation;
    public static event Action onShutOffOperation;

    public bool isOperableObject = false;
    public bool primaryTouchStarted = false;
    public bool secondaryTouchStarted = false;
    public bool primaryTouchPerformed = false;
    public GameObject OperableObject
    {
        get { return _operableObject; }
        private set { _operableObject = value; }
    }
    public GameObject OperableValve
    {
        get { return _operableValve; }
        private set { _operableValve = value; }
    }

    public Vector3 _operableObjectRotation;
    Vector3 _operableValveScale;

    public Vector3 OperableObjectRotation
    {
        get { return _operableObjectRotation; }
        private set { _operableObjectRotation = value; }
    }
    public Vector3 OperableValveScale
    {
        get { return _operableValveScale; }
        private set { _operableValveScale = value; }
    }
    public float deviceRotSensitivity;

    [SerializeField]
    public GameObject _operableObject;

    [SerializeField]
    GameObject _operableValve;

    public bool isInit = false;
    public GameObject initialOperableObject;

    // Start is called before the first frame update
    void Awake()
    {
        playerInput = new PlayerInputAction();
        list = GetComponent<OperableComponentsUsed>();
        testCockController = TestCockManager.GetComponent<TestCockController>();
        //Input
        playerInput.Touchscreen.Touch0Contact.started += Touch0Contact_started;
        playerInput.Touchscreen.Touch0Contact.canceled += Touch0Contact_canceled;
        playerInput.Touchscreen.Touch0Contact.performed += Touch0Contact_performed;
        playerInput.Touchscreen.Touch0Delta.started += Touch0Delta_started;
        playerInput.Touchscreen.Touch0Delta.canceled += Touch0Delta_canceled;
        playerInput.Touchscreen.Touch1Contact.started += Touch1Contact_started;
        playerInput.Touchscreen.Touch1Contact.canceled += Touch1Contact_canceled;
        Touch0Position = playerInput.Touchscreen.Touch0Position;
        _operableObject = initialOperableObject;
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
    ///Input-------------------
    /// </summary>


    public void Touch0Contact_started(InputAction.CallbackContext context)
    {
        isInit = true;
        touchStart = Camera.main.ScreenToWorldPoint(
            playerInput.Touchscreen.Touch0Position.ReadValue<Vector2>()
        );
        primaryTouchStarted = context.ReadValueAsButton();
        DetectObjectWithRaycast();
    }

    private void Touch0Contact_canceled(InputAction.CallbackContext context)
    {
        primaryTouchStarted = context.ReadValueAsButton();
        primaryTouchPerformed = context.ReadValueAsButton();
        onPanCanceled?.Invoke();

        isOperableObject = false;
        touchStart = Vector3.zero;
    }

    private void Touch0Contact_performed(InputAction.CallbackContext context)
    {
        primaryTouchPerformed = context.ReadValueAsButton();
    }

    private void Touch1Contact_started(InputAction.CallbackContext context)
    {
        //Debug.Log($"Touch1 started");
        secondaryTouchStarted = context.ReadValueAsButton();
    }

    private void Touch1Contact_canceled(InputAction.CallbackContext context)
    {
        secondaryTouchStarted = context.ReadValueAsButton();

        //Debug.Log($"Touch1 canceled");
        onZoomStop?.Invoke();
    }

    private void Touch0Delta_started(InputAction.CallbackContext context) { }

    private void Touch0Delta_canceled(InputAction.CallbackContext context) { }

    /// END INPUT
    ///-----------------------

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
                _operableObject = hit.collider.transform.gameObject;
                _operableObjectRotation = _operableObject.transform.rotation.eulerAngles;

                //GetOperableComponentComponent(hit.collider.transform.gameObject);
            }
            else
            {
                isOperableObject = false;
            }
        }
    }

    private void Operate()
    {
        //_operableObjectRotation = _operableObject.transform.rotation.eulerAngles;

        Vector2 primaryFingerPos = playerInput.Touchscreen.Touch0Position.ReadValue<Vector2>();
        _operableObjectRotation.z +=
            (touchStart.x - Camera.main.ScreenToWorldPoint(Input.mousePosition).x)
            * deviceRotSensitivity;

        //rotation clamp for parts that rotate arpund center mass (i.e. test cock valves)
        _operableObjectRotation.z = Mathf.Clamp(_operableObjectRotation.z, 0.0f, 90.0f);
        _operableObject.transform.rotation = Quaternion.Euler(_operableObjectRotation);
    }

    public void GetOperableComponentComponent(GameObject operableComponent)
    {
        _operableObject = operableComponent.gameObject;
        _operableObjectRotation = operableComponent.transform.rotation.eulerAngles;
    }

    private void Start() { }

    /// <summary>
    /// This will be in the update method to constantly check if the input was intended for assembly operation
    /// </summary>
    public void OperateCheck()
    {
        if (isOperableObject == true && primaryTouchStarted)
        {
            Operate();
        }
    }

    // Update is called once per frame
    private void Update()
    {
        OperateCheck();
    }
}
