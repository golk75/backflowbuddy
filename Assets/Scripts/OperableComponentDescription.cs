using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OperableComponentDescription : MonoBehaviour
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
        LowBleed,
        LowControl,
        HighBleed,
        HighControl,
        BypassControl
    }

    public enum PartsType
    {
        ShutOff,
        TestCock,
        CheckValve,
        TestKitValve
    }

    public ComponentId componentId;
    public PartsType partsType;
}
