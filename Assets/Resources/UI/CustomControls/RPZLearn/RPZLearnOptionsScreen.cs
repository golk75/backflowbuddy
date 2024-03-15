using UnityEngine;
using UnityEngine.UIElements;

public class RPZLearnOptionsScreen : VisualElement
{
    VisualElement m_MenuButton;
    VisualElement m_RestartButton;
    VisualElement m_HelpButton;
    public new class UxmlFactory : UxmlFactory<RPZLearnOptionsScreen, UxmlTraits> { }

    public RPZLearnOptionsScreen()
    {
        this.RegisterCallback<GeometryChangedEvent>(OnGeometryChange);
    }

    void OnGeometryChange(GeometryChangedEvent evt)
    {


        this.UnregisterCallback<GeometryChangedEvent>(OnGeometryChange);
    }
}