
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UIElements;

public class PanelDragger : PointerManipulator
{
    private Vector3 m_Start;
    protected bool m_Active;
    private int m_PointerId;
    private Vector2 m_StartSize;




    /// <summary>
    /// Awake is called when the script instance is being loaded.
    /// </summary>
    private void Awake()
    {

    }

    public PanelDragger()
    {
        activators.Add(new ManipulatorActivationFilter { button = MouseButton.LeftMouse });
        m_PointerId = -1;
        m_Active = false;
    }

    protected override void RegisterCallbacksOnTarget()
    {
        target.RegisterCallback<PointerDownEvent>(OnPointerDown);
        target.RegisterCallback<PointerMoveEvent>(OnPointerMove);
        target.RegisterCallback<PointerUpEvent>(OnPointerUp);
    }


    protected override void UnregisterCallbacksFromTarget()
    {
        target.UnregisterCallback<PointerDownEvent>(OnPointerDown);
        target.UnregisterCallback<PointerMoveEvent>(OnPointerMove);
        target.UnregisterCallback<PointerUpEvent>(OnPointerUp);
    }


    private void OnPointerUp(PointerUpEvent evt)
    {

        if (!m_Active || !target.HasPointerCapture(m_PointerId) || !CanStopManipulation(evt))
            return;
        // IEnumerable<VisualElement> HoveredSlots = PressureZoneHUDController.DropAreaSlotList.Where(x =>
        //         x.worldBound.Overlaps(target.worldBound));
        //Set panel in DropArea slot if panel has been dragged over and mouse was released

        Actions.onPanelDrop?.Invoke(target);
        m_Active = false;
        target.ReleaseMouse();
        evt.StopPropagation();



    }


    private void OnPointerMove(PointerMoveEvent evt)
    {
        if (!m_Active || !target.HasPointerCapture(m_PointerId))
            return;
        ;
        Vector2 diff = evt.localPosition - m_Start;
        ModifyPanelStyle((VisualElement)evt.currentTarget);
        target.style.top = target.layout.y + diff.y;
        target.style.left = target.layout.x + diff.x;

        evt.StopPropagation();

    }


    private void OnPointerDown(PointerDownEvent evt)
    {

        if (m_Active)
        {
            evt.StopPropagation();
            return;
        }
        if (CanStartManipulation(evt))
        {

            m_Start = evt.localPosition;
            m_PointerId = evt.pointerId;
            m_Active = true;
            target.CapturePointer(m_PointerId);

            evt.StopPropagation();
        }
    }

    private void ModifyPanelStyle(VisualElement visualElement)
    {
        if (visualElement.style.position != Position.Absolute)
        {
            visualElement.style.position = Position.Absolute;
        }

    }

}
