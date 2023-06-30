using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestCockOpeningState : TestCockBaseState
{
  public TestCockOpeningState(AssemblyStateMachine stateMachine, AssemblyStateFactory  stateFactory): base(stateMachine, stateFactory){}

public override void Enter()
    {
        Debug.Log($"Hello from TestCockOpeningState!!");

       
    }
    public override void Tick()
    {
  /*
    OperateCheck();

    if(testCock.OperableObject.transform.rotation.eulerAngles.z >= 89.9f)
      {
        testCock.SwitchState(new TestCockOpenState(testCock));

      }  
    */    
    }
    public override void InitializeSubState(){
    
    }
    public override void CheckSwitchStates(){
      int zRotOpenThreshHold = 0;
      switch(StateMachine.OperableObject.transform.rotation.eulerAngles.z > zRotOpenThreshHold)
      {
        case true : Debug.Log($"zRotOpen");
        break;
      }
    }
    public override void Exit()
    {
        
    }
}
