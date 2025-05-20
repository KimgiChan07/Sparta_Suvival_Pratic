using System;
using UnityEngine;

public class Player : MonoBehaviour
{
    public PlayerController  playerController;
    public PlayerCondition playerCondition;
    public Equipment equipment;
    
    public ItemData itemData;
    public Action addItem;

    public Transform dropPosition;

    private void Awake()
    {
        CharacterManager.Instance.Player = this;
        playerController = GetComponent<PlayerController>();
        playerCondition = GetComponent<PlayerCondition>();
        equipment = GetComponent<Equipment>();
    }
    
}
