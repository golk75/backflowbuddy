using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Item", menuName = "simpleInventoryList / Item")]
public class Item : ScriptableObject
{
    public string displayName;
    public Sprite icon;
}
