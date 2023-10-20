using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
public class GameScreenController : MonoBehaviour
{

    [Header("Scenes")]
    [SerializeField] string m_MainMenuSceneName = "MainMenu";

    /// <summary>
    /// This function is called when the object becomes enabled and active.
    /// </summary>
    private void OnEnable()
    {
        GameScreen.GameQuit += OnGameQuit;
    }
    /// <summary>
    /// This function is called when the behaviour becomes disabled or inactive.
    /// </summary>
    private void OnDisable()
    {

    }
    void OnGameQuit()
    {
        QuitGame();
    }
    void QuitGame()
    {
        // #if UNITY_EDITOR
        //         if (Application.isPlaying)
        // #endif
        // Application.Quit();
        // SceneManager.LoadSceneAsync(m_MainMenuSceneName);
        //SceneManager.LoadScene(m_MainMenuSceneName);
    }
    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }
}
