using System;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using UnityEngine;
using UnityEngine.UIElements;

public class DraggerHandle : PointerManipulator
{

    protected bool m_Active;
    private int m_PointerId;
    private Vector3 m_Start;
    private Vector2 targetStartPosition { get; set; }
    private Vector3 pointerStartPosition { get; set; }
    Vector3 pointerDelta = Vector3.zero;
    private VisualElement root { get; }
    private bool enabled { get; set; }
    public bool isDraggingOverPanel;
    public DraggerHandle(VisualElement target)
    {
        activators.Add(new ManipulatorActivationFilter { button = MouseButton.LeftMouse });
        m_PointerId = -1;
        m_Active = false;
        this.target = target;
        root = target.parent;

    }

    protected override void RegisterCallbacksOnTarget()
    {
        target.RegisterCallback<PointerDownEvent>(OnPointerDown);
        target.RegisterCallback<PointerMoveEvent>(OnPointerMove);
        target.RegisterCallback<PointerUpEvent>(OnPointerUp);

        target.RegisterCallback<PointerCaptureOutEvent>(PointerCaptureOutHandler);
    }



    protected override void UnregisterCallbacksFromTarget()
    {
        target.UnregisterCallback<PointerDownEvent>(OnPointerDown);
        target.UnregisterCallback<PointerMoveEvent>(OnPointerMove);
        target.UnregisterCallback<PointerUpEvent>(OnPointerUp);

        target.UnregisterCallback<PointerCaptureOutEvent>(PointerCaptureOutHandler);

    }


    private void OnPointerMove(PointerMoveEvent evt)
    {

        if (!m_Active || !target.HasPointerCapture(m_PointerId))
            return;

        pointerDelta = evt.position - pointerStartPosition;

        target.transform.position = new Vector2(targetStartPosition.x + pointerDelta.x, targetStartPosition.y + pointerDelta.y);

        evt.StopPropagation();


    }

    private void OnPointerDown(PointerDownEvent evt)
    {
        Debug.Log($"dragger grabbed");
        if (m_Active || !CanStartManipulation(evt))
        {
            evt.StopImmediatePropagation();
            return;
        }

        targetStartPosition = target.parent.LocalToWorld(target.transform.position);
        pointerStartPosition = evt.position;
        m_Start = evt.localPosition;
        m_PointerId = evt.pointerId;
        m_Active = true;
        target.CapturePointer(evt.pointerId);
        enabled = true;
    }

    private void OnPointerUp(PointerUpEvent evt)
    {


        if (!m_Active || !target.HasPointerCapture(m_PointerId) || !CanStopManipulation(evt))
            return;

        m_Active = false;
        target.transform.position = targetStartPosition;
        target.ReleasePointer(m_PointerId);
        evt.StopPropagation();
        Debug.Log($"pointer up");


    }
    private void PointerCaptureOutHandler(PointerCaptureOutEvent evt)
    {
        if (enabled)
        {

            target.transform.position = targetStartPosition;
        }

        enabled = false;
    }
}
