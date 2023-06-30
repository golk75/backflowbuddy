using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OperableParts : MonoBehaviour
{
    public enum ComponentId
    {
        ShutOffValve1,
        ShutOffValve2,
        TestCock1,
        TestCock2,
        TestCock3,
        TestCock4,
        CheckValve1,
        CheckValve2,
    }

    public enum PartsType
    {
        ShutOff,
        TestCock,
        CheckValve,
    }

    public ComponentId componentId;
    public PartsType partsType;
}
