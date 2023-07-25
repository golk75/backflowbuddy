using System.Collections;
using System.Collections.Generic;
using com.zibra.liquid.Manipulators;
using UnityEngine;

[CreateAssetMenu(fileName = "Data", menuName = "TestCock/List", order = 1)]
public class TestCock : ScriptableObject
{
    public OperableComponentDescription description;
    public ZibraLiquidCollider testCockCollider;
    public ZibraLiquidVoid testCockVoid;
}
