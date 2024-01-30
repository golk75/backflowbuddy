using UnityEngine;
using UnityEngine.UIElements;

public class LearnScreen : VisualElement
{

    public new class UxmlFactory : UxmlFactory<LearnScreen, UxmlTraits> { }

    public LearnScreen()
    {
        this.RegisterCallback<GeometryChangedEvent>(OnGeometryChange);
    }

    void OnGeometryChange(GeometryChangedEvent evt)
    {


        this.UnregisterCallback<GeometryChangedEvent>(OnGeometryChange);
    }
}