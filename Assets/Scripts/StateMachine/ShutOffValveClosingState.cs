using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShutOffValveClosingState : BaseState
{
    public ShutOffValveClosingState(AssemblyStateMachine stateMachine, AssemblyStateFactory  stateFactory): base(stateMachine, stateFactory){}
   


    
    
    public override void Enter()
    {
        
        Debug.Log($"Hello from ShutOffClosingState!!");
    }
    
     public override void Tick()
    {
        CheckSwitchStates();
    }
 public override void InitializeSubState(){
        
    }
  
    public override void CheckSwitchStates(){
        if(StateMachine.OperableObject.transform.rotation.eulerAngles.z == 0)
        {
            SwitchState(StateFactory.ShutOffValveClosing());
        }
    }
     public override void Exit()
    {
     
    }
}
