using System;
using System.ComponentModel.Design.Serialization;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UIElements;
public class QuizSelection : VisualElement
{


    public VisualElement m_RpzPopup;
    public VisualElement m_DCPlayScreen;
    public VisualElement m_DeviceSelectionScreen;

    public new class UxmlFactory : UxmlFactory<QuizSelection, UxmlTraits> { }

    public QuizSelection()
    {
        this.RegisterCallback<GeometryChangedEvent>(OnGeometryChange);
    }

    void OnGeometryChange(GeometryChangedEvent evt)
    {

        this.UnregisterCallback<GeometryChangedEvent>(OnGeometryChange);
    }





}