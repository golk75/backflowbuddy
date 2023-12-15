
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UIElements;

public class PanelDragger : PointerManipulator
{
    private Vector3 m_Start;
    protected bool m_Active;
    private int m_PointerId;
    private VisualElement root { get; }
    private bool enabled { get; set; }
    private Vector2 targetStartPosition { get; set; }
    private Vector3 pointerStartPosition { get; set; }
    /// <summary>
    /// Awake is called when the script instance is being loaded.
    /// </summary>
    private void Awake()
    {

    }

    public PanelDragger(VisualElement target)
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

        Vector3 pointerDelta = evt.position - pointerStartPosition;

        target.transform.position = new Vector2(
              Mathf.Clamp(targetStartPosition.x + pointerDelta.x, 0, target.panel.visualTree.worldBound.width),
              Mathf.Clamp(targetStartPosition.y + pointerDelta.y, 0, target.panel.visualTree.worldBound.height));
        // target.style.position = Position.Absolute;
        // Vector2 diff = evt.localPosition - m_Start;

        // target.style.top = target.layout.y + diff.y;
        // target.style.left = target.layout.x + diff.x;

        // evt.StopPropagation();

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
            targetStartPosition = target.transform.position;
            pointerStartPosition = evt.position;
            m_Start = evt.localPosition;
            m_PointerId = evt.pointerId;
            m_Active = true;
            target.CapturePointer(evt.pointerId);
            enabled = true;
            // target.CapturePointer(m_PointerId);
            // target.style.position = Position.Absolute;
            // Vector2 diff = evt.localPosition - m_Start;

            // target.style.top = target.layout.y + diff.y;
            // target.style.left = target.layout.x + diff.x;
            // Actions.onPanelGrab?.Invoke(target);
            // enabled = true;
            // evt.StopPropagation();
        }
    }
    private void PointerCaptureOutHandler(PointerCaptureOutEvent evt)
    {
        if (enabled)
        {
            VisualElement slotsContainer = root.Q<VisualElement>("slots");
            UQueryBuilder<VisualElement> allSlots =
                slotsContainer.Query<VisualElement>("slot");
            UQueryBuilder<VisualElement> overlappingSlots =
                allSlots.Where(OverlapsTarget);
            VisualElement closestOverlappingSlot =
                FindClosestSlot(overlappingSlots);
            Vector3 closestPos = Vector3.zero;
            if (closestOverlappingSlot != null)
            {
                closestPos = RootSpaceOfSlot(closestOverlappingSlot);
                closestPos = new Vector2(closestPos.x - 5, closestPos.y - 5);
            }
            target.transform.position =
                closestOverlappingSlot != null ?
                closestPos :
                targetStartPosition;

            enabled = false;
        }
    }
    private Vector3 RootSpaceOfSlot(VisualElement slot)
    {
        Vector2 slotWorldSpace = slot.parent.LocalToWorld(slot.layout.position);
        return root.WorldToLocal(slotWorldSpace);
    }

    private VisualElement FindClosestSlot(UQueryBuilder<VisualElement> slots)
    {
        List<VisualElement> slotsList = slots.ToList();
        float bestDistanceSq = float.MaxValue;
        VisualElement closest = null;
        foreach (VisualElement slot in slotsList)
        {
            Vector3 displacement =
                RootSpaceOfSlot(slot) - target.transform.position;
            float distanceSq = displacement.sqrMagnitude;
            if (distanceSq < bestDistanceSq)
            {
                bestDistanceSq = distanceSq;
                closest = slot;
            }
        }
        return closest;
    }
    private bool OverlapsTarget(VisualElement slot)
    {
        return target.worldBound.Overlaps(slot.worldBound);
    }
    private void ModifyPanelStyle(VisualElement visualElement)
    {
        if (visualElement.resolvedStyle.position == Position.Relative)
        {
            visualElement.style.position = Position.Absolute;
        }

    }

}
