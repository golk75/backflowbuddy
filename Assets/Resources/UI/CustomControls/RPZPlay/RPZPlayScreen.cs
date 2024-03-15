using System;
using UnityEngine;
using UnityEngine.UIElements;

public class RPZPlayScreen : VisualElement
{


    public new class UxmlFactory : UxmlFactory<RPZPlayScreen, UxmlTraits> { }

    public RPZPlayScreen()
    {
        this.RegisterCallback<GeometryChangedEvent>(OnGeometryChange);
    }

    void OnGeometryChange(GeometryChangedEvent evt)
    {

        //assign visual elements

        // m_LearnScreen?.Q("back-button")?.RegisterCallback<ClickEvent>(evt => EnableTitleScreen());

        this.UnregisterCallback<GeometryChangedEvent>(OnGeometryChange);
    }


}