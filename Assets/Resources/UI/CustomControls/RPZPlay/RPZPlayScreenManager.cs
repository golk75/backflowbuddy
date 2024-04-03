using System;
using System.Collections.Generic;
using System.Runtime.InteropServices.WindowsRuntime;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UIElements;

public class RPZPlayScreenManager : VisualElement
{

    VisualElement m_RPZPlayScreen;
    VisualElement m_RPZPlayMenuScreen;
    VisualElement m_GameMenuOptions;
    VisualElement m_GameMenuScreen;
    public new class UxmlFactory : UxmlFactory<RPZPlayScreenManager, UxmlTraits> { }

    public RPZPlayScreenManager()
    {
        this.RegisterCallback<GeometryChangedEvent>(OnGeometryChange);
    }

    void OnGeometryChange(GeometryChangedEvent evt)
    {
        m_RPZPlayScreen = this.Q("RPZPlayScreen");
        m_RPZPlayMenuScreen = this.Q("RPZPlayMenuScreen");
        m_GameMenuOptions = m_RPZPlayMenuScreen.Q("GameMenuOptions");
        m_GameMenuScreen = m_RPZPlayMenuScreen.Q("GameMenuScreen");
        m_RPZPlayScreen?.Q("MenuButton")?.RegisterCallback<ClickEvent>(evt => OpenMenu());
        m_RPZPlayMenuScreen?.Q("resume")?.RegisterCallback<ClickEvent>(evt => CloseMenu());
        m_RPZPlayMenuScreen?.Q("close-button")?.RegisterCallback<ClickEvent>(evt => CloseMenu());
        m_GameMenuOptions?.Q("close-button")?.RegisterCallback<ClickEvent>(evt => CloseMenu());
        m_GameMenuOptions?.Q("save-button")?.RegisterCallback<ClickEvent>(evt => CloseMenu());
        m_RPZPlayMenuScreen?.Q("quit")?.RegisterCallback<ClickEvent>(evt => ExitToMainMenu());




        this.UnregisterCallback<GeometryChangedEvent>(OnGeometryChange);
    }

    private void ExitToMainMenu()
    {
        SceneManager.LoadScene("MainMenu");
    }

    private void CloseMenu()
    {
        m_RPZPlayMenuScreen.style.display = DisplayStyle.None;
        m_GameMenuOptions.style.display = DisplayStyle.None;
    }

    private void OpenMenu()
    {
        m_RPZPlayMenuScreen.style.display = DisplayStyle.Flex;
        m_GameMenuScreen.style.display = DisplayStyle.Flex;
    }
}