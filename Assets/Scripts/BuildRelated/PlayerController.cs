using System;

using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UIElements;

public class PlayerController : MonoBehaviour
{
    [SerializeField]
    GameObject TestCockManager;
    TestCockController testCockController;
    public DoubleCheckTestKitController doubleCheckTestKitController;
    public GameObject FillButton;
    public UiClickFilter uiClickFilter;
    [SerializeField]
    GameObject WaterManager;

    private WaterController waterController;

    PlayerInputAction playerInput;
    public InputAction Touch0Position;
    public Vector3 touchStart;

    public Vector2 primaryTouchStartPos;

    //Camera events
    public static event Action onZoomStop;

    public static event Action OnPanCanceled;

    public bool isOperableObject = false;
    public float primaryClickPerformed;
    public float primaryClickStarted;
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

    public GameObject operableObject;

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
    public bool isMouseDown;
    public bool ClickOperationEnabled;
    public UIDocument root;
    public Toggle toggle;
    public bool clickPerformed;
    // Start is called before the first frame update
    void Awake()
    {
        toggle = root.rootVisualElement.Q<Toggle>("ClickEnable_toggle");
        if (toggle.value == true)
        {
            ClickOperationEnabled = true;
        }
        playerInput = new PlayerInputAction();

        testCockController = TestCockManager.GetComponent<TestCockController>();
        waterController = WaterManager.GetComponent<WaterController>();

#if UNITY_EDITOR
        Debug.Log("Unity Editor");
#endif

#if UNITY_IOS
        Debug.Log("iOS");

        //Touch Input
        operableObject = initialOperableObject;
        playerInput.Touchscreen.Touch0Contact.started += Touch0Contact_started;
        playerInput.Touchscreen.Touch0Contact.canceled += Touch0Contact_canceled;
        playerInput.Touchscreen.Touch0Contact.performed += Touch0Contact_performed;
        playerInput.Touchscreen.Touch0Delta.started += Touch0Delta_started;
        playerInput.Touchscreen.Touch0Delta.canceled += Touch0Delta_canceled;
        playerInput.Touchscreen.Touch1Contact.started += Touch1Contact_started;
        playerInput.Touchscreen.Touch1Contact.canceled += Touch1Contact_canceled;
        playerInput.Touchscreen.Touch0Delta.started += Touch0Delta_started;
        Touch0Position = playerInput.Touchscreen.Touch0Position;
#endif

#if UNITY_STANDALONE_OSX
        Debug.Log("Standalone OSX");
         playerInput.MouseOperate.Click.started += LeftMouseClick_started;
         playerInput.MouseOperate.Click.canceled += LeftMouseClick_canceled;
         playerInput.MouseOperate.Click.performed += LeftMouseClick_performed;

#endif

#if UNITY_STANDALONE_WIN
      Debug.Log("Standalone Windows");
          //Mouse Input
         playerInput.MouseOperate.Click.started += LeftMouseClick_started;
         playerInput.MouseOperate.Click.canceled += LeftMouseClick_canceled;
         playerInput.MouseOperate.Click.performed += LeftMouseClick_performed;
#endif



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


    private void LeftMouseClick_started(InputAction.CallbackContext context)
    {
        primaryClickStarted = context.ReadValue<float>();
        touchStart = Camera.main.ScreenToWorldPoint(
            playerInput.MouseOperate.MousePosition.ReadValue<Vector2>()
        );
        primaryTouchStartPos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        primaryClickPerformed = context.ReadValue<float>();
        if (uiClickFilter.isUiClicked == false)
            DetectObjectWithRaycast();
    }


    private void LeftMouseClick_canceled(InputAction.CallbackContext context)
    {
        primaryClickStarted = context.ReadValue<float>();
        primaryClickPerformed = context.ReadValue<float>();
        primaryTouchPerformed = context.ReadValueAsButton();
        OnPanCanceled?.Invoke();
        if (operableComponentDescription != null)
        {
            if (
                 // isOperableObject == true
                 operableComponentDescription.partsType
                    == OperableComponentDescription.PartsType.TestKitHose
                    ||
                    operableComponentDescription.partsType
                    == OperableComponentDescription.PartsType.TestKitSightTube
            )
            {
                Actions.onComponentDrop?.Invoke(operableObject, operableComponentDescription);
            }

            isOperableObject = false;
            operableObject = null;
            _operableTestGaugeObject = null;
            primaryTouchStartPos = Vector3.zero;
            touchStart = Vector3.zero;
            operableComponentDescription = null;

        }
        uiClickFilter.isUiClicked = false;
    }


    private void LeftMouseClick_performed(InputAction.CallbackContext context)
    {
        primaryTouchPerformed = context.ReadValueAsButton();
        if (uiClickFilter.isUiClicked == false || uiClickFilter.isUiHovered == false)
            if (operableComponentDescription != null)
            {
                if (
                     // isOperableObject == true
                     operableComponentDescription.partsType
                        == OperableComponentDescription.PartsType.TestKitHose
                )
                {
                    Actions.onComponentGrab?.Invoke(operableObject, operableComponentDescription);
                }
                if (
                   //   isOperableObject == true
                   operableComponentDescription.partsType
                      == OperableComponentDescription.PartsType.TestKitSightTube
              )
                {
                    Actions.onSightTubeGrab?.Invoke(operableObject);
                }
                if (ClickOperationEnabled == true)
                {
                    ClickOperate();

                }
            }
    }

    /// <summary>
    ///Input-------------------
    /// </summary>

    public void Touch0Contact_started(InputAction.CallbackContext context)
    {
        primaryClickStarted = context.ReadValue<float>();
        touchStart = Camera.main.ScreenToWorldPoint(
            playerInput.MouseOperate.MousePosition.ReadValue<Vector2>()
        );
        primaryTouchStartPos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        DetectObjectWithRaycast();

        if (operableComponentDescription != null && uiClickFilter.isUiClicked == false)
        {
            ///Click/press and drag-----------------------------------------------------------------------
            if (ClickOperationEnabled == false)
            {
                _operableObjectRotation.z +=
                    (touchStart.x - Camera.main.ScreenToWorldPoint(Input.mousePosition).x)
                    * deviceRotSensitivity
                    * -1;

                //rotation clamp for parts that rotate around center mass(i.e.test cock valves)

                _operableObjectRotation.z = Mathf.Clamp(_operableObjectRotation.z, 0.0f, 90.0f);
                ///End Click/press and drag--------------------------------------------------------------------
            }
            else if (ClickOperationEnabled == true)
            {
                ClickOperate();

            }

        }
    }

    private void Touch0Contact_canceled(InputAction.CallbackContext context)
    {
        primaryClickStarted = context.ReadValue<float>();
        primaryClickPerformed = context.ReadValue<float>();
        OnPanCanceled?.Invoke();
        if (operableComponentDescription != null && uiClickFilter.isUiClicked == false)
        {
            if (
                 // isOperableObject == true
                 operableComponentDescription.partsType
                    == OperableComponentDescription.PartsType.TestKitHose
                         ||
                    operableComponentDescription.partsType
                    == OperableComponentDescription.PartsType.TestKitSightTube
            )
            {
                Actions.onComponentDrop?.Invoke(operableObject, operableComponentDescription);
            }



            isOperableObject = false;
            operableObject = null;
            _operableTestGaugeObject = null;
            primaryTouchStartPos = Vector3.zero;
            touchStart = Vector3.zero;
            operableComponentDescription = null;

        }
        uiClickFilter.isUiClicked = false;
    }

    private void Touch0Contact_performed(InputAction.CallbackContext context)
    {

        primaryClickPerformed = context.ReadValue<float>();
        if (primaryTouchPerformed == false)
        {
            primaryTouchPerformed = true;
        }
        else
        {
            primaryTouchPerformed = false;
        }

    }

    private void Touch1Contact_started(InputAction.CallbackContext context)
    {

        secondaryTouchStarted = context.ReadValueAsButton();
    }

    private void Touch1Contact_canceled(InputAction.CallbackContext context)
    {
        secondaryTouchStarted = context.ReadValueAsButton();


        onZoomStop?.Invoke();
    }

    private void Touch0Delta_started(InputAction.CallbackContext context)
    {
        primaryFingerDelta = new Vector2(
            touchStart.x - Camera.main.ScreenToWorldPoint(Input.mousePosition).x,
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

        ///check if anything is hit, then if something was hit, check whether it is an operable component or not
        /// (if it has an OperableComponentDescription component, then it is operable)
        Physics.Raycast(ray, out hit, Mathf.Infinity, layerMask);

        if (hit.collider != null && ray2DHit.collider == null)
        {



            isOperableObject = true;
            //This is to differentiate between operable component types if an operable component is pressed/ clicked
            if (hit.collider.transform.GetComponent<OperableComponentDescription>())
            {
                operableComponentDescription =
                    hit.collider.transform.GetComponent<OperableComponentDescription>();
            }

            operableObject = hit.collider.transform.gameObject;

            _operableObjectRotation = operableObject.transform.rotation.eulerAngles;
        }

        else
        {
            _operableTestGaugeObject = null;
            operableObject = null;
            isOperableObject = false;
            operableComponentDescription = null;
        }
    }

    //THIS ONLY OPERATES ASSEMBY COMPONENTS! NOT TESTGAUGE COMPONENTS
    private void Operate()
    {

        if (operableObject != null)
        {

            if (
                 // isOperableObject == true
                 operableComponentDescription.partsType
                    == OperableComponentDescription.PartsType.TestKitHose
                || operableComponentDescription.partsType
                    == OperableComponentDescription.PartsType.TestKitSightTube
            )
            {
                Actions.onComponentGrab?.Invoke(operableObject, operableComponentDescription);
            }

        }
        else
        {

        }
    }
    private void ClickOperate()
    {
        ///Click/press---------------------------------------------------------------------------------
        if (OperableObject != null)
        {

            if (_operableObjectRotation.z > 0)
            {
                _operableObjectRotation.z = 0;
            }
            else if (_operableObjectRotation.z <= 0)
            {
                _operableObjectRotation.z = 90;

            }
            if (operableComponentDescription.componentId == OperableComponentDescription.ComponentId.HighBleed)
            {

                Actions.onHighBleedOperate?.Invoke();
            }
        }



        ///End Click/press------------------------------------------------------------------------------
    }

    private void Start() { }

    /// <summary>
    /// This will be in the update method to constantly check if the input was intended for assembly operation
    /// </summary>
    public void OperateCheck()
    {
        if (primaryClickPerformed > 0)
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
