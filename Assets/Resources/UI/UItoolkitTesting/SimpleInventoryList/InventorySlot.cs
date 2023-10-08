using System;
using System.Collections;
using System.Collections.Generic;
using JetBrains.Annotations;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UIElements;

public class InventorySlot
{
    public Button button;
    public Item item;
    public StyleSheet styleSheet;

    public InventorySlot(Item item, VisualTreeAsset template)
    {
        // TemplateContainer itemButtonContainer = template.CloneTree();
        TemplateContainer itemButtonContainer = template.Instantiate();

        button = itemButtonContainer.Q<Button>();

        button.style.backgroundImage = new StyleBackground(item.icon);
        Debug.Log($"{itemButtonContainer.styleSheets}");
        this.item = item;

        button.RegisterCallback<ClickEvent>(OnClick);
    }

    private void OnClick(ClickEvent evt)
    {
        Debug.Log($"the itemslot with the item name" + item.displayName + "has been clicked");
    }
}
