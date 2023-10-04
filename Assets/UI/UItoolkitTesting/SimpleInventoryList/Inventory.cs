using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class Inventory : MonoBehaviour
{
    UIDocument inventoryUiDocument;
    public VisualTreeAsset itemButtonTemplate;
    public List<Item> items;

    // Start is called before the first frame update
    /// <summary>
    /// This function is called when the object becomes enabled and active.
    /// </summary>
    private void OnEnable()
    {
        inventoryUiDocument = GetComponent<UIDocument>();

        foreach (Item item in items)
        {
            InventorySlot newSlot = new InventorySlot(item, itemButtonTemplate);
            inventoryUiDocument.rootVisualElement.Q("ItemRow").Add(newSlot.button);
        }
    }

    // Update is called once per frame
    void Update() { }
}
