
using System;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UIElements;

public class GameOptionsScreen : MonoBehaviour
{


    //string ids
    [Header("Menu Screen elements")]
    [Tooltip("String IDs to query Visual Elements")]
    [SerializeField] string m_MenuScreenName = "GameMenuScreen";
    [SerializeField] string m_OptionsScreenName = "GameMenuOptionsScreen";
    const string DCTestSceneName = "DCTestScene";
    const string m_OptionsScreenMainMenuButtonName = "OptionsMenuScreen_main-menu-button";
    const string m_OptionsScreenSaveButtonName = "OptionsMenuScreen_save-button";
    const string CloseMenuButtonString = "OptionsMenuScreen_close-button";
    const string TutorialButtonString = "OptionsMenuScreen_tutorial_button";
    private const string TutorialPlayerPrefString = "Skip Tutorial";

    //visual elements
    VisualElement m_MenuScreen;
    VisualElement m_OptionsScreen;

    //buttons
    Button m_MenuScreenButton;
    Button m_CloseMenuButton;
    Button m_OptionsSaveButton;
    Button m_TutorialButton;

    //root element
    UIDocument m_DCTestScreen;

    void SetVisualElements()
    {
        //root
        m_DCTestScreen = GetComponent<UIDocument>();
        VisualElement root = m_DCTestScreen.rootVisualElement;

        //modals
        m_MenuScreen = root.Q(m_MenuScreenName);
        m_OptionsScreen = root.Q(m_OptionsScreenName);

        //buttons
        m_MenuScreenButton = root.Q<Button>(m_OptionsScreenMainMenuButtonName);
        m_CloseMenuButton = root.Q<Button>(CloseMenuButtonString);
        m_OptionsSaveButton = root.Q<Button>(m_OptionsScreenSaveButtonName);
        m_TutorialButton = root.Q<Button>(TutorialButtonString);

    }
    void RegisterButtonCallBacks()
    {
        m_CloseMenuButton?.RegisterCallback<ClickEvent>(ResumeGame);
        m_MenuScreenButton?.RegisterCallback<ClickEvent>(ShowGameMenuScreen);
        m_OptionsSaveButton?.RegisterCallback<ClickEvent>(ShowGameMenuScreen);
        m_TutorialButton?.RegisterCallback<ClickEvent>(TutorialButtonClicked);

    }

    private void TutorialButtonClicked(ClickEvent evt)
    {
        PlayerPrefs.SetInt(TutorialPlayerPrefString, 0);
        SceneManager.LoadScene(DCTestSceneName);
    }

    private void ResumeGame(ClickEvent evt)
    {

        ShowVisualElement(m_OptionsScreen, false);

    }

    private void ShowGameMenuScreen(ClickEvent evt)
    {
        ShowVisualElement(m_OptionsScreen, false);
        ShowVisualElement(m_MenuScreen, true);

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

    // Update is called once per frame
    void Update()
    {

    }
}
