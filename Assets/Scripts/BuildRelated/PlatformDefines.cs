
using com.zibra.liquid.Solver;
using UnityEngine;
public class PlatformDefines : MonoBehaviour
{

    public ZibraLiquid liquid;

    void Start()
    {

        UpdateLiquidQuality();
    }
    private void UpdateLiquidQuality()
    {
        if (Application.platform == RuntimePlatform.Android)
        {
            liquid.EnableDownscale = true;
        }
        else if (Application.platform == RuntimePlatform.OSXPlayer)
        {
            liquid.EnableDownscale = false;
        }
    }


}