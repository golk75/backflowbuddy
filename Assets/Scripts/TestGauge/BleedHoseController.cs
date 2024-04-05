
using UnityEngine;


public class BleedHoseController : MonoBehaviour
{
    public bool isHighBleedOpen;
    public bool isLowBleedOpen;

    private void OnEnable()
    {

        Actions.onHighBleedOperate += HighBleedKnobOperate;
        Actions.onLowBleedOperate += LowBleedKnobOperate;

    }



    private void OnDisable()
    {
        Actions.onHighBleedOperate -= HighBleedKnobOperate;
        Actions.onLowBleedOperate -= LowBleedKnobOperate;


    }
    void HighBleedKnobOperate()
    {

        if (isHighBleedOpen == false)
        {
            isHighBleedOpen = true;
        }
        else
        {
            isHighBleedOpen = false;
        }

    }
    private void LowBleedKnobOperate()
    {
        if (isLowBleedOpen == false)
        {
            isLowBleedOpen = true;
        }
        else
        {
            isLowBleedOpen = false;
        }

    }
}
