using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestCockClosedState : TestCockBaseState 
{
public TestCockClosedState(AssemblyStateMachine stateMachine, AssemblyStateFactory  stateFactory): base(stateMachine, stateFactory){}

public override void Enter()
    {
        Debug.Log($"Entered TestCockClosedState!");

       
    }
    public override void Tick()
    {
    /*
      //Debug.Log($"{testCock.OperableObject.transform.rotation.z > 0}");
      OperateCheck();
      
      if(testCock.OperableObject.transform.rotation.eulerAngles.z > 0)
      {
        testCock.SwitchState(new TestCockOpeningState(testCock));

      }        
      */

    }
    public override void Exit()
    {
        
    }
    public override void CheckSwitchStates()
    {
       
    }
    public override void InitializeSubState()
    {
        
    }


}
