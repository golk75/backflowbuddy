using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Assertions.Must;
using UnityEngine.UIElements;

public class UIManipulatorTest : PointerManipulator
{
    private Vector3 m_Start;
    protected bool m_Active;
    private int m_PointerId;
    private Vector2 m_StartSize;
    public UIManipulatorTest()
    {
        m_PointerId = -1;
        // activators.Add(new ManipulatorActivationFilter { button = MouseButton.LeftMouse });
        m_Active = false;
    }

    protected override void RegisterCallbacksOnTarget()
    {

        target.RegisterCallback<PointerEnterEvent>(OnPointerEnter);
    }

    protected override void UnregisterCallbacksFromTarget()
    {

        target.UnregisterCallback<PointerEnterEvent>(OnPointerEnter);
    }
    protected void OnPointerEnter(PointerEnterEvent e)
    {

    }

}
