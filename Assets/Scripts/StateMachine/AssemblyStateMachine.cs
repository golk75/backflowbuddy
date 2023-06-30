using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using com.zibra.liquid.Manipulators;
public class AssemblyStateMachine : MonoBehaviour
{
    
    BaseState _currentState;
     AssemblyStateFactory _states;
   
   
     [SerializeField]
     GameObject _operableObject;
     PlayerInputAction _playerInput;
     Vector2 _mousePosition;
     Vector2 _clickPosition;
     Vector3 _testCockRotation;
     Vector3 _shutOffRotation;
     Vector3 _operableRotation;
     Camera _mainCamera;
    
    

     float _sceneWidth;
     float _mousePressInputValue;
     float _testCockRotationClamp;  
     float _shutOffRotationClamp;
     float _layerClamp;

     //water
     public ZibraLiquidEmitter SupplyEmitter {get; private set;}
     public float SupplyPSI {get; private set;}
     [SerializeField]
     ZibraLiquidEmitter _supplyEmitter;
     float _supplyPSI;

     [SerializeField]
     float _shutOffRotationSensitivity;
     
     bool  _clickPerformed;
     bool _isOperable;
     bool _isOperating;
     //LayerMask _layerMask;
    public enum OperbaleComponents {
    ShutOffValve_1,
    ShutOffValve_2,
    TestCock_1,
    TestCock_2,
    TestCock_3,
    TestCock_4,
    ReliefValve

}

     public BaseState CurrentState { get { return _currentState; } set { _currentState = value; }}
     
     
     public PlayerInputAction PlayerInput {get; private set;}
     public Vector2 MousePosition { get; private set; }
     public Vector2 ClickPosition { get; private set; }     
     
   
     public Vector3 OperableRotation {get {return _operableRotation;} set{_operableRotation = value;}}
     public Camera MainCamera {get; private set;}
     
    

     public GameObject OperableObject {get {return _operableObject;} set{_operableObject = value;}}

    
     public float SceneWidth {get; set;}
     public float MousePressInputValue { get; private set; }
     public float ShutOffRotationSensitivity {get; private set;}

     //Mathf.Clamp(_operableRotation.z, 0.0f, 89.0f);
     public float RotationClamp {get; private set;}
     public bool ClickPerformed { get; private set; }
     
     public bool isOperable {get; private set;}
     public bool isOperating {get {return _isOperating;} set{_isOperating = value;}}
     
     


 



  
    
    private void Awake()
    {
    _mainCamera = Camera.main;
    _playerInput = new PlayerInputAction();
    _playerInput.MouseOperate.Click.started += Click_started;
    _playerInput.MouseOperate.Click.performed += Click_performed;
    _playerInput.MouseOperate.Click.canceled += Click_cancled;
    _playerInput.MouseOperate.MousePosition.performed += MousePosition_performed;

    _states = new AssemblyStateFactory(this);
    _currentState = _states.ShutOffValve();
    _currentState.Enter();
    
    
    }

  

    
   protected void Operate()
    {
        
    _isOperating = true;
    
    //Debug.Log($"Now Operating -> {_operableObject}| rotation = {_operableRotation}| clamp = {_testCockRotationClamp}");
    _sceneWidth = Screen.width;
    _operableRotation.z += (_mousePosition.x - _clickPosition.x)* _shutOffRotationSensitivity * -1 / _sceneWidth;
    
    
    //rotation clamp for parts that rotate arpund center mass (i.e. test cock valves)
    _operableRotation.z = Mathf.Clamp(_operableRotation.z, 0.0f, 90.0f);
    _operableObject.transform.rotation = Quaternion.Euler(_operableRotation);


    }
    /*
    void CheckOperablePartsLayer(int layerMask)
    {   

        switch (layerMask)
        {
            case 6 : _currentState = _states.TestCock();
            break;
            case 7 : _currentState = _states.ShutOffValve();
            break;
            default:
            break;
        }
        
    }
    */

    protected OperbaleComponents CheckOperablePart(string tag) => tag  switch
    {
        "SO01" => OperbaleComponents.ShutOffValve_1,
        "SO02" => OperbaleComponents.ShutOffValve_2,
        "TC01" => OperbaleComponents.TestCock_1,
        "TC02" => OperbaleComponents.TestCock_2,
        "TC03" => OperbaleComponents.TestCock_3,
        "TC04" => OperbaleComponents.TestCock_4,
        "RV" => OperbaleComponents.ReliefValve,
        _ => throw new ArgumentOutOfRangeException(nameof(tag), $"Not expected tag value: {tag}"),

    };
    
    private void DetectObjectWithRaycast()
    {
        Ray ray = _mainCamera.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;
        Physics.Raycast(ray, out hit);
      
    
        if(hit.collider != null){
            
            //Debug.Log($"{hit.collider.tag} Detected|| ");
            _operableObject = hit.collider.gameObject;
            
            
            ///
            /// Check what device was clicked to determine which state to switch to-> Debug.Log($"{_clickController.Component.componentName == OperableObjects.xxxx}");");
            //
            _operableRotation = hit.collider.gameObject.transform.rotation.eulerAngles;
            _isOperable = true;
        }
        else{
            _isOperable = false;
        }


    }

    public void OperateCheck(){
        if(_mousePressInputValue != 0&&_isOperable){
            Operate();
        }
    }


    public void MousePosition_performed(InputAction.CallbackContext ctx)
    {
    _mousePosition = ctx.ReadValue<Vector2>();
    }

    public void Click_started(InputAction.CallbackContext ctx)
    {
    
    _mousePressInputValue = ctx.ReadValue<float>();
    _clickPosition = _mousePosition;
    DetectObjectWithRaycast();

    }

    
    public void Click_cancled(InputAction.CallbackContext ctx)
    {
    _mousePressInputValue = ctx.ReadValue<float>();
    _clickPosition = Vector2.zero;
    _isOperating = false;

    
    }

    public void Click_performed(InputAction.CallbackContext ctx)
    {
    
    }


   


    private void OnEnable()
    {
    _playerInput.MouseOperate.Click.Enable();
    _playerInput.MouseOperate.MousePosition.Enable();
    
    }

   
    private void OnDisable()
    {
    _playerInput.MouseOperate.Click.Disable();
    _playerInput.MouseOperate.MousePosition.Disable();
    
    }








private void Update()
{   
    
   OperateCheck();
    
    _currentState.UpdateStates();
    //_currentState.Tick();
}

 

  
   
}
 

