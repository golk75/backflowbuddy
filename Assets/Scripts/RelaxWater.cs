using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using com.zibra.liquid.Manipulators;

public class RelaxWater : MonoBehaviour
{
    [SerializeField]
    AssemblyController assemblyController;

    [SerializeField]
    AssemblyController_Touch assemblyController_Touch;

    [SerializeField]
    ZibraLiquidForceField checkValve1ForceField;

    [SerializeField]
    ZibraLiquidForceField checkValve2ForceField;

    private void checkRelax()
    {
        if (assemblyController.IsSupplyOn == false || assemblyController_Touch.IsSupplyOn == false)
        {
            checkValve1ForceField.enabled = false;
            checkValve2ForceField.enabled = false;
        }
        else
        {
            checkValve1ForceField.enabled = true;
            checkValve2ForceField.enabled = true;
        }
    }

    // Start is called before the first frame update
    void Start() { }

    // Update is called once per frame
    void Update()
    {
        checkRelax();
    }
}
