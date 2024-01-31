using UnityEngine;
using UnityEngine.UIElements;

public class DCLearnMenuScreen : VisualElement
{
    VisualElement m_MenuButton;
    VisualElement m_RestartButton;
    VisualElement m_HelpButton;
    public new class UxmlFactory : UxmlFactory<DCLearnMenuScreen, UxmlTraits> { }

    public DCLearnMenuScreen()
    {
        this.RegisterCallback<GeometryChangedEvent>(OnGeometryChange);
    }

    void OnGeometryChange(GeometryChangedEvent evt)
    {


        this.UnregisterCallback<GeometryChangedEvent>(OnGeometryChange);
    }
}