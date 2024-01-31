using UnityEngine;
using UnityEngine.UIElements;

public class DCLearnScreen : VisualElement
{

    public new class UxmlFactory : UxmlFactory<DCLearnScreen, UxmlTraits> { }

    public DCLearnScreen()
    {
        this.RegisterCallback<GeometryChangedEvent>(OnGeometryChange);
    }

    void OnGeometryChange(GeometryChangedEvent evt)
    {


        this.UnregisterCallback<GeometryChangedEvent>(OnGeometryChange);
    }
}