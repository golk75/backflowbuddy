using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel.Design.Serialization;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UIElements;

public class TutorialPopupTrigger : MonoBehaviour
{
    //visual element constants
    private const string TutorialContainerString = "TutorialPopup";
    private const string TutorialNextButtonString = "Tutotial_next_button";
    private const string TutorialPrevButtonString = "Tutotial_previous_button";
    private const string TutorialSkipButtonString = "Tutotial_skip_button";
    private const string TutorialPlayerPrefString = "Skip Tutorial";

    //visual elements
    private VisualElement m_Tutorial_container;
    private Button m_NextButton;
    private Button m_PreviousButton;
    private Button m_SkipButton;



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
        LoadTutorialPrefs();


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
    }
    //register call backs
    private void RegisterCallbacks()
    {
        m_NextButton.clicked += OnNextButtonClicked;
        m_SkipButton.clicked += OnSkipButtonClicked;
        m_PreviousButton.clicked += OnPrevButtonClicked;
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
    public void LoadTutorialPrefs()
    {
        popupIndex = 0;
        PlayerPrefs.GetInt(TutorialPlayerPrefString);
        if (PlayerPrefs.GetInt(TutorialPlayerPrefString) == 0)
        {
            m_Tutorial_container.style.display = DisplayStyle.Flex;
        }
        else
        {
            m_Tutorial_container.style.display = DisplayStyle.None;
        }
    }




    private void AssignVisualElements()
    {
        m_NextButton = root.rootVisualElement.Q<Button>(TutorialNextButtonString);
        m_PreviousButton = root.rootVisualElement.Q<Button>(TutorialPrevButtonString);
        m_SkipButton = root.rootVisualElement.Q<Button>(TutorialSkipButtonString);
        m_Tutorial_container = root.rootVisualElement.Q<VisualElement>(TutorialContainerString);
    }

    private void UpdatePopup()
    {
        // if (popupIndex == 1)
        // {
        //     TutorialSystem.Show(PopupScriptableObjects[popupIndex].content, PopupScriptableObjects[popupIndex].header);
        // }
        TutorialSystem.Show(PopupScriptableObjects[popupIndex].content, PopupScriptableObjects[popupIndex].header);

        if (popupIndex == PopupScriptableObjects.Length - 1)
        {
            // Debug.Log($"create close button");
            m_NextButton.style.display = DisplayStyle.None;
            m_SkipButton.text = "Close";
        }
        //player has pressed the skip button
        if (PlayerPrefs.GetInt(TutorialPlayerPrefString) == 1)
        {
            m_Tutorial_container.style.display = DisplayStyle.None;
        }
    }


    void Update()
    {
        UpdatePopup();

        //delete after testing skip button
        if (Input.GetKeyDown(KeyCode.Alpha0))
        {
            if (PlayerPrefs.GetInt(TutorialPlayerPrefString) == 0)
            {
                SaveTutorialPrefs(1);

                LoadTutorialPrefs();
            }
            else
            {
                SaveTutorialPrefs(0);

                LoadTutorialPrefs();
            }


        }

    }
}
