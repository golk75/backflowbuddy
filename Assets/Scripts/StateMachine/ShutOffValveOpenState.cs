using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShutOffValveOpenState : BaseState
{
   public ShutOffValveOpenState(AssemblyStateMachine stateMachine, AssemblyStateFactory  stateFactory): base(stateMachine, stateFactory){}

   


    public override void Enter()
    {
        Debug.Log($"Hello from ShutOffOpenState!!");
       
    }

    public override void Tick()
    {
        CheckSwitchStates();
    }
    public override void InitializeSubState(){
        
    }
    
    public override void CheckSwitchStates(){
        if(StateMachine.OperableObject.transform.eulerAngles.z == 90){
            SwitchState(StateFactory.ShutOffValveClosed());
        }
    }
    public override void Exit()
    {
        Debug.Log($"Exiting ShutOffValveOpenState");
    }

}
