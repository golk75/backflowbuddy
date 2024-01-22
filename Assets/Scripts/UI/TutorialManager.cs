using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.InputSystem.Layouts;
using UnityEngine.UIElements;

public class TutorialSystem : MonoBehaviour
{

    //come back to visit array loop..
    public GameObject[] popUps;
    int popUpIndex;

    //constants
    private const string PopUpString = "TutorialPopUp";
    public GameObject m_GameUi;
    public UIDocument root;

    public VisualElement m_PopUp;

    /// <summary>
    /// Awake is called when the script instance is being loaded.
    /// </summary>
    private void Awake()
    {
        root = m_GameUi.GetComponent<UIDocument>();
        m_PopUp = root.rootVisualElement.Q<VisualElement>(PopUpString);
    }
    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.RightArrow))
        {
            Debug.Log($"{m_PopUp.ElementAt(0)}");
            // m_PopUp.style.visibility = Visibility.Visible;
        }
        //come back to visit array loop
        // {
        //     for (int i = 0; i < popUps.Length; i++)
        //     {
        //         if (i == popUpIndex)
        //         {
        //             popUps[popUpIndex].SetActive(true);
        //         }
        //         else
        //         {
        //             popUps[popUpIndex].SetActive(false);
        //         }
        //     }
        //     switch (popUpIndex)
        //     {
        //         case 0:
        //             if (Input.GetKeyDown(KeyCode.RightArrow))
        //             {
        //                 popUpIndex++;
        //             }
        //             Debug.Log($"Tutorial part 1 started");
        //             break;
        //         case 1:
        //             if (Input.GetKeyDown(KeyCode.RightArrow))
        //             {
        //                 popUpIndex++;
        //             }
        //             Debug.Log($"Tutorial part 2 started");
        //             break;
        //         default:
        //             Debug.Log($"popUpIndex: {popUpIndex}, not a valid popUpIndex");
        //             break;
        //     }
        // }
    }
}
