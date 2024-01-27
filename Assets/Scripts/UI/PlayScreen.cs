using UnityEngine;
using UnityEngine.UIElements;

public class PlayScreen : VisualElement
{

    public new class UxmlFactory : UxmlFactory<PlayScreen, UxmlTraits> { }

    public PlayScreen()
    {
        this.RegisterCallback<GeometryChangedEvent>(OnGeometryChange);
    }

    void OnGeometryChange(GeometryChangedEvent evt)
    {


        this.UnregisterCallback<GeometryChangedEvent>(OnGeometryChange);
    }
}