using UnityEngine;
using UnityEngine.UIElements;

public class RPZPlayOptionsScreen : VisualElement
{
    VisualElement m_MenuButton;
    VisualElement m_RestartButton;
    VisualElement m_HelpButton;
    public new class UxmlFactory : UxmlFactory<RPZPlayOptionsScreen, UxmlTraits> { }

    public RPZPlayOptionsScreen()
    {
        this.RegisterCallback<GeometryChangedEvent>(OnGeometryChange);
    }

    void OnGeometryChange(GeometryChangedEvent evt)
    {


        this.UnregisterCallback<GeometryChangedEvent>(OnGeometryChange);
    }
}