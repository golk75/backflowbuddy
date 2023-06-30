using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OperableParts : MonoBehaviour
{
   
   
    enum here {
        hi
    }
    public enum ComponentId
    {
        ShutOffValve1,
        ShutOffValve2,
        TestCock1,
        TestCock2,
        TestCock3,
        TestCock4       
    }

    public enum PartsType{
        ShutOff,
        TestCock
    }
    
    public ComponentId  componentId;
    public PartsType partsType;
}
