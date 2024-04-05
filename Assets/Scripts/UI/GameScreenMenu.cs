using System;

using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.SceneManagement;
using UnityEngine.UIElements;

public class GameScreenMenu : MonoBehaviour
{

    [Header("Menu Screen elements")]
    [Tooltip("String IDs to query Visual Elements")]
    [SerializeField] string m_MenuScreenName = "GameMenuScreen";
    [SerializeField] string m_OptionsScreenName = "GameMenuOptions";

    [SerializeField] string m_MenuMenuSceneName = "MainMenu";


    [Header("Blur")]
    [SerializeField] Volume m_Volume;


    // string Ids
    const string MenuButtonString = "MenuButton";
    const string ResumeButtonString = "GameMenuScreen_resume-button";
    const string OptionsButtonString = "GameMenuScreen_options-button";
    const string SaveOptionsButtonString = "OptionsMenuScreen_save-button";
    const string CloseMenuButtonString = "GameMenuScreen_close-button";
    const string QuitToMenuButtonString = "GameMenuScreen_main-menu-button";



    // string IDs
    // references to functional UI elements (buttons and screens)
    VisualElement m_MenuScreen;
    VisualElement m_OptionsScreen;
    Button m_MenuButton;
    Button m_ResumeButton;
    Button m_OptionsButton;
    Button m_CloseMenuButton;
    Button m_QuitToMenuButton;


    UIDocument m_DCTestScreen;

    // public BannerViewController _bannerViewController;

    private void OnEnable()
    {
        // _bannerViewController.LoadAd();

    }

    private void OnDisable()
    {

    }
    void SetVisualElements()
    {
        m_DCTestScreen = GetComponent<UIDocument>();
        VisualElement root = m_DCTestScreen.rootVisualElement;
        m_MenuScreen = root.Q(m_MenuScreenName);
        m_OptionsScreen = root.Q(m_OptionsScreenName);
        m_MenuButton = root.Q<Button>(MenuButtonString);
        m_ResumeButton = root.Q<Button>(ResumeButtonString);
        m_OptionsButton = root.Q<Button>(OptionsButtonString);
        m_CloseMenuButton = root.Q<Button>(CloseMenuButtonString);
        m_QuitToMenuButton = root.Q<Button>(QuitToMenuButtonString);

    }
    void RegisterButtonCallBacks()
    {
        m_MenuButton?.RegisterCallback<ClickEvent>(ShowGameMenuScreen);
        m_ResumeButton?.RegisterCallback<ClickEvent>(ResumeGame);
        m_OptionsButton?.RegisterCallback<ClickEvent>(ShowOptions);
        m_CloseMenuButton?.RegisterCallback<ClickEvent>(ResumeGame);
        m_QuitToMenuButton?.RegisterCallback<ClickEvent>(QuitGame);

    }

    private void ShowOptions(ClickEvent evt)
    {
        ShowVisualElement(m_OptionsScreen, true);
        ShowVisualElement(m_MenuScreen, false);
    }

    private void ResumeGame(ClickEvent evt)
    {

        ShowVisualElement(m_MenuScreen, false);

    }

    private void ShowGameMenuScreen(ClickEvent evt)
    {
        ShowVisualElement(m_MenuScreen, true);

    }
    void QuitGame(ClickEvent evt)
    {


        SceneManager.LoadSceneAsync(m_MenuMenuSceneName);
        // _bannerViewController.DestroyAd();

    }
    void ShowVisualElement(VisualElement visualElement, bool state)
    {
        if (visualElement == null)
            return;

        visualElement.style.display = (state) ? DisplayStyle.Flex : DisplayStyle.None;

    }


    // Start is called before the first frame update
    void Start()
    {
        SetVisualElements();
        RegisterButtonCallBacks();
    }




}
