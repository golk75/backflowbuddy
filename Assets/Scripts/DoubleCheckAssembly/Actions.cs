using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public static class Actions
{
    public static Action<GameObject, OperableComponentDescription> onHoseConnect;
    public static Action<GameObject, OperableComponentDescription> onObjectDisconnect;
    //adding component descriptions with these list manipulations, although they are not being used at the moment, may comeback and remove these Action parameters in the future if they are causing issues
    public static Action<GameObject, OperableComponentDescription> onAddTestCockToList;
    public static Action<GameObject, OperableComponentDescription> onRemoveTestCockFromList;
    public static Action<GameObject, OperableComponentDescription> onTestCockColliderEnter;
    public static Action<GameObject, OperableComponentDescription> onTestCockColliderExit;
    public static Action<GameObject, GameObject, OperableComponentDescription> onHoseColliderEnter;
    public static Action<GameObject, GameObject, OperableComponentDescription> onHoseColliderExit;

    public static Action<GameObject, OperableComponentDescription> onAddHoseToList;
    public static Action<GameObject, OperableComponentDescription> onRemoveHoseFromList;
    public static Action<GameObject, OperableComponentDescription> onHoseBibGrab;
    public static Action<GameObject, OperableComponentDescription> onHoseBibDrop;
    public static Action<GameObject, OperableComponentDescription> onHoseAttach;
    public static Action<GameObject, OperableComponentDescription> onHoseDetach;
    public static Action<GameObject, OperableComponentDescription> onHoseBibConnect;
    public static Action<GameObject, OperableComponentDescription> onHoseContact;


    public static Action<GameObject> onSightTubeGrab;
    public static Action<GameObject> onSightTubeDrop;
    public static Action<GameObject> onSightTubeAttach;
    public static Action<GameObject> onSightTubeDettach;
    public static Action<GameObject, OperableComponentDescription> onSightTubeConnect;
    // public static Action<GameObject, OperableComponentDescription> onHoseBibDisconnect;
    public static Action<GameObject> onCheckClosed;
    public static Action<GameObject> onCheckOpened;
    public static Action<bool, GameObject> onTestCockOpen;
    public static Action<GameObject> onCheck1Closed;
    public static Action<GameObject> onCheck1Opened;
    public static Action<GameObject> onCheck2Closed;
    public static Action<GameObject> onCheck2Opened;
    public static Action onTestCock1Opened;
    public static Action onTestCock1Closed;
    public static Action onTestCock2Opened;
    public static Action onTestCock2Closed;
    public static Action onTestCock3Opened;
    public static Action onTestCock3Closed;
    public static Action onTestCock4Opened;
    public static Action onTestCock4Closed;
    public static Action onHighBleedOperate;
    public static Action onHighBleedClosed;

    //ui
    public static Action<VisualElement> onPanelDrop;

    public static Action<float> onSupplyPressureInputChange;

}
