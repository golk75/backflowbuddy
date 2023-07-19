using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using com.zibra.liquid.Manipulators;
using com.zibra.liquid.Analytics;
using com.zibra.liquid.Solver;

public class FillButton : MonoBehaviour
{
    ZibraLiquid liquid;

    public void FillDevice()
    {
        liquid.InitialState = ZibraLiquid.InitialStateType.BakedLiquidState;
    }

    // Start is called before the first frame update
    void Start() { }

    // Update is called once per frame
    void Update() { }
}
