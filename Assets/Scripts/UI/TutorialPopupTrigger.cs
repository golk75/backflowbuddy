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

    //visual elements
    private VisualElement m_Tutorial_container;

    //gameobjects
    public GameObject m_GameUi;

    //root
    private UIDocument root;

    public TutorialPopUpScriptableObject[] PopupScriptableObjects;

    public int popupIndex = 0;
    public int skipTutPlayerPrefTestInt = 0;

    private void Awake()
    {
        root = m_GameUi.GetComponent<UIDocument>();
        m_Tutorial_container = root.rootVisualElement.Q<VisualElement>(TutorialContainerString);
        if (PlayerPrefs.GetInt("Tutorial Skip") == 0)
        {
            m_Tutorial_container.style.display = DisplayStyle.Flex;
        }
        else
        {
            m_Tutorial_container.style.display = DisplayStyle.None;
        }
        // if (skipTutPlayerPrefTestInt == 0)
        // {
        //     m_Tutorial_container.style.display = DisplayStyle.Flex;
        // }
        // else
        // {
        //     m_Tutorial_container.style.display = DisplayStyle.None;
        // }


    }





    void Update()
    {
        // if (popupIndex == 1)
        // {
        //     TutorialSystem.Show(PopupScriptableObjects[popupIndex].content, PopupScriptableObjects[popupIndex].header);
        // }

        if (Input.GetKeyDown(KeyCode.Space) && popupIndex < PopupScriptableObjects.Length - 1)
        {
            popupIndex++;
        }
        TutorialSystem.Show(PopupScriptableObjects[popupIndex].content, PopupScriptableObjects[popupIndex].header);
    }
}
