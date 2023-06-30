using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using com.zibra.liquid.Manipulators;
public class RelaxWater : MonoBehaviour
{
    [SerializeField]
    AssemblyController assemblyController;
    ZibraLiquidForceField forceField;

    private void checkRelax(){
        if(assemblyController.IsSupplyOn == false){
            forceField.enabled = false;        
            }
            else{
                forceField.enabled = true;
            }
    }

    // Start is called before the first frame update
    void Start()
    {
        forceField = GetComponent<ZibraLiquidForceField>();
    }

    // Update is called once per frame
    void Update()
    {
        checkRelax();
        
    }
}
