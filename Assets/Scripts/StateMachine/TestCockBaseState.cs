using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestCockBaseState : BaseState
{
 

  public TestCockBaseState(AssemblyStateMachine stateMachine, AssemblyStateFactory  stateFactory): base(stateMachine, stateFactory)
  {
  IsRootState = true;
  InitializeSubState();
  }

    public override void Enter()
    {
        Debug.Log($"Hello from TestCockBaseState");
    }
    public override void Tick(){
        
        CheckSwitchStates();
    }
    public override void Exit(){
        Debug.Log($"Exiting TestCockBaseState");
    }
    public override void InitializeSubState(){
       if(StateMachine.OperableObject.layer == 6){
       if(StateMachine.OperableObject.transform.rotation.eulerAngles.z == 0){
           SetSubState(StateFactory.TestCockClosed());
        }
        if(StateMachine.OperableObject.transform.rotation.eulerAngles.z > 0){
           SetSubState(StateFactory.TestCockOpen());
        }
       }
    }
    public override void CheckSwitchStates(){
        //Debug.Log($"{StateMachine.OperableObject.transform.rotation.eulerAngles}");
           if(StateMachine.OperableObject.layer == 7){
        SwitchState(StateFactory.ShutOffValve());
            }

    }

}
