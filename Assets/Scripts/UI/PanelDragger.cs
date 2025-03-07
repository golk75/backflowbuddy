
using System.Collections.Generic;

using UnityEngine;
using UnityEngine.UIElements;

public class PanelDragger : PointerManipulator
{
    protected bool m_Active;
    private VisualElement root { get; }
    private bool enabled { get; set; }
    private Vector2 targetStartPosition { get; set; }
    private Vector3 pointerStartPosition { get; set; }
    Vector3 pointerDelta = Vector3.zero;
    public bool isDraggingOverPanel;


    public PanelDragger(VisualElement target)
    {
        activators.Add(new ManipulatorActivationFilter { button = MouseButton.LeftMouse });
        m_Active = false;
        this.target = target;
        root = target.parent;

    }


    protected override void RegisterCallbacksOnTarget()
    {
        target.RegisterCallback<PointerDownEvent>(OnPointerDown);
        target.RegisterCallback<PointerMoveEvent>(OnPointerMove);
        target.RegisterCallback<PointerUpEvent>(OnPointerUp);
        // target.RegisterCallback<PointerCaptureOutEvent>(PointerCaptureOutHandler);
    }


    protected override void UnregisterCallbacksFromTarget()
    {
        target.UnregisterCallback<PointerDownEvent>(OnPointerDown);
        target.UnregisterCallback<PointerMoveEvent>(OnPointerMove);
        target.UnregisterCallback<PointerUpEvent>(OnPointerUp);
        // target.UnregisterCallback<PointerCaptureOutEvent>(PointerCaptureOutHandler);
    }


    private void OnPointerUp(PointerUpEvent evt)
    {
        if (target.HasPointerCapture(evt.pointerId))
        {
            m_Active = false;
            target.ReleasePointer(evt.pointerId);
            evt.StopPropagation();
        }


    }


    private void OnPointerMove(PointerMoveEvent evt)
    {

        if (enabled && target.HasPointerCapture(evt.pointerId))
        {

            Vector3 pointerDelta = evt.position - pointerStartPosition;

            target.transform.position = new Vector2(targetStartPosition.x + pointerDelta.x, targetStartPosition.y + pointerDelta.y);

        }

    }


    private void OnPointerDown(PointerDownEvent evt)
    {
        targetStartPosition = target.transform.position;
        pointerStartPosition = evt.position;
        target.CapturePointer(evt.pointerId);
        enabled = true;

    }


    // private void PointerCaptureOutHandler(PointerCaptureOutEvent evt)
    // {
    //     if (enabled)
    //     {

    //         VisualElement slotsContainer = root.Q<VisualElement>("PanelSlots");
    //         UQueryBuilder<VisualElement> allSlots =
    //             slotsContainer.Query<VisualElement>(className: "pressure-panel-slot");
    //         UQueryBuilder<VisualElement> overlappingSlots =
    //             allSlots.Where(OverlapsTarget);
    //         VisualElement closestOverlappingSlot =
    //             FindClosestSlot(overlappingSlots);
    //         Vector3 closestPos = Vector3.zero;

    //         if (closestOverlappingSlot != null)
    //         {
    //             closestPos = RootSpaceOfSlot(closestOverlappingSlot);
    //             // closestPos = new Vector2(closestPos.x - 5, closestPos.y - 5);
    //         }
    //         if (closestOverlappingSlot != null)
    //         {
    //             target.transform.position = closestPos;
    //         }
    //         else
    //         {

    //             target.transform.position = new Vector2(targetStartPosition.x + pointerDelta.x, targetStartPosition.y + pointerDelta.y); ;
    //         }

    //         enabled = false;
    //     }
    // }


    private Vector3 RootSpaceOfSlot(VisualElement slot)
    {
        Vector2 slotWorldSpace = slot.parent.parent.LocalToWorld(slot.layout.position);
        Vector2 dist = target.parent.WorldToLocal(target.layout.position);
        Vector2 diff = new Vector2(slotWorldSpace.x - dist.x, slotWorldSpace.y - dist.y);
        return root.WorldToLocal(diff);
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


}
