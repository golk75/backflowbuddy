using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class MoveCamera : MonoBehaviour
{
    bool isCameraMoving;
    PlayerInputAction playerInput;
    public Vector3 ClickPosition { get; private set; }
    public Vector3 MousePosition { get; private set; }
    [SerializeField]
    public AssemblyController assemblyController;

    public void MoveCamera_started(InputAction.CallbackContext ctx){
        isCameraMoving = ctx.ReadValueAsButton();
        ClickPosition = assemblyController.MousePosition;
        StartCoroutine(moveCamera());
    }
    public void MoveCamera_canceled(InputAction.CallbackContext ctx){
        isCameraMoving = ctx.ReadValueAsButton();
        
    }
    /// <summary>
    /// This function is called when the object becomes enabled and active.
    /// </summary>
    private void OnEnable()
    {
        playerInput.MouseOperate.MoveCamera.Enable();
    }
    /// <summary>
    /// This function is called when the behaviour becomes disabled or inactive.
    /// </summary>
    private void OnDisable()
    {
        playerInput.MouseOperate.MoveCamera.Disable();
    }
   
    IEnumerator moveCamera(){
       
      while(isCameraMoving){
        //Debug.Log($"{assemblyController.MousePosition}");
        Vector3 movement = ClickPosition - transform.position;
       
        transform.position =  movement;
        
        yield return null;
      }
      
    }
    void Awake()
    {//
        playerInput = new PlayerInputAction();
        playerInput.MouseOperate.MoveCamera.started += MoveCamera_started;
        playerInput.MouseOperate.MoveCamera.canceled += MoveCamera_canceled;
    }
    // Start is called before the first frame update
    void Start()
    {
  
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
