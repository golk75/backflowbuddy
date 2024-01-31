using UnityEngine;
using UnityEngine.UIElements;

public class PlayScreenQuickTour : VisualElement
{

    public new class UxmlFactory : UxmlFactory<PlayScreenQuickTour, UxmlTraits> { }

    public PlayScreenQuickTour()
    {
        this.RegisterCallback<GeometryChangedEvent>(OnGeometryChange);
    }

    void OnGeometryChange(GeometryChangedEvent evt)
    {


        this.UnregisterCallback<GeometryChangedEvent>(OnGeometryChange);
    }
}