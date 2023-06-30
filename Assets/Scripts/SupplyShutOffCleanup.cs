using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using com.zibra.liquid.Manipulators;
using com.zibra.liquid.DataStructures;


 
public class SupplyShutOffCleanup : MonoBehaviour
{
    
    /// <summary>
    /// Slowing velocity of liquid within the assembly when shut off is closed. 
    /// Disabling Voids so water that is in the assembly when it is shut off remains in assembly.
    /// </summary>
 
    ZibraLiquidSolverParameters solverParameters;
    float defaultMaxVelocity;
    [SerializeField]
    float cleanUpMaxVelocity = 1f;
    public AssemblyController assemblyController;

    [SerializeField]
    ZibraLiquidCollider supplyCapCollider;
    
    

    // Start is called before the first frame update
    void Start()
    {
        solverParameters = GetComponent<ZibraLiquidSolverParameters>();
        defaultMaxVelocity = solverParameters.MaximumVelocity;
        

        
    }

    // Update is called once per frame
    void Update()
    {
      if(!assemblyController.IsSupplyOn){
        solverParameters.MaximumVelocity = cleanUpMaxVelocity;
        supplyCapCollider.enabled = true;
      }
      else{
        solverParameters.MaximumVelocity = defaultMaxVelocity;
        supplyCapCollider.enabled = false;
      }
    } 

    
    




}
