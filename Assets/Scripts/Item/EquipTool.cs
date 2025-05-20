using System;
using UnityEngine;


public class EquipTool : Equip
{
    private static readonly int Attack = Animator.StringToHash("Attack");
    public float attackRate;
    private bool attacking;
    public float attackDistance;
    public float useStamina;
    
    [Header("Resource Gathering")]
    public bool doesGatherResource;

    [Header("Combat")] 
    public bool doesDealDamage;
    public int damage;

    private Animator animator;
    private Camera cam;

    private void Start()
    {
        animator = GetComponent<Animator>();
        cam = Camera.main;
    }

    public override void OnAttackInput()
    {
        if (!attacking)
        {
            if (CharacterManager.Instance.player.playerCondition.UseStamina(useStamina))
            {
                attacking = true;
                animator.SetBool(Attack, true);
                Invoke("OnCanAttack", attackRate);
            }
        }
    }

    void OnCanAttack()
    {
        attacking = false;
    }

    public void OnHit()
    {
        Ray ray = cam.ScreenPointToRay(new Vector3(Screen.width / 2, Screen.height / 2, 0));
        RaycastHit hit;
        if (Physics.Raycast(ray, out hit, attackDistance))
        {
            if (doesGatherResource && hit.collider.TryGetComponent(out Resource resource))
            {
                resource.Gather(hit.point, hit.normal);
            }
        }
    }
}
