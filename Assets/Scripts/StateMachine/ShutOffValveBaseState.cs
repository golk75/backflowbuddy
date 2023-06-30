using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShutOffValveBaseState : BaseState
{
   
    public ShutOffValveBaseState(AssemblyStateMachine stateMachine, AssemblyStateFactory stateFactory) : base(stateMachine, stateFactory)
    {
        IsRootState = true;
        InitializeSubState();
    }
    

    public override void Enter(){
        

        Debug.Log($"Hello from ShutOffValveBaseState!!");
    }
    public override void Tick(){
       
        CheckSwitchStates();
    }
    //Only calling exit if switching superstates (ie. TestCockBaseState)
    public override void Exit(){
        Debug.Log($"Exiting ShutOffValveBaseState");
    }
    public override void InitializeSubState(){
        if(StateMachine.OperableObject.layer == 7){
        //Debug.Log($"{StateMachine.OperableObject.transform.rotation.eulerAngles}");
         if(StateMachine.OperableObject.transform.rotation.eulerAngles.z == 90){
           SetSubState(StateFactory.ShutOffValveClosed());
        }
        if(StateMachine.OperableObject.transform.rotation.eulerAngles.z == 0){
           SetSubState(StateFactory.ShutOffValveOpen());
        }
        }
      
        
    }
    public override void CheckSwitchStates(){
         if(StateMachine.OperableObject.layer ==6){
            SwitchState(StateFactory.TestCock());
         }
    }
    

}
