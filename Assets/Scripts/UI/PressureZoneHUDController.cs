
using System.Runtime.Serialization;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UIElements;

public class PressureZoneHUDController : MonoBehaviour
{

    public UiClickFilter uiClickFilter;
    const string SupplyZonePanelString = "SupplyPressure__panel";
    const string SupplyPressureTestString = "PressureZone1__value";
    const string PressureZone2 = "PressureZone2__panel";
    const string PressureZone3 = "PressureZone3__panel";
    VisualElement SupplyZonePanel;
    TextField SupplyPressureTextField;
    TextField PressureZone2Text;
    TextField PressureZone3Text;


    // Start is called before the first frame update
    void Start()
    {
        var root = GetComponent<UIDocument>();
        SupplyPressureTextField = root.rootVisualElement.Q<TextField>(SupplyPressureTestString);
        SupplyZonePanel = root.rootVisualElement.Q<VisualElement>(SupplyZonePanelString);

    }
    void OnEnable()
    {

    }
    void OnDisable()
    {

    }


    // Update is called once per frame
    void Update()
    {

    }
}
