
using com.zibra.liquid.DataStructures;
using com.zibra.liquid.Solver;
using UnityEngine;

public class FixedTimeApp : MonoBehaviour
{
    ZibraLiquid liquid;
    ZibraLiquidSolverParameters liquidSolverParameters;

    // Start is called before the first frame update
    void Start()
    {
        // liquid = GetComponent<ZibraLiquid>();
        // liquidSolverParameters = GetComponent<ZibraLiquidSolverParameters>();

        // if (Application.platform == RuntimePlatform.WindowsPlayer)
        // {
        //     liquid.UseFixedTimestep = true;
        //     liquidSolverParameters.ForceInteractionStrength = -0.5f;
        //     //moved to water controller 
        //     // liquidSolverParameters.ForceInteractionStrength = 0.0f;



        // }
        //OSX--------------------
        // else
        // {
        //     liquid.UseFixedTimestep = false;
        // }
    }


}
