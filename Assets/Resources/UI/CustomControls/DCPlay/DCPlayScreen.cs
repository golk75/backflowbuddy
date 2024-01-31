using System;
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

        //assign visual elements

        // m_LearnScreen?.Q("back-button")?.RegisterCallback<ClickEvent>(evt => EnableTitleScreen());

        this.UnregisterCallback<GeometryChangedEvent>(OnGeometryChange);
    }


}