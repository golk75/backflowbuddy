using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel.Design.Serialization;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UIElements;

public class TutorialPopupTrigger : MonoBehaviour
{
    //visual element constants
    private const string TutorialContainerString = "TutorialPopup";
    private const string TutorialNextButtonString = "Tutotial_next_button";
    private const string TutorialPrevButtonString = "Tutotial_previous_button";
    private const string TutorialSkipButtonString = "Tutotial_skip_button";
    private const string TutorialPopupHeaderString = "TutorialPopup_header";
    private const string OptionsTutorialButtonString = "OptionsMenuScreen_tutorial_button";
    private const string TutorialPlayerPrefString = "Skip Tutorial";
    //visual elements
    private VisualElement m_Tutorial_container;
    private VisualElement m_PopupHeader;
    private Button m_NextButton;
    private Button m_PreviousButton;
    private Button m_SkipButton;
    private Button m_OptionsTutorialButton;


    //scene management
    [SerializeField] string m_DCTestScene_tutorial = "DCTestScene_tutorial";
    [SerializeField] string m_DCTestScene = "DCTestScene";


    //gameobjects
    public GameObject m_GameUi;


    //root
    private UIDocument root;


    public TutorialPopUpScriptableObject[] PopupScriptableObjects;


    //throw away
    public int popupIndex = 0;
    public int skipTutPlayerPrefTestInt = 0;


    private void Awake()
    {
        root = m_GameUi.GetComponent<UIDocument>();
        AssignVisualElements();
        RegisterCallbacks();
        popupIndex = 0;


    }

    private void AssignVisualElements()
    {
        m_NextButton = root.rootVisualElement.Q<Button>(TutorialNextButtonString);
        m_PreviousButton = root.rootVisualElement.Q<Button>(TutorialPrevButtonString);
        m_SkipButton = root.rootVisualElement.Q<Button>(TutorialSkipButtonString);
        m_OptionsTutorialButton = root.rootVisualElement.Q<Button>(OptionsTutorialButtonString);
        m_Tutorial_container = root.rootVisualElement.Q<VisualElement>(TutorialContainerString);
        m_PopupHeader = root.rootVisualElement.Q<VisualElement>(TutorialPopupHeaderString);

    }

    //register call backs
    private void RegisterCallbacks()
    {
        m_NextButton.clicked += OnNextButtonClicked;
        m_SkipButton.clicked += OnSkipButtonClicked;
        m_PreviousButton.clicked += OnPrevButtonClicked;
        m_OptionsTutorialButton.clicked += OnOptionsTutorialButtonClicked;
    }


    private void OnOptionsTutorialButtonClicked()
    {
        SaveTutorialPrefs(0);
        SceneManager.LoadSceneAsync(m_DCTestScene_tutorial);

    }


    private void OnNextButtonClicked()
    {
        if (popupIndex < PopupScriptableObjects.Length - 1)
        {
            popupIndex++;
        }

    }


    private void OnPrevButtonClicked()
    {
        if (popupIndex >= 1)
        {
            popupIndex--;
        }
    }


    private void OnSkipButtonClicked()
    {
        SaveTutorialPrefs(1);

    }


    public void SaveTutorialPrefs(int pref)
    {
        PlayerPrefs.SetInt(TutorialPlayerPrefString, pref);
    }


    void OnDisable()
    {
        UnRegisterCallbacks();
    }


    private void UnRegisterCallbacks()
    {
        m_NextButton.clicked -= OnNextButtonClicked;
        m_SkipButton.clicked -= OnSkipButtonClicked;
        m_PreviousButton.clicked -= OnPrevButtonClicked;
        m_OptionsTutorialButton.clicked -= OnOptionsTutorialButtonClicked;
    }


    private void UpdatePopup()
    {
        // if (popupIndex == 1)
        // {
        //     TutorialSystem.Show(PopupScriptableObjects[popupIndex].content, PopupScriptableObjects[popupIndex].header);
        // }
        TutorialSystem.Show(PopupScriptableObjects[popupIndex].content, PopupScriptableObjects[popupIndex].header);
        if (popupIndex == 0)
        {
            m_NextButton.text = "Start A Quick Tour";
            m_PreviousButton.style.display = DisplayStyle.None;
        }
        else
        {
            m_NextButton.text = "Next";
            m_PreviousButton.style.display = DisplayStyle.Flex;
        }
        if (popupIndex == PopupScriptableObjects.Length - 1)
        {
            m_NextButton.style.display = DisplayStyle.None;
            m_PopupHeader.style.display = DisplayStyle.None;

            m_SkipButton.text = "Close";
        }
        else
        {
            m_NextButton.style.display = DisplayStyle.Flex;
            m_PopupHeader.style.display = DisplayStyle.Flex;
        }
        if (PlayerPrefs.GetInt(TutorialPlayerPrefString) == 1)
            SceneManager.LoadScene(m_DCTestScene);
    }


    void Update()
    {
        UpdatePopup();


    }
}
