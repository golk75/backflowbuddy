using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;
public class PausePlayButton : MonoBehaviour
{

    Button m_PauseButton;
    Button m_PlayButton;
    const string PauseButtonString = "PauseButton";
    const string PlayButtonString = "PlayButton";
    /// <summary>
    /// Start is called on the frame when a script is enabled just before
    /// any of the Update methods is called the first time.
    /// </summary>
    private void Start()
    {
        var root = GetComponent<UIDocument>();
        m_PauseButton = root.rootVisualElement.Q<Button>(PauseButtonString);
        m_PlayButton = root.rootVisualElement.Q<Button>(PlayButtonString);

        m_PauseButton.clicked += PauseGame;
        m_PlayButton.clicked += ResumeGame;
    }
    void PauseGame()
    {
        Time.timeScale = 0;
    }
    void ResumeGame()
    {
        Time.timeScale = 1;
    }
}
