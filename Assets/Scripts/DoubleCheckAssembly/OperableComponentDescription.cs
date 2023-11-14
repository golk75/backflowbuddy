using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OperableComponentDescription : MonoBehaviour
{
    public enum ComponentId
    {
        Body,
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
        BypassControl,
        HighHose,
        LowHose,
        BypassHose,
        SightTube
    }

    public enum PartsType
    {
        Housing,
        ShutOff,
        TestCock,
        CheckValve,
        TestKitValve,
        TestKitHose,
        TestKitSightTube
    }

    public ComponentId componentId;
    public PartsType partsType;
}
