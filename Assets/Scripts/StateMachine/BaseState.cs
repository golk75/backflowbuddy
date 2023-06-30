/// <summary>
/// Priginally was going to try to use this state machine to track states across
/// the assembly, however Im not sure what the states should be. There can only be 
/// one state at a time, so it cant be in a ShutOffClosed state at the same time that
/// its in TestCockOpenState.
/// 
/// </summary>
public abstract class BaseState
{
    private bool _isRootState = false; 
    private AssemblyStateMachine _stateMachine;
    private AssemblyStateFactory _stateFactory;
    private BaseState _currentSuperState;
    private BaseState _currentSubState;
    protected bool IsRootState {set{_isRootState = value;}}
    
   
    protected AssemblyStateMachine StateMachine {get {return _stateMachine;}}
    protected AssemblyStateFactory StateFactory {get {return _stateFactory;}}
    
    
    public BaseState(AssemblyStateMachine stateMachine, AssemblyStateFactory stateFactory)
    {
        _stateMachine = stateMachine;
        _stateFactory = stateFactory;
    }
 
    public abstract void Enter();
    public abstract void Tick();
    public abstract void Exit();
    public abstract void CheckSwitchStates();
    public abstract void InitializeSubState();

    public void UpdateStates(){
       Tick();
          if (_currentSubState != null) {
            
            _currentSubState.UpdateStates();
        }

    }
    protected void SwitchState(BaseState newState)
    {

        Exit();
        
        newState.Enter();


        if (_isRootState) {
            // switch current state of context
            _stateMachine.CurrentState = newState;
        } else if (_currentSuperState != null) {
            // set the current super states sub state to the new state
            _currentSuperState.SetSubState(newState);
        }

    }
    protected void SetSuperState(BaseState newSuperState){
        _currentSuperState = newSuperState;

    }
    protected void SetSubState(BaseState newSubState){
        _currentSubState = newSubState;
        newSubState.SetSuperState(this);

    }
        
}
