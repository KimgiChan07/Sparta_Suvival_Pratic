using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ItemSlot : MonoBehaviour
{
    public ItemData item;
    
    [Header("UI")]
    public Button button;
    public Image icon;
    public TextMeshProUGUI quantityText;
    public UIInventory  inventory;
    private Outline outline;

    public int index;
    public bool equipped;
    public int quantity;


    private void Awake()
    {
        outline = GetComponent<Outline>();
    }

    private void OnEnable()
    {
        outline.enabled = equipped;
    }

    public void Set()
    {
        icon.gameObject.SetActive(true);
        icon.sprite = item.icon;
        quantityText.text = quantity > 1 ? quantity.ToString() : String.Empty;

        if (outline != null)
        {
            outline.enabled = equipped;
        }
    }

    public void Clear()
    {
        item = null;
        icon.gameObject.SetActive(false);
        quantityText.text = String.Empty;
    }

    public void OnClickButton()
    {
        inventory.SelectItem(index);
    }
}
