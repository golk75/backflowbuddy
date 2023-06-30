using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShutOffValveOpeningState : BaseState
{
    public ShutOffValveOpeningState(AssemblyStateMachine stateMachine, AssemblyStateFactory  stateFactory): base(stateMachine, stateFactory){}
    

    
    
    public override void Enter()
    {    
  
        
        Debug.Log($"Hello from ShutOffOpeningState!!");
    

    }
    
    public override void Tick()
    { 
       
        CheckSwitchStates();
    }
    public override void InitializeSubState(){
        
    }
  
    public override void CheckSwitchStates(){
    
     
      
    }
    
        
      
    
     public override void Exit()
    {
    
    }
}
