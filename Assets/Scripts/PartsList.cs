using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;



[System.Serializable]
public struct OperatingParts
{    
   
    public OperableParts.ComponentId ComponentId;
    
    public Vector3 ComponentScale;
    public OperatingParts(OperableParts.ComponentId componentId, Vector3 componentScale){
        this.ComponentId = componentId;
        this.ComponentScale = componentScale;
    }
}

