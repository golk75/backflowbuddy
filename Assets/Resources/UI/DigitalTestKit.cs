using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UIElements;

public class DigitalTestKit : MonoBehaviour
{

    private VisualElement _root;
    private VisualElement _gaugeProgressBar;
    public VisualTreeAsset tickGroupAsset;
    public GameObject reading;

    public Vector3 initCubePos;
    private float initCubeX;



    // Start is called before the first frame update
    void Start()
    {
        _root = GetComponent<UIDocument>().rootVisualElement;
        _gaugeProgressBar = _root.Q<VisualElement>("Gauge_progress_bar");
        // MoveDigitalTestKitProgressBar(100);
        // TemplateContainer tickGroupContainerTemp = tickGroupAsset.Instantiate();
        // _root.Q("Gauge_tick_container").Add(tickGroupContainerTemp);

        // initCubeX = cube.transform.position.x;
        // initCubePos = cube.transform.position;



    }

    public void MoveDigitalTestKitProgressBar(float val)
    {
        // _gaugeProgressBar.style.width = Length.Percent(val);
    }
    // Update is called once per frame
    void Update()
    {
        // if (cube.transform.position != initCubePos)
        // {
        //     Debug.Log($"Cube has moved {initCubeX - cube.transform.position.x} units");
        // }
    }
}
