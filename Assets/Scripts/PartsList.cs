using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public struct OperatingParts
{
    public OperableComponentDescription.ComponentId ComponentId;

    public Vector3 ComponentScale;

    public OperatingParts(
        OperableComponentDescription.ComponentId componentId,
        Vector3 componentScale
    )
    {
        this.ComponentId = componentId;
        this.ComponentScale = componentScale;
    }
}
