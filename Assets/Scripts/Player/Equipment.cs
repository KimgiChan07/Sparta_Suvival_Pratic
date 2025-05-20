using System;
using UnityEngine;
using UnityEngine.InputSystem;


public class Equipment : MonoBehaviour
{
    public Equip curEquip;
    public Transform equipParent; //장비장착 위치
    
    private PlayerController  playerController;
    private PlayerCondition playerCondition;

    private void Start()
    {
        playerController = GetComponent<PlayerController>();
        playerCondition = GetComponent<PlayerCondition>();
    }

    public void EquipNew(ItemData _itemData)
    {
        curEquip= Instantiate(_itemData.equipPrefab,equipParent).GetComponent<Equip>();
    }

    public void UnEquip()
    {
        if (curEquip != null)
        {
            Destroy(curEquip.gameObject);
            curEquip = null;
        }
    }

    public void OnAttackInput(InputAction.CallbackContext context)
    {
        if (context.phase == InputActionPhase.Performed && curEquip != null && playerController.canLook)
        {
            curEquip.OnAttackInput();
        }
    }
}