
using com.zibra.liquid.DataStructures;
using com.zibra.liquid.Solver;
using UnityEngine;
using UnityEngine.SceneManagement;


public class MainMenuScreenController : MonoBehaviour
{
    [Tooltip("String IDs to query Visual Elements")]
    [SerializeField] string m_DCTestScene = "DCTestScene";
    // [SerializeField] string m_DCTestScene_BuildOut_water = "DCTestScene_BuildOut_water";
    public ZibraLiquidSolverParameters solverParameters;
    public ZibraLiquid liquid;
    private void OnEnable()
    {
        MainMenuScreen.GamePlayed += OnPlayGame;
        MainMenuScreen.GameQuit += OnQuitGame;
    }

    private void OnQuitGame()
    {
#if UNITY_EDITOR
        if (Application.isPlaying)
#endif
            Application.Quit();

    }

    void OnPlayGame()
    {
        // #if UNITY_EDITOR
        //         if (Application.isPlaying)
        // #endif
        // Application.Quit();
        // SceneManager.LoadSceneAsync(m_DCTestScene);
        liquid.ReleaseSimulation();
        SceneManager.LoadScene(m_DCTestScene);




    }
    //SceneManager.LoadSceneAsync(m_DCTestScene_BuildOut_water);
    void OnDisable()
    {

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
