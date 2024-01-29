using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem.Layouts;
using UnityEngine.UIElements;

public class TutorialSystem : MonoBehaviour
{

    private static TutorialSystem current;
    public TutorialPopup tutorialPopup;
    [SerializeField]
    int skipTourPlayerPrefs;


    void Awake()
    {
        current = this;

    }
    void OnEnable()
    {
        skipTourPlayerPrefs = PlayerPrefs.GetInt("Skip Tutorial");
    }
    public static void Hide()
    {
        current.tutorialPopup.gameObject.SetActive(false);
        // current.tooltip.gameObject.SetActive(false);
        // Debug.Log($"Hiding");
    }
    public static void Show(string content, string header)
    {
        current.tutorialPopup.SetText(content, header);
        current.tutorialPopup.gameObject.SetActive(true);
        // current.tooltip.SetText(content, header);
        // current.tooltip.gameObject.SetActive(true);
        // Debug.Log($"Showing");

    }
    // //come back to visit array loop..
    // // public GameObject[] popUps;

    // public VisualElement[] popUps;

    // int popUpIndex;

    // //constants
    // private const string TutorialContainerString = "TutorialPopup";
    // public GameObject m_GameUi;
    // private UIDocument root;

    // public VisualElement m_Tutorial_container;

    // /// <summary>
    // /// Awake is called when the script instance is being loaded.
    // /// </summary>
    // private void Awake()
    // {
    //     root = m_GameUi.GetComponent<UIDocument>();
    //     m_Tutorial_container = root.rootVisualElement.Q<VisualElement>(TutorialContainerString);

    // }

    // public void SaveTutorialPrefs()
    // {
    //     PlayerPrefs.SetInt("Skip Tutorial", 1);
    // }
    // public void LoadTutorialPrefs()
    // {
    //     PlayerPrefs.GetInt("Skip Tutorial");
    // }
    // // Update is called once per frame
    // void Update()
    // {
    //     if (Input.GetKeyDown(KeyCode.RightArrow))
    //     {
    //         m_Tutorial_container.style.display = DisplayStyle.Flex;

    //     }
    //     //come back to visit array loop
    //     // {
    //     //     for (int i = 0; i < popUps.Length; i++)
    //     //     {
    //     //         if (i == popUpIndex)
    //     //         {
    //     //             popUps[popUpIndex].SetActive(true);
    //     //         }
    //     //         else
    //     //         {
    //     //             popUps[popUpIndex].SetActive(false);
    //     //         }
    //     //     }
    //     //     switch (popUpIndex)
    //     //     {
    //     //         case 0:
    //     //             if (Input.GetKeyDown(KeyCode.RightArrow))
    //     //             {
    //     //                 popUpIndex++;
    //     //             }
    //     //             Debug.Log($"Tutorial part 1 started");
    //     //             break;
    //     //         case 1:
    //     //             if (Input.GetKeyDown(KeyCode.RightArrow))
    //     //             {
    //     //                 popUpIndex++;
    //     //             }
    //     //             Debug.Log($"Tutorial part 2 started");
    //     //             break;
    //     //         default:
    //     //             Debug.Log($"popUpIndex: {popUpIndex}, not a valid popUpIndex");
    //     //             break;
    //     //     }
    //     // }
    // }
}
