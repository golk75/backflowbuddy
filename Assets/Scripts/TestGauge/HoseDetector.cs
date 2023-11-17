using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Events;

public class HoseDetector : MonoBehaviour
{
    public TestCockController testCockController;
    public TestKitManager testKitManager;
    public GameObject testCock;
    public GameObject currentHoseConnection;
    public GameObject currentHose;
    public bool isConnectionAttempting;
    public bool isConnected = false;
    Coroutine onAttachAttempt;

    public PlayerController playerController;
    public OperableComponentDescription currentTestCockDescription;
    public OperableComponentDescription currentHoseDescription;
    private Collider boxCollider;
    private Coroutine InitialColliderBlock;

    public CameraController cameraController;
    void OnEnable()
    {
        boxCollider = GetComponent<Collider>();
    }
    void Start()
    {
        InitialColliderBlock = StartCoroutine(HideCollider());
        currentTestCockDescription = GetComponent<OperableComponentDescription>();
    }
    /// <summary>
    /// If collider is enabled at opening of scene, the hosebib gets thrown around due to the hosbib config joint moving the bib through the collider @Start
    /// </summary>
    /// <returns></returns>
    private IEnumerator HideCollider()
    {
        yield return new WaitForSeconds(1);
        if (boxCollider.enabled == false)
        {
            boxCollider.enabled = true;
        }

    }
    public void OnTriggerEnter(Collider other)
    {
        currentHoseDescription = other.GetComponent<OperableComponentDescription>();

        if (
            currentHoseDescription.partsType == OperableComponentDescription.PartsType.TestKitHose ||
            currentHoseDescription.partsType == OperableComponentDescription.PartsType.TestKitSightTube
           )
        {
            //this will keep the connected hose for this test cock protected from changing when other hoses/ colliders enter.
            //currentHoseConnection will only become null after a hose is removed from this.gameObject (testCockDetector).
            if (currentHoseConnection == null)
                currentHoseConnection = other.gameObject;
        }

        if (cameraController.isPanning == false)
        {

            onAttachAttempt = StartCoroutine(AttachInitiate());
            Actions.onTestCockColliderEnter?.Invoke(this.gameObject, currentTestCockDescription);

        }

        isConnectionAttempting = true;
    }

    private void OnTriggerExit(Collider other)
    {
        isConnectionAttempting = false;

        /// <summary>
        ///     THESE WILL NOT WORK CORRECTLY IF TESTKITMANAGER'S LISTS ARE EXPANDED IN THE EDITOR ISNPECTOR! ---------------------------------------------->>
        /// </summary>

        // check if the hose is removable or just passing through or is being mistakenly attached to a test cock that is alreay hooked up to another hose
        if (testKitManager.AttachedHoseList.Contains(other.gameObject) && testKitManager.AttachedTestCockList.Contains(this.gameObject))
        {
            Actions.onRemoveHoseFromList?.Invoke(currentHoseConnection, other.GetComponent<OperableComponentDescription>());
            Actions.onRemoveTestCockFromList?.Invoke(this.gameObject, GetComponent<OperableComponentDescription>());
            currentHoseConnection = null;
            isConnected = false;
        }




    }

    IEnumerator AttachInitiate()
    {
        //check if test cock and hose are already in lists @TestKitManager, if they are, then do not add them again ---->
        if (!testKitManager.AttachedTestCockList.Contains(this.gameObject) && !testKitManager.AttachedHoseList.Contains(currentHoseConnection))
        {
            yield return new WaitForSeconds(0.5f);
            if (isConnectionAttempting == true)
            {

                Actions.onAddTestCockToList?.Invoke(this.gameObject, currentTestCockDescription);
                Actions.onAddHoseToList?.Invoke(currentHoseConnection, currentHoseDescription);
                isConnected = true;

                if (currentHoseConnection.GetComponent<OperableComponentDescription>().partsType == OperableComponentDescription.PartsType.TestKitHose)
                {
                    // and do not set their position------------
                    Actions.onHoseConnect?.Invoke(this.gameObject, currentTestCockDescription);
                }
                else if (currentHoseConnection.GetComponent<OperableComponentDescription>().partsType == OperableComponentDescription.PartsType.TestKitSightTube)
                {

                    Actions.onSightTubeConnect?.Invoke(currentHoseConnection, currentTestCockDescription);
                }


            }

        }


    }
}
