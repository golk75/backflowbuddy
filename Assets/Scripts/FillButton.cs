using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FillButton : MonoBehaviour
{
    public GameObject Check1;
    public GameObject Check2;

    // Start is called before the first frame update
    void Start() { }

    public void FillDevice()
    {
        Check1.transform.localPosition = new Vector3(-0.10f, 0, -0.08f);
        Check2.transform.localPosition = new Vector3(-0.20f, -2.25f, -0.17f);
    }

    // Update is called once per frame
    void Update() { }
}
