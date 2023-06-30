

public class AssemblyStateFactory {

    AssemblyStateMachine _currentState;
  
    public AssemblyStateFactory(AssemblyStateMachine currentState){
        _currentState = currentState;
       

    }

    
    public BaseState TestCock(){
        return new TestCockBaseState(_currentState, this);
    }
    public BaseState TestCockOpening(){
        return new TestCockOpeningState(_currentState, this);
    }
    public BaseState TestCockOpen(){
        return new TestCockOpenState(_currentState, this);
    }
    public BaseState TestCockClosing(){
        return new TestCockClosingState(_currentState, this);
    }
    public BaseState TestCockClosed(){
        return new TestCockClosedState(_currentState, this);
    }
    public BaseState CheckValve(){
        return new CheckValveBaseState(_currentState, this);
    }
      public BaseState CheckValveOpening(){
        return new CheckValveOpeningState(_currentState, this);
    }
    public BaseState CheckValveOpen(){
        return new CheckValveOpenState(_currentState, this);
    }
    public BaseState CheckValveClosing(){
        return new CheckValveClosingState(_currentState, this);
    }
    public BaseState CheckValveClosed(){
        return new CheckValveClosedState(_currentState, this);
    }
    public BaseState ShutOffValve(){
        return new ShutOffValveBaseState(_currentState, this);
    }
      public BaseState ShutOffValveOpening(){
        return new ShutOffValveOpeningState(_currentState, this);
    }
    public BaseState ShutOffValveOpen(){
        return new ShutOffValveOpenState(_currentState, this);
    }
    public BaseState ShutOffValveClosing(){
        return new ShutOffValveClosingState(_currentState, this);
    }
    public BaseState ShutOffValveClosed(){
        return new ShutOffValveClosedState(_currentState, this);
    }
  
}

