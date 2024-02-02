using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class AssemblyManager : MonoBehaviour
{
    public GameObject m_DCWaterManager;
    public GameObject m_RPZWaterManager;
    // Start is called before the first frame update
    void Start()
    {
        if (SceneManager.GetActiveScene().name == "RPZPlayScene_testing")
        {
            m_DCWaterManager.SetActive(false);
            m_RPZWaterManager.SetActive(true);
        }
        else
        {
            m_DCWaterManager.SetActive(true);
            m_RPZWaterManager.SetActive(false);
        }
    }

    // Update is called once per frame
    void Update()
    {

    }
}
