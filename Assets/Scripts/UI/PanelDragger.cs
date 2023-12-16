
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
    Vector3 pointerDelta = Vector3.zero;
    public bool isDraggingOverPanel;
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


        m_Active = false;
        target.ReleaseMouse();
        evt.StopPropagation();



    }


    private void OnPointerMove(PointerMoveEvent evt)
    {
        if (!m_Active || !target.HasPointerCapture(m_PointerId))
            return;
        VisualElement testTarg = (VisualElement)evt.target;
        //Vector3 pointerDelta = evt.position - pointerStartPosition;
        pointerDelta = evt.position - pointerStartPosition;




        // target.transform.position = new Vector2(
        //       Mathf.Clamp(pos.x + pointerDelta.x, 0, target.panel.visualTree.worldBound.width),
        //       Mathf.Clamp(pos.y + pointerDelta.y, 0, target.panel.visualTree.worldBound.height));
        target.transform.position = new Vector2(targetStartPosition.x + pointerDelta.x, targetStartPosition.y + pointerDelta.y);


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
            Vector3 pointerDelta = evt.position - pointerStartPosition;
            var pos = target.parent.LocalToWorld(target.layout.position);
            targetStartPosition = target.parent.LocalToWorld(target.transform.position);
            pointerStartPosition = evt.position;
            m_Start = evt.localPosition;
            m_PointerId = evt.pointerId;
            m_Active = true;
            target.CapturePointer(evt.pointerId);
            enabled = true;

            // Debug.Log($"targetStartPosition: {targetStartPosition} ; pointerStartPosition: {pointerStartPosition} ; pointerDelta.x: {pointerDelta.x} ; target.transform.position: {target.transform.position} ; pos: {pos})");
            // target.CapturePointer(m_PointerId);
            // target.style.position = Position.Absolute;
            // Vector2 diff = evt.localPosition - m_Start;

            // target.style.top = target.layout.y + diff.y;
            // target.style.left = target.layout.x + diff.x;

            // enabled = true;
            // evt.StopPropagation();
        }
    }
    private void PointerCaptureOutHandler(PointerCaptureOutEvent evt)
    {
        if (enabled)
        {
            Vector3 prevPos = targetStartPosition;
            VisualElement slotsContainer = root.Q<VisualElement>("PanelSlots");
            UQueryBuilder<VisualElement> allSlots =
                slotsContainer.Query<VisualElement>(className: "pressure-panel-slot");
            UQueryBuilder<VisualElement> overlappingSlots =
                allSlots.Where(OverlapsTarget);
            VisualElement closestOverlappingSlot =
                FindClosestSlot(overlappingSlots);
            Vector3 closestPos = Vector3.zero;
            Debug.Log($"closestOverlappingSlot: {closestOverlappingSlot}");
            if (closestOverlappingSlot != null)
            {
                closestPos = RootSpaceOfSlot(closestOverlappingSlot);
                // closestPos = new Vector2(closestPos.x - 5, closestPos.y - 5);
            }
            if (closestOverlappingSlot != null)
            {
                target.transform.position = closestPos;
            }
            else
            {
                // target.transform.position = targetStartPosition;
                target.transform.position = new Vector2(targetStartPosition.x + pointerDelta.x, targetStartPosition.y + pointerDelta.y); ;
            }
            // target.transform.position =
            //     closestOverlappingSlot != null ?
            //     closestPos :
            //     targetStartPosition;

            enabled = false;
        }
    }
    private Vector3 RootSpaceOfSlot(VisualElement slot)
    {
        Vector2 slotWorldSpace = slot.parent.LocalToWorld(slot.layout.position);
        Vector2 dist = target.parent.WorldToLocal(target.layout.position);
        Vector2 diff = new Vector2(slotWorldSpace.x - dist.x, slotWorldSpace.y - dist.y);
        // Debug.Log($"{target.parent.WorldToLocal(target.layout.position)}");
        // return root.WorldToLocal(slotWorldSpace);
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
