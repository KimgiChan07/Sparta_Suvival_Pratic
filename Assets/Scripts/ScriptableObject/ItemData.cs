using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum ItemType
{
    Equipable, //장착가능
    Consumable, //섭취가능
    Resource //자원들
}

public enum ConsumableType
{
    Health,
    Hunger
}
[System.Serializable]
public class ItemDataConsumable
{
    public ConsumableType type;
    public float value;
}

[CreateAssetMenu(fileName = "Item",menuName = "New Item")]
public class ItemData : ScriptableObject
{
    [Header("Info")] 
    public string displayName;
    public string description;
    public ItemType type;
    public Sprite icon;
    public GameObject dropPrefab;

    [Header("Stacking")] 
    public bool canStack;
    public int maxStackAmount;
    
    [Header("Consumable")]
    public ItemDataConsumable[] consumables;
    
    [Header("Equip")]
    public GameObject equipPrefab;
}
