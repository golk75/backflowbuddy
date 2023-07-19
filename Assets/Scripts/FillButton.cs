using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using com.zibra.liquid.Solver;

public class FillButton : MonoBehaviour
{
    public ZibraLiquid liquid;
    public GameObject Check1;
    public GameObject Check2;

    // Start is called before the first frame update
    void Start() { }

    public void FillDevice()
    {
        liquid.ReleaseSimulation();
        liquid.InitialState = ZibraLiquid.InitialStateType.BakedLiquidState;
        liquid.enabled = false;
        liquid.enabled = true;
        liquid.InitializeSimulation();

        Check1.transform.localPosition = new Vector3(-0.10f, 0, -0.08f);
        Check2.transform.localPosition = new Vector3(-0.20f, -2.25f, -0.17f);
    }

    // Update is called once per frame
    void Update() { }
}
