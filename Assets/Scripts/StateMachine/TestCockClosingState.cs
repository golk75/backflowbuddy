using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestCockClosingState : TestCockBaseState
{
   public TestCockClosingState(AssemblyStateMachine stateMachine, AssemblyStateFactory  stateFactory): base(stateMachine, stateFactory){}

    public override void Enter()
    {
        Debug.Log($"Hello from TestCock Closing!!");

       
    }
    public override void Tick()
    {
            
        
    }
    public override void Exit()
    {
        
    }
}
