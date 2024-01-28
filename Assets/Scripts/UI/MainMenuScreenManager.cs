using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UIElements;

public class MainMenuScreenManager : VisualElement
{
    VisualElement m_MainMenuScreen;
    VisualElement m_LearnScreen;
    VisualElement m_PlayScreen;
    VisualElement m_PlayScreenQuickTour;
    VisualElement m_QuickTourContainer;


    const string MainMenuPlayButtonString = "play-button";
    const string MainMenuLearnButtonString = "learn-button";
    const string MainMenuQuitButtonString = "quit-button";
    const string QuickTourContainerString = "QuickTourContainer";
    private const string TutorialPlayerPrefString = "Skip Tutorial";



    public new class UxmlFactory : UxmlFactory<MainMenuScreenManager, UxmlTraits> { }

    public MainMenuScreenManager()
    {
        this.RegisterCallback<GeometryChangedEvent>(OnGeometryChange);
    }

    void OnGeometryChange(GeometryChangedEvent evt)
    {

        // Debug.Log($"playerprefs: {PlayerPrefs.GetInt("Tutorial Skip")}");
        m_MainMenuScreen = this.Q("MainMenuScreen");
        m_LearnScreen = this.Q("LearnScreen");
        m_PlayScreen = this.Q("PlayScreen");
        m_PlayScreenQuickTour = this.Q("PlayScreenQuickTour");
        m_QuickTourContainer = this.Q(QuickTourContainerString);




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



        //Async Load Scene--> prevents ui from changing until scene is loaded up
        //DO NOT CHANGE THE ORDER IN THIS---->
        {
            AsyncOperation sceneLoad = SceneManager.LoadSceneAsync("DCTestScene");


#if UNITY_EDITOR
            PlayerPrefs.SetInt(TutorialPlayerPrefString, 0);
#endif
            //hide quick tour if user has already completed or skipped;
            if (PlayerPrefs.GetInt(TutorialPlayerPrefString) == 1)
            {

                m_QuickTourContainer.style.display = DisplayStyle.None;

            }
            //wait for scene to load before switching Ui
            if (sceneLoad.isDone)
            {


                m_MainMenuScreen.style.display = DisplayStyle.None;
                m_PlayScreen.style.display = DisplayStyle.Flex;
                m_LearnScreen.style.display = DisplayStyle.None;



            }
        }



    }



}