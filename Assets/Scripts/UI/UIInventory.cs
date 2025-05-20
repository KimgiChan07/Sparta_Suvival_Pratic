using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using Random = UnityEngine.Random;

public class UIInventory : MonoBehaviour
{
    public ItemSlot[] slots;
    public GameObject inventoryWindow;
    public Transform slotPanel;
    public Transform dropPos;

    [Header("Select Item")] public TextMeshProUGUI selectedItemName;
    public TextMeshProUGUI selectedItemNameDescription;
    public TextMeshProUGUI selectedItemStatName;
    public TextMeshProUGUI selectedItemStatValue;

    public GameObject useButton;
    public GameObject equipButton;
    public GameObject unEquipButton;
    public GameObject dropButton;

    private PlayerCondition playercondition;
    private PlayerController playerController;
    
    ItemData selectedItemData;
    private int selectedItemIndex;

    private int curEquipIndex;

    private void Start()
    {
        playerController = CharacterManager.Instance.player.playerController;
        playercondition = CharacterManager.Instance.player.playerCondition;
        dropPos = CharacterManager.Instance.player.dropPosition;

        playerController.inventoryAction += Toggle;
        CharacterManager.Instance.player.addItem += AddItem;

        inventoryWindow.SetActive(false);
        slots = new ItemSlot[slotPanel.childCount];

        for (int i = 0; i < slots.Length; i++)
        {
            slots[i] = slotPanel.GetChild(i).GetComponent<ItemSlot>();
            slots[i].index = i;
            slots[i].inventory = this;

        }

        ClearSelectedItemWindow();
    }

    void ClearSelectedItemWindow()
    {
        selectedItemName.text = String.Empty;
        selectedItemNameDescription.text = String.Empty;
        selectedItemStatName.text = String.Empty;
        selectedItemStatValue.text = String.Empty;

        useButton.SetActive(false);
        equipButton.SetActive(false);
        unEquipButton.SetActive(false);
        dropButton.SetActive(false);
    }

    public void Toggle()
    {
        if (isOpen())
        {
            inventoryWindow.SetActive((false));
        }
        else
        {
            inventoryWindow.SetActive((true));
        }
    }

    public bool isOpen()
    {
        return inventoryWindow.activeInHierarchy;
    }

    void AddItem()
    {
        ItemData item = CharacterManager.Instance.player.itemData;

        //아이템 중복 가능한지 canStack체크
        //아닐시 비어있는 슬롯을 가져온다.

        if (item.canStack)
        {
            ItemSlot slot = GetItemStack(item);
            if (slot != null)
            {
                slot.quantity++;
                UpdateUI();
                CharacterManager.Instance.player.itemData = null;
                return;
            }
        }

        ItemSlot emptySlot = GetEmptySlot();

        if (emptySlot != null)
        {
            emptySlot.item = item;
            emptySlot.quantity = 1;
            UpdateUI();
            CharacterManager.Instance.player.itemData = null;
            return;
        }

        ThrowItem(item);

        CharacterManager.Instance.player.itemData = null;
    }

    void UpdateUI()
    {
        for (int i = 0; i < slots.Length; i++)
        {
            if (slots[i].item != null)
            {
                slots[i].Set();
            }
            else
            {
                slots[i].Clear();
            }
        }
    }

    ItemSlot GetItemStack(ItemData _item)
    {
        for (int i = 0; i < slots.Length; i++)
        {
            if (slots[i].item == _item && slots[i].quantity < _item.maxStackAmount)
            {
                return slots[i];
            }
        }

        return null;
    }

    ItemSlot GetEmptySlot()
    {
        for (int i = 0; i < slots.Length; i++)
        {
            if (slots[i].item == null)
            {
                return slots[i];
            }
        }

        return null;
    }

    void ThrowItem(ItemData _item)
    {
        Instantiate(_item.dropPrefab, dropPos.position, Quaternion.Euler(Vector3.one * Random.value * 360));
    }

    public void SelectItem(int _index)
    {
        if(slots[_index].item == null)
            return;
        
        selectedItemData=slots[_index].item;
        selectedItemIndex = _index;
        selectedItemName.text = selectedItemData.displayName;
        selectedItemNameDescription.text = selectedItemData.description;

        selectedItemStatName.text = string.Empty;
        selectedItemStatValue.text = string.Empty;

        for (int i = 0; i < selectedItemData.consumables.Length; i++)
        {
            selectedItemStatName.text+=selectedItemData.consumables[i].type.ToString()+"\n";
            selectedItemStatValue.text += selectedItemData.consumables[i].value.ToString()+"\n";
        }
        
        useButton.SetActive(selectedItemData.type==ItemType.Consumable);
        equipButton.SetActive(selectedItemData.type==ItemType.Equipable&&!slots[_index].equipped);
        unEquipButton.SetActive(selectedItemData.type==ItemType.Equipable&&slots[_index].equipped);
        dropButton.SetActive(true);
    }

    public void OnUseButton()
    {
        if (selectedItemData.type == ItemType.Consumable)
        {
            for (int i = 0; i < selectedItemData.consumables.Length; i++)
            {
                switch (selectedItemData.consumables[i].type)
                {
                    case ConsumableType.Health:
                        playercondition.Heal(selectedItemData.consumables[i].value);
                        break;
                    case ConsumableType.Hunger:
                        playercondition.Eat(selectedItemData.consumables[i].value);
                        break;
                }
            }
            RemoveSelectedItem();
        }
    }

    public void OnDropButton()
    {
        ThrowItem(selectedItemData);
        RemoveSelectedItem();
    }

    void RemoveSelectedItem()
    {
        slots[selectedItemIndex].quantity--;

        if (slots[selectedItemIndex].quantity <= 0)
        {
            selectedItemData = null;
            slots[selectedItemIndex].item = null;
            selectedItemIndex = -1;
            ClearSelectedItemWindow();
            
        }
        UpdateUI();
    }

    public void OnEquipButton()
    {
        if (slots[curEquipIndex].equipped)
        {
            UnEquip(curEquipIndex);
        }
        
        slots[selectedItemIndex].equipped = true; 
        curEquipIndex = selectedItemIndex;
        CharacterManager.Instance.player.equipment.EquipNew(selectedItemData);
        UpdateUI();
        
        SelectItem(selectedItemIndex);
    }

    void UnEquip(int _index)
    {
        slots[_index].equipped = false;
        CharacterManager.Instance.player.equipment.UnEquip();
        UpdateUI();

        if (selectedItemIndex == _index)
        {
            SelectItem(selectedItemIndex);
        }
    }

    public void OnUnEquipButton()
    {
        UnEquip(selectedItemIndex);
    }
}
