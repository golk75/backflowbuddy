using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CheckValveBaseState : BaseState
{
    public CheckValveBaseState(AssemblyStateMachine stateMachine, AssemblyStateFactory  stateFactory): base(stateMachine, stateFactory){}

    public override void Enter(){}
    public override void Tick(){
        CheckSwitchStates();
    }
    public override void Exit(){}
    public override void InitializeSubState(){}
    public override void CheckSwitchStates(){}
    

}
