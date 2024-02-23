using System;
using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UIElements;

public class PlayerController : MonoBehaviour
{
    [SerializeField]
    GameObject TestCockManager;
    TestCockController testCockController;

    public GameObject FillButton;
    public UiClickFilter uiClickFilter;
    [SerializeField]
    GameObject WaterManager;



    PlayerInputAction playerInput;

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
    public GameObject prevOperableObject;

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
    Coroutine DelayFilterReading;
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





        //Dual Input (mouse/keyboard and touch)
        playerInput.DualMap.Touch0Contact.started += Touch0Contact_started;
        playerInput.DualMap.Touch0Contact.canceled += Touch0Contact_canceled;
        playerInput.DualMap.Touch0Contact.performed += Touch0Contact_performed;
        playerInput.DualMap.Touch0Delta.started += Touch0Delta_started;
        playerInput.DualMap.Touch0Delta.canceled += Touch0Delta_canceled;
        playerInput.DualMap.Touch1Contact.started += Touch1Contact_started;
        playerInput.DualMap.Touch1Contact.canceled += Touch1Contact_canceled;
        playerInput.DualMap.Touch0Delta.started += Touch0Delta_started;

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
    IEnumerator Delay()
    {
        yield return new WaitForSeconds(0.001f);

        ///Click/press---------------------------------------------------------------------------------
        if (operableObject != null && uiClickFilter.isUiClicked == false)
        {

            if (_operableObjectRotation.z > 0)
            {
                _operableObjectRotation.z = 0;
            }
            else if (_operableObjectRotation.z <= 0)
            {
                _operableObjectRotation.z = 90;

            }
            switch (operableComponentDescription.componentId)
            {
                case OperableComponentDescription.ComponentId.HighBleed:

                    Actions.onHighBleedOperate?.Invoke();
                    break;
                case OperableComponentDescription.ComponentId.LowBleed:

                    Actions.onLowBleedOperate?.Invoke();
                    break;
                case OperableComponentDescription.ComponentId.HighControl:

                    Actions.onHighControlOperate?.Invoke();
                    break;
                case OperableComponentDescription.ComponentId.LowControl:

                    Actions.onLowControlOperate?.Invoke();
                    break;
                case OperableComponentDescription.ComponentId.BypassControl:

                    Actions.onBypassControlOperate?.Invoke();
                    break;
                default:
                    break;
            }

        }

    }
    public void Touch0Contact_started(InputAction.CallbackContext context)
    {
        primaryClickStarted = context.ReadValue<float>();
        touchStart = Camera.main.ScreenToWorldPoint(
        //  playerInput.MouseOperate.MousePosition.ReadValue<Vector2>()
        playerInput.DualMap.Touch0Position.ReadValue<Vector2>()

        //playerInput.DualMap.Touch0Position.ReadValue<Vector2>()
        );
        primaryTouchStartPos = Camera.main.ScreenToWorldPoint(Input.mousePosition);

        if (uiClickFilter.isUiClicked == false)
        {
            DetectObjectWithRaycast();
        }


        if (uiClickFilter.isUiClicked == false)
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
        // primaryClickPerformed = context.ReadValue<float>();
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
            prevOperableObject = operableObject;
            operableObject = null;
            _operableTestGaugeObject = null;
            primaryTouchStartPos = Vector3.zero;
            touchStart = Vector3.zero;
            operableComponentDescription = null;

        }
        // uiClickFilter.isUiClicked = false;
    }

    private void Touch0Contact_performed(InputAction.CallbackContext context)
    {

        primaryClickPerformed = context.ReadValue<float>();
        if (primaryTouchPerformed == true)
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

        primaryTouchPerformed = false;
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

        DelayFilterReading = StartCoroutine(Delay());

    }

    private void Start() { }

    /// <summary>
    /// This will be in the update method to constantly check if the input was intended for assembly operation
    /// </summary>
    public void OperateCheck()
    {
        if (primaryClickStarted > 0)
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
