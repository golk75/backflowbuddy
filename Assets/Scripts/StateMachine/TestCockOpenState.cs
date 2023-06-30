using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestCockOpenState : TestCockBaseState
{
  public TestCockOpenState(AssemblyStateMachine stateMachine, AssemblyStateFactory  stateFactory): base(stateMachine, stateFactory){}

public override void Enter()
    {
        Debug.Log($"Hello from TestCock Open!!");

       
    }
    public override void Tick()
    {
            
        
    }
    public override void Exit()
    {
        
    }

}
