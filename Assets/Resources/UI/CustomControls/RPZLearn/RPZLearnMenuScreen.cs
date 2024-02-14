using UnityEngine;
using UnityEngine.UIElements;

public class RPZLearnMenuScreen : VisualElement
{
    VisualElement m_MenuButton;
    VisualElement m_RestartButton;
    VisualElement m_HelpButton;
    public new class UxmlFactory : UxmlFactory<RPZLearnMenuScreen, UxmlTraits> { }

    public RPZLearnMenuScreen()
    {
        this.RegisterCallback<GeometryChangedEvent>(OnGeometryChange);
    }

    void OnGeometryChange(GeometryChangedEvent evt)
    {


        this.UnregisterCallback<GeometryChangedEvent>(OnGeometryChange);
    }
}