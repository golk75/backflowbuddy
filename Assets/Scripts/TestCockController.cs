using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestCockController : MonoBehaviour
{

    public enum TestCockId{
        TestCock1,
        TestCock2,
        TestCock3,
        TestCock4,
    }
    public enum TestCockState{
        Closed,
        Open
    }

    TestCockState currentTestCockState;
    
    [SerializeField]
    AssemblyController _assemblyController;
    public AssemblyController AssemblyController {get{return _assemblyController;} set{_assemblyController = value;}}
    
    
    [SerializeField] 
    TestCockId _id;
    public TestCockId Id {get{return _id;} set{_id = value;}}
    [SerializeField] 
    TestCockState _testCockCurrentState;
    public TestCockState TestCockCurrentState {get{return _testCockCurrentState;} set{_testCockCurrentState = value;}}

    
    void Start()
    {
       
      
    }

  

    public void CheckState()
    {
        switch (transform.rotation.eulerAngles.z)
        {
            case 90 : _testCockCurrentState = TestCockState.Open;
            break;
            case 0 : _testCockCurrentState = TestCockState.Closed;
            break;
            default:
            break;
        }
    }
    // Update is called once per frame
    void Update()
    {
       
    }
}
