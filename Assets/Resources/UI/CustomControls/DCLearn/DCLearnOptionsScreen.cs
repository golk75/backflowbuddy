using UnityEngine;
using UnityEngine.UIElements;

public class DCLearnOptionsScreen : VisualElement
{
    VisualElement m_MenuButton;
    VisualElement m_RestartButton;
    VisualElement m_HelpButton;
    public new class UxmlFactory : UxmlFactory<DCLearnOptionsScreen, UxmlTraits> { }

    public DCLearnOptionsScreen()
    {
        this.RegisterCallback<GeometryChangedEvent>(OnGeometryChange);
    }

    void OnGeometryChange(GeometryChangedEvent evt)
    {


        this.UnregisterCallback<GeometryChangedEvent>(OnGeometryChange);
    }
}