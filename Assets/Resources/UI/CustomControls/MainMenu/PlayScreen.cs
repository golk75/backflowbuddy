using System;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UIElements;

public class PlayScreen : VisualElement
{
    public VisualElement m_RpzPopup;
    public VisualElement m_DCPlayScreen;
    public VisualElement m_DeviceSelectionScreen;
    public new class UxmlFactory : UxmlFactory<PlayScreen, UxmlTraits> { }

    public PlayScreen()
    {
        this.RegisterCallback<GeometryChangedEvent>(OnGeometryChange);
    }

    void OnGeometryChange(GeometryChangedEvent evt)
    {
        m_RpzPopup = this.Q("rpz-pop-up");

        m_DCPlayScreen = this.Q("DCPlayScreen");
        m_DeviceSelectionScreen = this.Q("device-selection");

        // this?.Q("rpz-button")?.RegisterCallback<ClickEvent>(evt => EnableRpzPopup());
        // this?.Q("rpz-popup-back-button")?.RegisterCallback<ClickEvent>(evt => RemoveRpzPopup());
        // this.Q("rpz-button").RegisterCallback<ClickEvent>(evt => EnableRPZPlayScreenAndScene());



        //Double Check button
        // this.Q("double-check-button").RegisterCallback<ClickEvent>(evt => EnableDoubleCheckPlayScreenAndScene());


        this.UnregisterCallback<GeometryChangedEvent>(OnGeometryChange);
    }

    private void EnableRPZPlayScreenAndScene()
    {
        SceneManager.LoadScene("RPZPlayScene");
        //Async Load Scene--> prevents ui from changing until scene is loaded up
        //DO NOT CHANGE THE ORDER IN THIS---->
        // {
        //     //AsyncOperation sceneLoadAsync = SceneManager.LoadSceneAsync("RPZPlayScene");
        //   SceneManager.LoadScene("RPZPlayScene");

        //     // #if UNITY_EDITOR
        //     //             PlayerPrefs.SetInt(TutorialPlayerPrefString, 0);
        //     // #endif


        //     // skipping quick tour

        //     //wait for scene to load before switching Ui
        //     if (SceneManager.GetActiveScene().name == "RPZPlayScene")
        //     {
        //         m_DeviceSelectionScreen.style.display = DisplayStyle.None;
        //     }



        // }
    }

    private void EnableDoubleCheckPlayScreenAndScene()
    {
        SceneManager.LoadScene("DCPlayScene");
        //Async Load Scene--> prevents ui from changing until scene is loaded up
        //DO NOT CHANGE THE ORDER IN THIS---->
        // {
        //     //AsyncOperation sceneLoadAsync = SceneManager.LoadSceneAsync("DCPlayScene");
        //     SceneManager.LoadScene("DCPlayScene");

        //     // #if UNITY_EDITOR
        //     //             PlayerPrefs.SetInt(TutorialPlayerPrefString, 0);
        //     // #endif


        //     // skipping quick tour

        //     //wait for scene to load before switching Ui
        //     if (SceneManager.GetActiveScene().name == "DCPlayScene")
        //     {
        //         m_DeviceSelectionScreen.style.display = DisplayStyle.None;
        //     }



        // }
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