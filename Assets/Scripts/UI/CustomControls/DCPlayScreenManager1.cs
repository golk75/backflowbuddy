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








        //Main Menu screen buttons
        // m_MainMenuScreen?.Q(MainMenuPlayButtonString)?.RegisterCallback<ClickEvent>(evt => EnablePlayScreen());
        // m_MainMenuScreen?.Q(MainMenuLearnButtonString)?.RegisterCallback<ClickEvent>(evt => EnableLearnScreen());
        // m_MainMenuScreen?.Q(MainMenuQuitButtonString)?.RegisterCallback<ClickEvent>(evt => QuitApplication());

        // m_LearnScreen?.Q("back-button")?.RegisterCallback<ClickEvent>(evt => EnableTitleScreen());
        // m_PlayScreen?.Q("back-button")?.RegisterCallback<ClickEvent>(evt => EnableTitleScreen());

        // m_LearnScreen.style.display = DisplayStyle.None;
        // m_PlayScreen.style.display = DisplayStyle.None;

        this.UnregisterCallback<GeometryChangedEvent>(OnGeometryChange);
    }





}