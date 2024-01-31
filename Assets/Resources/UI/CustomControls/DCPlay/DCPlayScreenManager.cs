using System;
using System.Collections.Generic;
using System.Runtime.InteropServices.WindowsRuntime;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UIElements;

public class DCPlayScreenManager : VisualElement
{

    VisualElement m_DCPlayScreen;
    VisualElement m_DCPlayMenuScreen;
    VisualElement m_GameMenuOptions;
    VisualElement m_GameMenuScreen;
    public new class UxmlFactory : UxmlFactory<DCPlayScreenManager, UxmlTraits> { }

    public DCPlayScreenManager()
    {
        this.RegisterCallback<GeometryChangedEvent>(OnGeometryChange);
    }

    void OnGeometryChange(GeometryChangedEvent evt)
    {
        m_DCPlayScreen = this.Q("DCPlayScreen");
        m_DCPlayMenuScreen = this.Q("DCPlayMenuScreen");
        m_GameMenuOptions = m_DCPlayMenuScreen.Q("GameMenuOptions");
        m_GameMenuScreen = m_DCPlayMenuScreen.Q("GameMenuScreen");
        m_DCPlayScreen?.Q("MenuButton")?.RegisterCallback<ClickEvent>(evt => OpenMenu());
        m_DCPlayMenuScreen?.Q("close-button")?.RegisterCallback<ClickEvent>(evt => CloseMenu());
        m_GameMenuOptions?.Q("close-button")?.RegisterCallback<ClickEvent>(evt => CloseMenu());
        m_GameMenuOptions?.Q("save-button")?.RegisterCallback<ClickEvent>(evt => CloseMenu());
        m_DCPlayMenuScreen?.Q("quit-button")?.RegisterCallback<ClickEvent>(evt => ExitToMainMenu());




        this.UnregisterCallback<GeometryChangedEvent>(OnGeometryChange);
    }

    private void ExitToMainMenu()
    {
        SceneManager.LoadScene("MainMenu");
    }

    private void CloseMenu()
    {
        m_DCPlayMenuScreen.style.display = DisplayStyle.None;
        m_GameMenuOptions.style.display = DisplayStyle.None;
    }

    private void OpenMenu()
    {
        m_DCPlayMenuScreen.style.display = DisplayStyle.Flex;
        m_GameMenuScreen.style.display = DisplayStyle.Flex;
    }
}