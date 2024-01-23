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
    private const string QuickTourContainerString = "QuickTourContainer";
    private const string TutorialNextButtonString = "Tutotial_next_button";
    private const string TutorialPrevButtonString = "Tutotial_previous_button";
    private const string TutorialSkipButtonString = "Tutotial_skip_button";
    private const string TutorialPopupHeaderString = "TutorialPopup_header";
    private const string OptionsTutorialButtonString = "OptionsMenuScreen_tutorial_button";
    private const string TutorialPlayerPrefString = "Skip Tutorial";
    private const string TutorialButtonContainerString = "TutorialPopup_button_container";

    //style classes
    private const string PopupButtonStyleString = "tutorial-popup-button";
    private const string PopupButtonMaxStyleString = "tutorial-popup-button-max";


    //visual elements
    public VisualElement m_Tutorial_container;
    //private VisualElement m_Tutorial_container;
    private VisualElement m_PopupHeader;
    private VisualElement m_ButtonContainer;
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

        if (PlayerPrefs.GetInt(TutorialPlayerPrefString) == 0)
        {
            m_Tutorial_container.style.display = DisplayStyle.Flex;
        }

    }

    private void AssignVisualElements()
    {
        m_NextButton = root.rootVisualElement.Q<Button>(TutorialNextButtonString);
        m_PreviousButton = root.rootVisualElement.Q<Button>(TutorialPrevButtonString);
        m_SkipButton = root.rootVisualElement.Q<Button>(TutorialSkipButtonString);
        m_OptionsTutorialButton = root.rootVisualElement.Q<Button>(OptionsTutorialButtonString);
        m_Tutorial_container = root.rootVisualElement.Q<VisualElement>(QuickTourContainerString);
        m_PopupHeader = root.rootVisualElement.Q<VisualElement>(TutorialPopupHeaderString);
        m_ButtonContainer = root.rootVisualElement.Q<VisualElement>(TutorialButtonContainerString);

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
            m_NextButton.RemoveFromClassList(PopupButtonStyleString);
            m_NextButton.AddToClassList(PopupButtonMaxStyleString);
            m_SkipButton.AddToClassList(PopupButtonMaxStyleString);
            m_SkipButton.RemoveFromClassList(PopupButtonStyleString);
            m_NextButton.text = "Start A Quick Tour";
            m_ButtonContainer.style.flexDirection = FlexDirection.ColumnReverse;
            m_PreviousButton.style.display = DisplayStyle.None;
        }
        else
        {
            m_NextButton.RemoveFromClassList(PopupButtonStyleString);
            m_NextButton.AddToClassList(PopupButtonMaxStyleString);
            m_NextButton.text = "Next";
            m_ButtonContainer.style.flexDirection = FlexDirection.Row;
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
