using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using com.zibra.liquid.DataStructures;
using com.zibra.liquid.Manipulators;

public class AssemblyController_Touch : MonoBehaviour
{
    private PlayerInputAction playerInput;

    //assembly components
    [SerializeField]
    ZibraLiquidSolverParameters liquidSolver;

    [SerializeField]
    List<float> tcPressures;

    [SerializeField]
    GameObject ShutOffValve1;

    [SerializeField]
    ZibraLiquidEmitter supply;

    [SerializeField]
    ZibraLiquidDetector supplyDetector;

    [SerializeField]
    GameObject ShutOffValve2;

    [SerializeField]
    GameObject TestCockValve1;

    [SerializeField]
    GameObject TestCockValve2;

    [SerializeField]
    GameObject TestCockValve3;

    [SerializeField]
    GameObject TestCockValve4;

    [SerializeField]
    ZibraLiquidEmitter TestCockEmitter1;

    [SerializeField]
    ZibraLiquidEmitter TestCockEmitter2;

    [SerializeField]
    ZibraLiquidEmitter TestCockEmitter3;

    [SerializeField]
    ZibraLiquidEmitter TestCockEmitter4;

    [SerializeField]
    ZibraLiquidDetector TestCockDetector1;

    [SerializeField]
    ZibraLiquidDetector TestCockDetector2;

    [SerializeField]
    ZibraLiquidDetector TestCockDetector3;

    [SerializeField]
    ZibraLiquidDetector TestCockDetector4;

    [SerializeField]
    ZibraLiquidForceField TestCockFF1;

    [SerializeField]
    ZibraLiquidForceField TestCockFF2;

    [SerializeField]
    ZibraLiquidForceField TestCockFF3;

    [SerializeField]
    ZibraLiquidForceField TestCockFF4;

    ZibraLiquidForceField _operableTestCockFF;
    public ZibraLiquidForceField OperableTestCockFF
    {
        get { return _operableTestCockFF; }
        set { _operableTestCockFF = value; }
    }

    [SerializeField]
    GameObject CheckValve1;

    [SerializeField]
    ZibraLiquidDetector CheckValve1Detector;

    public Vector3 Checkvalve1InitPos;
    public Vector3 Checkvalve1CurrentPos;
    public Vector3 Checkvalve1FullyOpenPos = new Vector3(-0.107f, 0.0f, -0.085f);

    [SerializeField]
    GameObject CheckValve2;

    [SerializeField]
    ZibraLiquidDetector CheckValve2Detector;
    public Vector3 Checkvalve2InitPos;
    public Vector3 Checkvalve2CurrentPos;
    public Vector3 Checkvalve2FullyOpenPos = new Vector3(-0.202f, 0.0f, -0.1776f);

    [SerializeField]
    GameObject _operableObject;

    [SerializeField]
    GameObject _operableValve;

    //camera
    public float SceneWidth { get; private set; }
    Coroutine zoomCoroutine;
    Coroutine panningCoroutine;
    bool isZooming;
    bool isPanningCamera;

    [SerializeField]
    float maxZoom;

    [SerializeField]
    float minZoom;

    [Range(0, 0.01f)]
    [SerializeField]
    float zoomingSpeed = 0.001f;

    [SerializeField]
    float panningSpeed;

    [SerializeField]
    float panningLeftBoundry = -15.0f;

    [SerializeField]
    float panningRightBoundry = 23.0f;

    [SerializeField]
    float panningTopBoundry = 4.0f;

    [SerializeField]
    float panningBottomBoundry = 0.1f;

    [SerializeField]
    float zoomFactor;
    private Vector2 lastPosition;
    private Vector2 centerPoint;
    private Vector3 targetPanPos;
    private Vector3 touchStart;

    //assembly controller
    [SerializeField]
    float _rotationSensitivity;
    public float RotationSensitivity
    {
        get { return _rotationSensitivity; }
        private set { _rotationSensitivity = value; }
    }

    //input (touch)
    Vector2 touch0Delta;

    bool isCameraMoving;

    //device operation
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
    public Vector2 MousePosition { get; private set; }
    public Vector2 ClickPosition { get; private set; }
    Vector2 primaryFingerPos;
    public bool IsOperable { get; private set; }
    public bool IsOperating { get; private set; }
    public bool IsSupplyOn { get; private set; }
    Vector3 _operableObjectRotation;
    Vector3 _operableValveScale;
    Vector3 _startingTestCockValveScale;
    float testCockClosedYScale;
    public float TestCockValveScaleFactor
    {
        get { return _testCockValveScaleFactor; }
        private set { _testCockValveScaleFactor = value; }
    }
    float _testCockValveScaleFactor;

    public float testCockOpenYScale = 0.005f;

    //Vector3 testCockClosedScale = new Vector3(0.00886263326f,0.0028f,0.000265074486f);
    Vector3 testCockClosedScale;
    public bool TestCockOpenCheck { get; private set; } = false;

    [SerializeField]
    float supplyVolume = 60;
    float supplyPsi;
    float checkValve1Psi;
    float checkValve2Psi;

    [SerializeField]
    private AnimationCurve curve;
    float Check1psiThreshold;
    float Check2psiThreshold;

    private void Awake()
    {
        Debug.Log($"AssemblyController_Touch Enabled...");
        SceneWidth = Screen.width;

        playerInput = new PlayerInputAction();
        Checkvalve1InitPos = CheckValve1.transform.localPosition;
        Checkvalve2InitPos = CheckValve2.transform.localPosition;
        testCockClosedScale = TestCockValve1.transform.localScale;
        testCockClosedYScale = testCockClosedScale.y;

        //touch input

        //Touch0
        playerInput.Touchscreen.Touch0Contact.started += Touch0Contact_started;
        playerInput.Touchscreen.Touch0Contact.canceled += Touch0Contact_canceled;
        playerInput.Touchscreen.Touch0Delta.started += Touch0Delta_started;
        playerInput.Touchscreen.Touch0Delta.canceled += Touch0Delta_canceled;
        //Touch1
        playerInput.Touchscreen.Touch1Contact.started += Zoom_started;
        playerInput.Touchscreen.Touch1Contact.canceled += Zoom_canceled;
    }

    /// 2023-06-19 : Touch input issue to note: When trying to use the assembly operation logic from AssemblyController.cs > how do I translate input from touch to mouse? Do I need to add inputs from here over/on top of
    /// mouse inputs in the AssemblyController.Operate()? Or is there an api that translates mouse to touch? or is there a way to check whether a device is using touch vs. mouse, then switch from mouse logic
    /// to touch input logic in the AssemblyController.Operate()?
    ///
    /// After a little google search, I'm seeing more along the lines of checking the device type and switching logic accordingly (i.e. if())
    ///

    /// <summary>
    /// 6/20/23 TODO : Figure out why StopCoroutine(panningCoroutine) is not stopping my coroutine!
    /// </summary>
    ///
    ///
    ///




    void OnEnable()
    {
        //cameraTransform.LookAt(this.transform);

        playerInput.Enable();

        /// </summary>
    }

    void OnDisable()
    {
        playerInput.Disable();
        Debug.Log($"AssemblyController_Touch Disabled...");
    }

    private void Touch0Delta_started(InputAction.CallbackContext context)
    {
        if (IsOperable)
        {
            touch0Delta = context.ReadValue<Vector2>();
        }
    }

    private void Touch0Delta_canceled(InputAction.CallbackContext context)
    {
        //checking if a panningCoroutine was created yet (i.e. if an operable component is pressed before anything else)
    }

    private void Touch0Contact_started(InputAction.CallbackContext context)
    {
        //Debug.Log($"Primary finger now pressing down");
        //touchStart = Camera.main.ScreenToWorldPoint(context.ReadValue<Vector2>());
        touchStart = Camera.main.ScreenToWorldPoint(
            playerInput.Touchscreen.Touch0Position.ReadValue<Vector2>()
        );
        DetectObjectWithRaycast();
    }

    private void Touch0Contact_canceled(InputAction.CallbackContext context)
    {
        if (panningCoroutine != null)
        {
            //Debug.Log($"Panning Stopped");
            StopCoroutine(panningCoroutine);
        }
        IsOperable = false;
        _operableObject = null;
        _operableValve = null;
        isPanningCamera = false;
        touchStart = Vector3.zero;
        _operableObjectRotation = Vector3.zero;
        //Debug.Log($"Primary UNpressed");
    }

    private void Zoom_started(InputAction.CallbackContext context)
    {
        zoomCoroutine = StartCoroutine(ZoomDetection());
        isZooming = true;
    }

    private void Zoom_canceled(InputAction.CallbackContext context)
    {
        if (isZooming == true)
        {
            StopCoroutine(zoomCoroutine);
            isZooming = false;
        }
    }

    IEnumerator ZoomDetection()
    {
        float prevDistance = Vector2.Distance(
            playerInput.Touchscreen.Touch0Position.ReadValue<Vector2>(),
            playerInput.Touchscreen.Touch1Position.ReadValue<Vector2>()
        );

        float distance = 0f;
        // touchStart = Camera.main.ScreenToWorldPoint(Input.mousePosition);


        while (true)
        {
            //isZooming = true;
            /*
            cnterPoint = Vector2.Lerp(
                primaryTouchPos.ReadValue<Vector2>(),
                secondaryTouchPos.ReadValue<Vector2>(),
                0.5f
            );
            targetPanPos = touchStart - Camera.main.ScreenToWorldPoint(centerPoint);
            Camera.main.transform.position += targetPanPos;
            */
            distance = Vector2.Distance(
                playerInput.Touchscreen.Touch0Position.ReadValue<Vector2>(),
                playerInput.Touchscreen.Touch1Position.ReadValue<Vector2>()
            );
            /*Debug.Log(
                $"deltaPrimary = {playerInput.Touchscreen.deltaPrimary.ReadValue<Vector2>()} | delrtaSecondary = {playerInput.Touchscreen.deltaSecondary.ReadValue<Vector2>()}"
            );*/
            /*
            Debug.Log(
                $"dot = {Vector2.Dot(playerInput.Touchscreen.deltaPrimary.ReadValue<Vector2>(), playerInput.Touchscreen.deltaPrimary.ReadValue<Vector2>())}"
            );
            */





            //zoom in
            if (distance > prevDistance)
            {
                //Camera.main.orthographicSize -= distance / zoomFactor;
                Camera.main.orthographicSize -= distance * zoomingSpeed;
                if (Camera.main.orthographicSize <= maxZoom)
                {
                    Camera.main.orthographicSize = maxZoom;
                }
            }
            //zoom out
            else if (prevDistance > distance)
            {
                //Camera.main.orthographicSize += distance / zoomFactor;
                //Camera.main.orthographicSize += distance * zoomingSpeed;
                Camera.main.orthographicSize += distance * zoomingSpeed;

                if (Camera.main.orthographicSize >= minZoom)
                {
                    Camera.main.orthographicSize = minZoom;
                }

                //Debug.Log($"Zoomin out");
            }
            //keeps track of previous distance for next loop
            prevDistance = distance;

            yield return null;
        }
    }

    /// <summary>
    /// Check whether the GO the user is interacting with is an operable part (ex. shutoff vs checkvalve)
    /// </summary>

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

    /// <summary>
    /// Operating operable parts
    /// Going to try using one part at a time, which is to say the user will only be able to use/ operate one GO at a time. Restricting the
    /// user to a single operating part allows me to use a single reference and reassign this reference to the next GO being used/ operated.
    /// Will need to check partType of operable GO to determine which GO the user is trying to
    /// operate (test cock vs. shut off vs. test kit) and how to manipulate the assigned valves, GO's transforms etc.
    /// </summary>

    public void Operate()
    {
        var _componentId = _operableObject.GetComponent<OperableComponentDescription>().componentId;
        var _componentType = _operableObject.GetComponent<OperableComponentDescription>().partsType;

        _operableObjectRotation = _operableObject.transform.rotation.eulerAngles;
        IsOperating = true;

        /*
        _operableObjectRotation.z +=
            (MousePosition.x - ClickPosition.x) * RotationSensitivity * -1 / SceneWidth;
        */
        Vector2 primaryFingerPos = playerInput.Touchscreen.Touch0Position.ReadValue<Vector2>();
        _operableObjectRotation.z +=
            (touchStart.x - Camera.main.ScreenToWorldPoint(Input.mousePosition).x)
            * RotationSensitivity;
        //Debug.Log($"{Camera.main.ScreenToViewportPoint(Input.mousePosition).x}|");
        //(touch0Delta.x - touchStart.x) * RotationSensitivity * -1 / SceneWidth;

        //rotation clamp for parts that rotate arpund center mass (i.e. test cock valves)
        _operableObjectRotation.z = Mathf.Clamp(_operableObjectRotation.z, 0.0f, 90.0f);
        _operableObject.transform.rotation = Quaternion.Euler(_operableObjectRotation);
        float shutOffValveScaleFactor = (_operableObjectRotation.z * 0.01f) + 0.1f;
        _testCockValveScaleFactor = (_operableObjectRotation.z * 0.01f) + 0.1f;

        /// <summary>
        /// TestCock Operation
        /// </summary>
        if (_componentType == OperableComponentDescription.PartsType.TestCock)
        {
            //assign the associated test cock valve game object to currently operating test cock;
            switch (_componentId)
            {
                case OperableComponentDescription.ComponentId.TestCock1:
                    _operableValve = TestCockValve1;
                    _operableTestCockFF = TestCockFF1;

                    break;
                case OperableComponentDescription.ComponentId.TestCock2:
                    _operableValve = TestCockValve2;
                    _operableTestCockFF = TestCockFF2;
                    break;
                case OperableComponentDescription.ComponentId.TestCock3:
                    _operableValve = TestCockValve3;
                    _operableTestCockFF = TestCockFF3;
                    break;
                case OperableComponentDescription.ComponentId.TestCock4:
                    _operableValve = TestCockValve4;
                    _operableTestCockFF = TestCockFF4;
                    break;
            }

            _operableValveScale = _operableValve.transform.localScale;

            _operableValveScale.y = Mathf.Lerp(
                testCockClosedYScale,
                testCockOpenYScale,
                _testCockValveScaleFactor
            );
            _operableValve.transform.localScale = _operableValveScale;

            /// <summary>
            /// Might revisit this to scale with checkvalve movement for a more realistic
            /// operation (ie. test cocks will not open if the psi upstream is not strong enough to open upstream checkvalve)
            /// </summary>


            //testcock valve should still be closable (via Void) while emitter volume scaled with supply volume

            // manipulate forcefield on test cock


            //_operableTestCockFF.Strength = Mathf.Lerp(0, 1f, testCockValveScaleFactor);

            ///END TEST COCK CHECK
        }
        /// <summary>
        /// ShutoffValve Operation
        /// need to differentiate ShutoffValve1's supply emiiter from ShutOffValve2's output void..
        /// </summary>

        else if (_componentType == OperableComponentDescription.PartsType.ShutOff)
        {
            //assign the associated shutoff valve game object to currently operating shutoff;
            switch (_componentId)
            {
                case OperableComponentDescription.ComponentId.ShutOffValve1:
                    _operableValve = ShutOffValve1;
                    break;
                case OperableComponentDescription.ComponentId.ShutOffValve2:
                    _operableValve = ShutOffValve2;
                    break;
            }

            //ShutOffValve1's emitter operation--->
            if (_componentId == OperableComponentDescription.ComponentId.ShutOffValve1)
            {
                supply.VolumePerSimTime = Mathf.Lerp(
                    supplyVolume,
                    0,
                    _operableObjectRotation.z / 90
                );

                // tie test cock emitter volumes to Assembly volume filled (using detectors), so that if the supply is off the test cocks can not output and visa versa
                if (supply.VolumePerSimTime > 0)
                {
                    IsSupplyOn = true;
                }
                else if (supply.VolumePerSimTime <= 0)
                {
                    IsSupplyOn = false;
                }
            }
            //ShutOffValve2's void operation--->
            else if (_componentId == OperableComponentDescription.ComponentId.ShutOffValve2)
            {
                Vector3 lerpedScale = Vector3.Lerp(
                    new Vector3(3, 1, 1),
                    new Vector3(0, 0, 0),
                    shutOffValveScaleFactor
                );
                _operableValve.transform.localScale = lerpedScale;
            }
            /// END SHUTOFF CHECK
        }

        // END OPERATE
    }

    private void CameraPanning()
    {
        Vector3 difference =
            touchStart
            - Camera.main.ScreenToWorldPoint(
                playerInput.Touchscreen.Touch0Position.ReadValue<Vector2>()
            );
        Camera.main.transform.position += difference;
        //placing bouneries on camera panning
        if (Camera.main.transform.position.x < panningLeftBoundry)
        {
            Camera.main.transform.position = new Vector3(
                panningLeftBoundry,
                Camera.main.transform.position.y,
                Camera.main.transform.position.z
            );
        }
        else if (Camera.main.transform.position.x > panningRightBoundry)
        {
            Camera.main.transform.position = new Vector3(
                panningRightBoundry,
                Camera.main.transform.position.y,
                Camera.main.transform.position.z
            );
        }
        if (Camera.main.transform.position.y > panningTopBoundry)
        {
            Camera.main.transform.position = new Vector3(
                Camera.main.transform.position.x,
                panningTopBoundry,
                Camera.main.transform.position.z
            );
        }
        else if (Camera.main.transform.position.y < panningBottomBoundry)
        {
            Camera.main.transform.position = new Vector3(
                Camera.main.transform.position.x,
                panningBottomBoundry,
                Camera.main.transform.position.z
            );
        }
    }

    /// <summary>
    /// This will be in the update method to constantly check if the input was intended for assembly operation or camera/ui operation
    /// </summary>
    public void OperateCheck()
    {
        if (IsOperable)
        {
            Operate();
        }
        else if (_operableObject == null && isZooming != true && isPanningCamera)
        {
            //Debug.Log($"panning camera!");
            CameraPanning();
        }
        else if (isZooming) { }
        else
        {
            //Debug.Log($"Idle");
        }
    }

    void Update()
    {
        OperateCheck();
    }
}
