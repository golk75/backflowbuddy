using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class MainMenuScreenManager : VisualElement
{
    VisualElement m_MainMenuScreen;
    VisualElement m_LearnScreen;
    VisualElement m_PlayScreen;
    VisualElement m_PlayScreenQuickTour;


    const string MainMenuPlayButtonString = "play-button";
    const string MainMenuLearnButtonString = "learn-button";
    const string MainMenuQuitButtonString = "quit-button";

    public new class UxmlFactory : UxmlFactory<MainMenuScreenManager, UxmlTraits> { }

    public MainMenuScreenManager()
    {
        this.RegisterCallback<GeometryChangedEvent>(OnGeometryChange);
    }

    void OnGeometryChange(GeometryChangedEvent evt)
    {
        m_MainMenuScreen = this.Q("MainMenuScreen");
        m_LearnScreen = this.Q("LearnScreen");
        m_PlayScreen = this.Q("PlayScreen");
        m_PlayScreenQuickTour = this.Q("PlayScreenQuickTour");



        m_MainMenuScreen?.Q(MainMenuPlayButtonString)?.RegisterCallback<ClickEvent>(ev => EnablePlayScreen());
        m_MainMenuScreen?.Q(MainMenuLearnButtonString)?.RegisterCallback<ClickEvent>(ev => EnableLearnScreen());

        m_LearnScreen?.Q("back-button")?.RegisterCallback<ClickEvent>(ev => EnableTitleScreen());
        m_PlayScreen?.Q("back-button")?.RegisterCallback<ClickEvent>(ev => EnableTitleScreen());


        this.UnregisterCallback<GeometryChangedEvent>(OnGeometryChange);
    }

    private void EnableTitleScreen()
    {
        m_MainMenuScreen.style.display = DisplayStyle.Flex;
        m_PlayScreen.style.display = DisplayStyle.None;
        m_LearnScreen.style.display = DisplayStyle.None;
    }

    private void EnableLearnScreen()
    {
        m_MainMenuScreen.style.display = DisplayStyle.None;
        m_PlayScreen.style.display = DisplayStyle.None;
        m_LearnScreen.style.display = DisplayStyle.Flex;
    }

    private void EnablePlayScreen()
    {
        m_MainMenuScreen.style.display = DisplayStyle.None;
        m_PlayScreen.style.display = DisplayStyle.Flex;
        m_LearnScreen.style.display = DisplayStyle.None;
    }

    // public void EnableHostScreen()
    // {
    //     m_TitleScreen.style.display = DisplayStyle.None;
    //     m_HostScreen.style.display = DisplayStyle.Flex;
    //     m_JoinScreen.style.display = DisplayStyle.None;
    //     m_ManualConnectScreen.style.display = DisplayStyle.None;

    // }

    // public void EnableJoinScreen()
    // {
    //     m_TitleScreen.style.display = DisplayStyle.None;
    //     m_HostScreen.style.display = DisplayStyle.None;
    //     m_JoinScreen.style.display = DisplayStyle.Flex;
    //     m_ManualConnectScreen.style.display = DisplayStyle.None;
    // }

    // public void EnableManualScreen()
    // {
    //     m_TitleScreen.style.display = DisplayStyle.None;
    //     m_HostScreen.style.display = DisplayStyle.None;
    //     m_JoinScreen.style.display = DisplayStyle.None;
    //     m_ManualConnectScreen.style.display = DisplayStyle.Flex;
    // }

    // public void EnableTitleScreen()
    // {
    //     m_TitleScreen.style.display = DisplayStyle.Flex;
    //     m_HostScreen.style.display = DisplayStyle.None;
    //     m_JoinScreen.style.display = DisplayStyle.None;
    //     m_ManualConnectScreen.style.display = DisplayStyle.None;
    // }

}