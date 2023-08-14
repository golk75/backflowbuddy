using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor.ShaderGraph;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    [SerializeField]
    GameObject TestCockManager;
    TestCockController testCockController;
    public TestKitController testKitController;
    public GameObject FillButton;

    [SerializeField]
    GameObject WaterManager;
    private WaterController waterController;

    PlayerInputAction playerInput;
    public InputAction Touch0Position;
    public Vector3 touchStart;

    [SerializeField]
    public Vector2 primaryTouchStartPos;

    //Camera events
    public static event Action onZoomStop;

    public static event Action onPanCanceled;

    public bool isOperableObject = false;
    public float primaryTouchStarted;
    public bool secondaryTouchStarted = false;
    public bool primaryTouchPerformed = false;
    public GameObject OperableObject
    {
        get { return operableObject; }
        private set { operableObject = value; }
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
    public GameObject operableObject;

    [SerializeField]
    public GameObject _operableTestGaugeObject;
    public GameObject OperableTestGaugeObject
    {
        get { return _operableTestGaugeObject; }
        private set { _operableTestGaugeObject = value; }
    }

    [SerializeField]
    GameObject _operableValve;

    public bool isInit = false;
    public GameObject initialOperableObject;
    public GameObject initialTestGaugeOperableObject;
    public OperableComponentDescription operableComponentDescription;
    public Vector2 primaryFingerPos;
    public Vector2 primaryFingerDelta;

    // Start is called before the first frame update
    void Awake()
    {
        playerInput = new PlayerInputAction();

        testCockController = TestCockManager.GetComponent<TestCockController>();
        waterController = WaterManager.GetComponent<WaterController>();
        //Input
        playerInput.Touchscreen.Touch0Contact.started += Touch0Contact_started;
        playerInput.Touchscreen.Touch0Contact.canceled += Touch0Contact_canceled;
        playerInput.Touchscreen.Touch0Contact.performed += Touch0Contact_performed;
        playerInput.Touchscreen.Touch0Delta.started += Touch0Delta_started;
        playerInput.Touchscreen.Touch0Delta.canceled += Touch0Delta_canceled;
        playerInput.Touchscreen.Touch1Contact.started += Touch1Contact_started;
        playerInput.Touchscreen.Touch1Contact.canceled += Touch1Contact_canceled;
        playerInput.Touchscreen.Touch0Delta.started += Touch0Delta_started;
        Touch0Position = playerInput.Touchscreen.Touch0Position;
        operableObject = initialOperableObject;
        _operableTestGaugeObject = initialTestGaugeOperableObject;
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
        primaryTouchStartPos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        primaryTouchStarted = context.ReadValue<float>();

        DetectObjectWithRaycast();
    }

    private void Touch0Contact_canceled(InputAction.CallbackContext context)
    {
        primaryTouchStarted = context.ReadValue<float>();
        primaryTouchPerformed = context.ReadValueAsButton();
        onPanCanceled?.Invoke();

        if (
            isOperableObject == true
            && operableComponentDescription.partsType
                == OperableComponentDescription.PartsType.TestKitHose
        )
        {
            Actions.onHoseBibDrop?.Invoke(operableObject, operableComponentDescription);
        }
        isOperableObject = false;
        operableObject = null;
        primaryTouchStartPos = Vector3.zero;
        touchStart = Vector3.zero;
    }

    private void Touch0Contact_performed(InputAction.CallbackContext context)
    {
        primaryTouchPerformed = context.ReadValueAsButton();
        if (
            isOperableObject == true
            && operableComponentDescription.partsType
                == OperableComponentDescription.PartsType.TestKitHose
        )
        {
            Actions.onHoseBibGrab?.Invoke(operableObject, operableComponentDescription);
            Debug.Log($"GRABBING");
        }
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

    private void Touch0Delta_started(InputAction.CallbackContext context)
    {
        primaryFingerDelta = new Vector2(
            (touchStart.x - Camera.main.ScreenToWorldPoint(Input.mousePosition).x),
            touchStart.y - Camera.main.ScreenToWorldPoint(Input.mousePosition).y
        );
    }

    private void Touch0Delta_canceled(InputAction.CallbackContext context) { }

    /// END INPUT
    ///-----------------------

    public void DetectObjectWithRaycast()
    {
        LayerMask layerMask = LayerMask.GetMask("OperableObject");
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit2D ray2DHit = Physics2D.Raycast(primaryTouchStartPos, Vector2.zero);
        RaycastHit hit;

        //current distance to device is about 60-70
        Physics.Raycast(ray, out hit, Mathf.Infinity, layerMask);

        //Debug.Log($"hit.collider = {hit.collider}; ray2DHit = {ray2DHit.collider}");
        ///check if anything is hit, then if something was hit, check whether it is an operable component or not
        /// (if it has an OperableComponentDescription component, then it is operable)

        if (hit.collider != null && ray2DHit.collider == null)
        {
            _operableTestGaugeObject = null;
            isOperableObject = true;
            operableComponentDescription =
                hit.collider.transform.GetComponent<OperableComponentDescription>();
            operableObject = hit.collider.transform.gameObject;

            _operableObjectRotation = operableObject.transform.rotation.eulerAngles;
        }
        else if (hit.collider == null && ray2DHit.collider != null)
        {
            operableObject = null;
            isOperableObject = true;
            operableComponentDescription =
                ray2DHit.collider.transform.GetComponent<OperableComponentDescription>();
            _operableTestGaugeObject = ray2DHit.collider.transform.gameObject;
        }
        else
        {
            _operableTestGaugeObject = null;
            operableObject = null;
            isOperableObject = false;
        }
    }

    //THIS ONLY OPERATES ASSEMBY COMPONENTS! NOT TESTGAUGE COMPONENTS
    private void Operate()
    {
        //_operableObjectRotation = _operableObject.transform.rotation.eulerAngles;

        //Vector2 primaryFingerPos = playerInput.Touchscreen.Touch0Position.ReadValue<Vector2>();
        if (operableObject != null)
        {
            if (
                operableComponentDescription.partsType
                    == OperableComponentDescription.PartsType.ShutOff
                || operableComponentDescription.partsType
                    == OperableComponentDescription.PartsType.TestCock
            )
            {
                _operableObjectRotation.z +=
                    (touchStart.x - Camera.main.ScreenToWorldPoint(Input.mousePosition).x)
                    * deviceRotSensitivity;

                //rotation clamp for parts that rotate arpund center mass (i.e. test cock valves)
                _operableObjectRotation.z = Mathf.Clamp(_operableObjectRotation.z, 0.0f, 90.0f);
                operableObject.transform.rotation = Quaternion.Euler(_operableObjectRotation);
            }
        }
    }

    private void Start() { }

    /// <summary>
    /// This will be in the update method to constantly check if the input was intended for assembly operation
    /// </summary>
    public void OperateCheck()
    {
        if (isOperableObject == true && primaryTouchStarted > 0)
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
