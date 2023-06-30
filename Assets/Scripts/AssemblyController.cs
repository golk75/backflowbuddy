using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using com.zibra.liquid.Manipulators;
using UnityEngine.InputSystem;
public class AssemblyController : MonoBehaviour
{
    //cameras
    public Camera Camera {get; private set;}

    //assembly components
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
    GameObject CheckValve1;
    [SerializeField]
    ZibraLiquidDetector CheckValve1Detector;
    
    public Vector3 Checkvalve1InitPos;
    public Vector3 Checkvalve1CurrentPos;
    public Vector3 Checkvalve1FullyOpenPos = new Vector3(-0.107f,0.0f,-0.085f);
    
    [SerializeField]
    GameObject CheckValve2;
    [SerializeField]
    ZibraLiquidDetector CheckValve2Detector;
    public Vector3 Checkvalve2InitPos;
    public Vector3 Checkvalve2CurrentPos;
    public Vector3 Checkvalve2FullyOpenPos = new Vector3(-0.202f,0.0f,-0.1776f);
   
    [SerializeField]
    GameObject _operableObject;
    [SerializeField]
    GameObject _operableValve;
    
    //input
    public PlayerInputAction PlayerInput{get; private set;}
    public Vector2 MousePosition {get; private set;}
    public Vector2 ClickPosition {get; private set;}
    public float MousePressInputValue {get; private set;}
    public float SceneWidth {get; private set;}
    [SerializeField]
    float _rotationSensitivity;
    public float RotationSensitivity {get{return _rotationSensitivity;} private set{_rotationSensitivity = value;}}
    bool isCameraMoving;
    

    

    //device operation
    public GameObject OperableObject {get{return _operableObject;} private set{_operableObject = value;}}
    public GameObject OperableValve {get{return _operableValve;} private set{_operableValve = value;}}
    public bool IsOperable {get; private set;}
    public bool IsOperating {get; private set;}
    public bool IsSupplyOn {get; private set;}
    Vector3 _operableObjectRotation;
    Vector3 _operableValveScale;
    Vector3 _startingTestCockValveScale;
    float testCockClosedYScale;
    float testCockOpenYScale = 0.005f;
    //Vector3 testCockClosedScale = new Vector3(0.00886263326f,0.0028f,0.000265074486f);
    Vector3 testCockClosedScale;
    public bool TestCockOpenCheck {get; private set;} = false;
    [SerializeField]
    float supplyVolume = 20;
    float supplyPsi;
    float checkValve1Psi;
    float checkValve2Psi;
    
  
    [SerializeField]
    private AnimationCurve curve;
    float Check1psiThreshold;
    float Check2psiThreshold;
    
    

    ///end variables
   
   /// <summary>
   /// This will run in the update to constantly check for psi against the checkvalve, this psi with determine the opening and closing of the checkvalve GO. 
   /// we will be using a zibra liquid detector component to measure this
   /// </summary>
    private void CheckValveOperation()
    {

     /// <summary>
     /// CheckVale1 Ops
     /// </summary>
     Check1psiThreshold = CheckValve1Detector.ParticlesInside * 0.001f;
     Checkvalve1CurrentPos = Vector3.Lerp(Checkvalve1InitPos,Checkvalve1FullyOpenPos, curve.Evaluate(Check1psiThreshold));
     CheckValve1.transform.localPosition = Checkvalve1CurrentPos;
     
     /// <summary>
     /// CheckVale2 Ops
     /// </summary>  
     Check2psiThreshold = CheckValve2Detector.ParticlesInside * 0.002f;
     Checkvalve2CurrentPos = Vector3.Lerp(Checkvalve2InitPos,Checkvalve2FullyOpenPos, curve.Evaluate(Check2psiThreshold));
     CheckValve2.transform.localPosition = Checkvalve2CurrentPos;

     
   
         
         
     //Debug.Log($"Zone 1 primed @ {psi}--> Check#1 can now open!");
     
 


    }


    /// <summary>
    /// Check whether the GO the user is interacting with is an operable part (ex. shutoff vs checkvalve)
    /// </summary>

    private void DetectObjectWithRaycast()
    {
     Ray ray = Camera.ScreenPointToRay(Input.mousePosition);
     RaycastHit hit;
     Physics.Raycast(ray, out hit);
     
    
        if(hit.collider != null){
             
                   
             _operableObject = hit.collider.transform.gameObject;  
             _operableObjectRotation = _operableObject.transform.rotation.eulerAngles;   
             IsOperable = true;
        }
        else{
         IsOperable = false;
        }
      
        
    }

    

    /// <summary>
    /// This will be in the update method to constantly check if the input was on an operable GO
    /// </summary>
    public void OperateCheck(){
        

        if(MousePressInputValue != 0 && IsOperable){
         Operate();  
        }
       
    }

     /// <summary>
    /// Operating operable parts
    /// Going to try using one part at a time, which is to say the user will only be able to use/ operate one GO at a time. Restricting the 
    /// user to a single operating part allows me to use a single reference and reassign this reference to the next GO being used/ operated.
    /// Will need to check partType of operable GO to determine which GO the user is trying to 
    /// operate (test cock vs. shut off vs. test kit) and how to manipulate the assigned valves, GO's transforms etc.
    /// </summary>

    protected void Operate()
    {
   
    
     var _componentId = _operableObject.GetComponent<OperableParts>().componentId;
     var _componentType = _operableObject.GetComponent<OperableParts>().partsType;
        
     _operableObjectRotation = _operableObject.transform.rotation.eulerAngles;
     IsOperating = true;
    
     SceneWidth = Screen.width;
     _operableObjectRotation.z += (MousePosition.x - ClickPosition.x)* RotationSensitivity * -1 / SceneWidth;
    
    
    //rotation clamp for parts that rotate arpund center mass (i.e. test cock valves)
     _operableObjectRotation.z = Mathf.Clamp(_operableObjectRotation.z, 0.0f, 90.0f);
     _operableObject.transform.rotation = Quaternion.Euler(_operableObjectRotation);
     float shutOffValveScaleFactor = (_operableObjectRotation.z * 0.01f) + 0.1f;
     float testCockValveScaleFactor = (_operableObjectRotation.z * 0.01f)+ 0.1f;
    
   


    /// <summary>
    /// TestCock Operation 
    /// </summary>
     if(_componentType == OperableParts.PartsType.TestCock){
        //assign the associated test cock valve game object to currently operating test cock; 
        switch (_componentId){
         case OperableParts.ComponentId.TestCock1 : 
         _operableValve = TestCockValve1;
         break;
         case OperableParts.ComponentId.TestCock2 : 
         _operableValve = TestCockValve2;
         break;
         case OperableParts.ComponentId.TestCock3 : 
         _operableValve = TestCockValve3;
         break;
         case OperableParts.ComponentId.TestCock4 : 
         _operableValve = TestCockValve4;
         break;
        }
       //check is supply is turned on before opening test cocks, so that water will not emit from the test cocks if shutoff#1 (supply) is off.
       
       _operableValveScale = _operableValve.transform.localScale;
           
       _operableValveScale.y = Mathf.Lerp(testCockClosedYScale, testCockOpenYScale, testCockValveScaleFactor);
       _operableValve.transform.localScale = _operableValveScale;


       /// <summary>
       /// Might revisit this to scale with checkvalve movement for a more realistic
       /// operation (ie. test cocks will not open if the psi upstream is not strong enough to open upstream checkvalve) 
       /// </summary>
       
       
       //testcock valve should still be able to be closed (via Void) while emitter volume scaled with supply volume 
        TestCockEmitter1.VolumePerSimTime = supply.VolumePerSimTime * 0.1f;
        TestCockEmitter2.VolumePerSimTime = supply.VolumePerSimTime * 0.1f;
        TestCockEmitter3.VolumePerSimTime = supply.VolumePerSimTime * 0.1f;
        TestCockEmitter4.VolumePerSimTime = supply.VolumePerSimTime * 0.1f;

       
       ///END TEST COCK CHECK
     }
     
        /// <summary>
        /// ShutoffValve Operation 
        /// need to differentiate ShutoffValve1's supply emiiter from ShutOffValve2's output void..
        /// </summary>
        
        else if(_componentType == OperableParts.PartsType.ShutOff){
         //assign the associated shutoff valve game object to currently operating shutoff; 
         switch (_componentId){
          case OperableParts.ComponentId.ShutOffValve1 : 
          _operableValve = ShutOffValve1;
          break;
          case OperableParts.ComponentId.ShutOffValve2 : 
          _operableValve = ShutOffValve2;
          break;
        }
           
        //ShutOffValve1's emitter operation--->
        if(_componentId == OperableParts.ComponentId.ShutOffValve1){    
         supply.VolumePerSimTime = Mathf.Lerp(supplyVolume,0,_operableObjectRotation.z/90);

        // tie test cock emitter volumes to supply volume, so that if the supply is off the test cocks can not output and visa versa 
         if(supply.VolumePerSimTime > 0){
             IsSupplyOn = true;
         } 
         else if (supply.VolumePerSimTime <= 0 ){
             IsSupplyOn = false;
         }
         TestCockEmitter1.VolumePerSimTime = supply.VolumePerSimTime * 0.1f;
         TestCockEmitter2.VolumePerSimTime = supply.VolumePerSimTime * 0.1f;
         TestCockEmitter3.VolumePerSimTime = supply.VolumePerSimTime * 0.1f;
         TestCockEmitter4.VolumePerSimTime = supply.VolumePerSimTime * 0.1f;
       
        }
        //ShutOffValve2's void operation--->
        else if(_componentId == OperableParts.ComponentId.ShutOffValve2){
         Vector3 lerpedScale = Vector3.Lerp(new Vector3(3,1,1),new Vector3(0,0,0) , shutOffValveScaleFactor);
         _operableValve.transform.localScale = lerpedScale;
        }
        /// END SHUTOFF CHECK
      }
     // END OPERATE
    }


    private void OnEnable()
    {
    PlayerInput.MouseOperate.Click.Enable();
    PlayerInput.MouseOperate.MousePosition.Enable();
    PlayerInput.MouseOperate.MoveCamera.Enable();
    }

   
    private void OnDisable()
    {
    PlayerInput.MouseOperate.Click.Disable();
    PlayerInput.MouseOperate.MousePosition.Disable();
    PlayerInput.MouseOperate.MoveCamera.Disable();
    }

     public void MousePosition_performed(InputAction.CallbackContext ctx)
    {
     MousePosition = ctx.ReadValue<Vector2>();
    }

    public void Click_started(InputAction.CallbackContext ctx)
    {
     MousePressInputValue = ctx.ReadValue<float>();
     ClickPosition = MousePosition;
     DetectObjectWithRaycast();
    }

    
    public void Click_cancled(InputAction.CallbackContext ctx)
    {
     MousePressInputValue = ctx.ReadValue<float>();
     ClickPosition = Vector2.zero;
     IsOperating = false;
    }





    private void Awake()
    {
    Camera = Camera.main;
    PlayerInput = new PlayerInputAction(); 
    PlayerInput.MouseOperate.Click.started += Click_started;
    PlayerInput.MouseOperate.Click.canceled += Click_cancled;
    PlayerInput.MouseOperate.MousePosition.performed += MousePosition_performed;

    //PlayerInput.MouseOperate.MoveCamera.started += MoveCamera_started;
    //PlayerInput.MouseOperate.MoveCamera.canceled += MoveCamera_canceled;
    Checkvalve1InitPos = CheckValve1.transform.localPosition;
    Checkvalve2InitPos = CheckValve2.transform.localPosition;
    testCockClosedScale = TestCockValve1.transform.localScale;
    testCockClosedYScale = testCockClosedScale.y;
    
    IsSupplyOn = false;

    }
    void Start()
    {
     
    }

    void Update()
    {
     
     CheckValveOperation();
     OperateCheck();   
    }

    
}
