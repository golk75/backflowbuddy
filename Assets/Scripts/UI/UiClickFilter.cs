using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UIElements;

public class UiClickFilter : MonoBehaviour
{
    [SerializeField] UIDocument _uiDocument = null;
    public PlayerController playerController;

    const string SupplyPressureTextFieldString = "SupplyPressure__value";
    const string PressureZone2TextFieldString = "PressureZone2__value";
    const string PressureZone3TextFieldString = "PressureZone3__value";
    TextField SupplyPressureTextField;
    TextField PressureZone2TextField;
    TextField PressureZone3TextField;


    // Start is called before the first frame update
    void Start()
    {
        var root = GetComponent<UIDocument>();
        SupplyPressureTextField = root.rootVisualElement.Q<TextField>(SupplyPressureTextFieldString);
        PressureZone2TextField = root.rootVisualElement.Q<TextField>(PressureZone2TextFieldString);
        PressureZone3TextField = root.rootVisualElement.Q<TextField>(PressureZone3TextFieldString);


        //Register callbacks

        SupplyPressureTextField.RegisterCallback<MouseDownEvent>(MouseDown);
        PressureZone2TextField.RegisterCallback<MouseDownEvent>(MouseDown);
        PressureZone3TextField.RegisterCallback<MouseDownEvent>(MouseDown);

    }



    public bool IsPointerOverUI(Vector2 screenPos)
    {

        Vector2 pointerUiPos = new Vector2 { x = screenPos.x, y = Screen.height - screenPos.y };
        List<VisualElement> picked = new List<VisualElement>();
        _uiDocument.rootVisualElement.panel.PickAll(pointerUiPos, picked);

        if (picked.Count <= 0)
        {
            return false;
        }
        else
        {
            return true;
        }
    }


    private void MouseDown(MouseDownEvent evt)
    {
        Vector2 pointerScreenPos = Pointer.current.position.ReadValue();
        if (IsPointerOverUI(pointerScreenPos))
        {
            playerController.isOperableObject = false;
            playerController.operableObject = null;
        }


        // Vector2 pointerScreenPos = Pointer.current.position.ReadValue();
        // if (!uiClickFilter.IsPointerOverUI(pointerScreenPos))
        // {
        //     Debug.Log($"Ui Element clicked");
        // }
        // else
        // {
        //     Debug.Log($"Ui Element not clicked");
        // }

    }
    // Update is called once per frame
    void Update()
    {

    }
}
