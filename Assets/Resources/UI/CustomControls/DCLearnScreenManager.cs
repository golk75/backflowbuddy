using System;
using System.Collections.Generic;
using System.Runtime.InteropServices.WindowsRuntime;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UIElements;

public class DCLearnScreenManager : VisualElement
{
    public VisualElement m_MainMenuScreen;
    public VisualElement m_LearnScreen;
    public VisualElement m_PlayScreen;
    public VisualElement m_PlayScreenQuickTour;
    public VisualElement m_QuickTourContainer;
    public VisualElement m_DeviceSelectionScreen;
    public VisualElement m_LearnScreenRpzPopup;
    public VisualElement m_PlayScreenRpzPopup;

    public new class UxmlFactory : UxmlFactory<DCLearnScreenManager, UxmlTraits> { }

    public DCLearnScreenManager()
    {
        this.RegisterCallback<GeometryChangedEvent>(OnGeometryChange);
    }

    void OnGeometryChange(GeometryChangedEvent evt)
    {



        this.UnregisterCallback<GeometryChangedEvent>(OnGeometryChange);
    }





}