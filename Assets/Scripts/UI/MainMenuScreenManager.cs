using System;
using System.Collections.Generic;
using System.Runtime.InteropServices.WindowsRuntime;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UIElements;

public class MainMenuScreenManager : VisualElement
{
    private VisualElement m_MainMenuScreen;
    private VisualElement m_LearnScreen;
    private VisualElement m_PlayScreen;
    private VisualElement m_PlayScreenQuickTour;
    private VisualElement m_QuickTourContainer;
    private VisualElement m_DeviceSelectionScreen;
    private VisualElement m_RpzPopup;


    private const string MainMenuPlayButtonString = "play-button";
    private const string MainMenuLearnButtonString = "learn-button";
    private const string MainMenuQuitButtonString = "quit-button";
    private const string DoubleCheckButtonString = "double-check-button";
    private const string RpzButtonString = "rpz-button";
    private const string RpzPopupString = "rpz-pop-up";
    private const string RpzBackButtonString = "rpz-popup-back-button";

    private const string QuickTourContainerString = "QuickTourContainer";
    private const string DeviceSelectionScreenString = "DeviceSelectionScreen";

    private const string DCTestSceneString = "DCTestScene";


    private const string TutorialPlayerPrefString = "Skip Tutorial";

    private List<VisualElement> ScreensToHide;
    private List<VisualElement> ScreensToShow;

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
        m_QuickTourContainer = this.Q(QuickTourContainerString);
        m_DeviceSelectionScreen = this.Q(DeviceSelectionScreenString);
        m_RpzPopup = m_DeviceSelectionScreen.Q(RpzPopupString);




        //Main Menu screen buttons
        m_MainMenuScreen?.Q(MainMenuPlayButtonString)?.RegisterCallback<ClickEvent>(evt => EnablePlayScreen());
        m_MainMenuScreen?.Q(MainMenuLearnButtonString)?.RegisterCallback<ClickEvent>(evt => EnableLearnScreen());
        m_MainMenuScreen?.Q(MainMenuQuitButtonString)?.RegisterCallback<ClickEvent>(evt => QuitApplication());
        //end Main Menu

        //Device Seleciton screen buttons
        m_DeviceSelectionScreen?.Q(DoubleCheckButtonString)?.RegisterCallback<ClickEvent>(evt => EnableDoubleCheckScreen());
        m_DeviceSelectionScreen?.Q(RpzButtonString)?.RegisterCallback<ClickEvent>(evt => EnableRpzPopUp());
        m_DeviceSelectionScreen?.Q(RpzBackButtonString)?.RegisterCallback<ClickEvent>(evt => EnableDeviceSelectionScreen());
        m_DeviceSelectionScreen?.Q("back-button")?.RegisterCallback<ClickEvent>(evt => EnableTitleScreen());

        //
        m_LearnScreen?.Q("back-button")?.RegisterCallback<ClickEvent>(evt => EnableTitleScreen());
        m_PlayScreen?.Q("back-button")?.RegisterCallback<ClickEvent>(evt => EnableTitleScreen());



        this.UnregisterCallback<GeometryChangedEvent>(OnGeometryChange);
    }

    private void EnablePlayScreen()
    {
        EnableDeviceSelectionScreen();
    }

    private void EnableRpzPopUp()
    {

        ScreensToShow = new List<VisualElement>
         {
             m_RpzPopup

         };
        ChangeScreenStatus(ScreensToShow, ScreensToHide);
    }


    private void EnableDeviceSelectionScreen()
    {

        ScreensToHide = new List<VisualElement>()
         {
             m_QuickTourContainer,
             m_MainMenuScreen,
             m_LearnScreen,
             m_RpzPopup
         };
        ScreensToShow = new List<VisualElement>
         {
             m_DeviceSelectionScreen

         };


        ChangeScreenStatus(ScreensToShow, ScreensToHide);

    }



    private void EnableTitleScreen()
    {
        ScreensToHide = new List<VisualElement>()
         {
             m_DeviceSelectionScreen,
             m_QuickTourContainer,
             m_LearnScreen,
         };
        ScreensToShow = new List<VisualElement>
         {
             m_MainMenuScreen

         };
        ChangeScreenStatus(ScreensToShow, ScreensToHide);
        // m_MainMenuScreen.style.display = DisplayStyle.Flex;
        // m_PlayScreen.style.display = DisplayStyle.None;
        // m_LearnScreen.style.display = DisplayStyle.None;
    }

    private void EnableLearnScreen()
    {
        EnableDeviceSelectionScreen();

    }

    private void EnableDoubleCheckScreen()
    {
        /// <summary>
        /// Quick Tour Disabled
        /// </summary>

        if (PlayerPrefs.GetInt(TutorialPlayerPrefString) == 1)
        {
            ScreensToHide = new List<VisualElement>()
         {
            m_QuickTourContainer,
            m_MainMenuScreen,
            m_LearnScreen,
            m_DeviceSelectionScreen

         };

            ScreensToShow = new List<VisualElement>
         {
            m_PlayScreen
        };
        }
        /// <summary>
        /// Quick Tour Enabled
        /// </summary>

        else
        {
            ScreensToHide = new List<VisualElement>()
            {
                m_MainMenuScreen,
                m_LearnScreen,
                m_DeviceSelectionScreen
            };


            ScreensToShow = new List<VisualElement>
            {
              m_PlayScreen,
              m_QuickTourContainer
            };


        }

        ChangeSceneAndScreen(DCTestSceneString, PlayerPrefs.GetInt(TutorialPlayerPrefString), ScreensToShow, ScreensToHide);


    }







    private void ChangeSceneAndScreen(string sceneToLoad, int playerPref, List<VisualElement> listToShow, List<VisualElement> listToHide)
    {

        //Async Load Scene--> prevents ui from changing until scene is loaded up
        //DO NOT CHANGE THE ORDER IN THIS---->
        {
            AsyncOperation sceneLoadAsync = SceneManager.LoadSceneAsync(sceneToLoad);


            // #if UNITY_EDITOR
            //             PlayerPrefs.SetInt(TutorialPlayerPrefString, 0);
            // #endif


            //skipping quick tour

            //wait for scene to load before switching Ui
            if (sceneLoadAsync.isDone)
            {

                ChangeScreenStatus(listToShow, listToHide);
            }


        }


    }



    private void ChangeScreenStatus(List<VisualElement> listOfScreensToShow, List<VisualElement> listOfScreensToHide)
    {
        if (listOfScreensToShow == null || listOfScreensToHide == null)
        {
            return;
        }

        foreach (var screenToHide in listOfScreensToHide)
        {
            screenToHide.style.display = DisplayStyle.None;
        }
        foreach (var screenToShow in listOfScreensToShow)
        {

            screenToShow.style.display = DisplayStyle.Flex;
        }

        ScreensToShow.Clear();
        ScreensToHide.Clear();
    }


    private void QuitApplication()
    {
#if UNITY_EDITOR
        if (Application.isPlaying)
#endif
            Application.Quit();

    }

}