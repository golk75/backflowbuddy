using System;
using System.ComponentModel.Design.Serialization;
using UnityEngine;
using UnityEngine.UIElements;
public class LearnScreen : VisualElement
{

    public VisualElement m_RpzPopup;


    public new class UxmlFactory : UxmlFactory<LearnScreen, UxmlTraits> { }

    public LearnScreen()
    {
        this.RegisterCallback<GeometryChangedEvent>(OnGeometryChange);
    }

    void OnGeometryChange(GeometryChangedEvent evt)
    {
        m_RpzPopup = this.Q("rpz-pop-up");
        this?.Q("rpz-button")?.RegisterCallback<ClickEvent>(evt => EnableRpzPopup());
        this?.Q("rpz-popup-back-button")?.RegisterCallback<ClickEvent>(evt => RemoveRpzPopup());


        this.UnregisterCallback<GeometryChangedEvent>(OnGeometryChange);
    }

    private void EnableRpzPopup()
    {
        m_RpzPopup.style.display = DisplayStyle.Flex;
    }

    private void RemoveRpzPopup()
    {
        m_RpzPopup.style.display = DisplayStyle.None;
    }



}