using UnityEngine;
using UnityEngine.UIElements;

public class DCPlayScreen : VisualElement
{

    public new class UxmlFactory : UxmlFactory<DCPlayScreen, UxmlTraits> { }

    public DCPlayScreen()
    {
        this.RegisterCallback<GeometryChangedEvent>(OnGeometryChange);
    }

    void OnGeometryChange(GeometryChangedEvent evt)
    {


        this.UnregisterCallback<GeometryChangedEvent>(OnGeometryChange);
    }
}