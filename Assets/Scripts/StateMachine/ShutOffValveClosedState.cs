using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShutOffValveClosedState : BaseState
{
    public ShutOffValveClosedState(AssemblyStateMachine stateMachine, AssemblyStateFactory  stateFactory): base(stateMachine, stateFactory){}
   

    

    public override void Enter()
    {
       //Will not log if switching here from ShutOffValveBaseState
        Debug.Log($"Hello from ShutOffClosedState!!");
        
       
    }
    
    public override void Tick()
    {  
        
        CheckSwitchStates();
      

    }
    public override void CheckSwitchStates(){
  
    //Debug.Log($"{StateMachine.OperableObject.transform.rotation.eulerAngles.z}");
    
 /* if(StateMachine.OperableObject.layer == 6){
        SwitchState(StateFactory.TestCock());
        }
    
    }
    */

    if(StateMachine.OperableObject.transform.rotation.eulerAngles.z == 0){
        SwitchState(StateFactory.ShutOffValveOpen());
    }
    }
     public override void InitializeSubState(){}
   
     public override void Exit()
    {

    }
}
