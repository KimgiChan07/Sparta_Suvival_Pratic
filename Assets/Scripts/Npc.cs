using System.Collections;
using UnityEngine.AI;
using UnityEngine;
using Random = UnityEngine.Random;

public enum AIState
{
    Idle,
    Wandering,
    Attacking,
}

public class Npc : MonoBehaviour, IDamageIbe
{
    private static readonly int IsMoving = Animator.StringToHash("IsMoving");
    private static readonly int Attack = Animator.StringToHash("Attack");

    [Header("Stat")] 
    public int health;
    public float walkSpeed;
    public float runSpeed;
    public ItemData[] dropOnDeath;

    [Header("AI")] 
    private NavMeshAgent agent;
    public float detectDistance;
    private AIState aiState;
    
    [Header("Wandering")]
    public float minWanderDistance;
    public float maxWanderDistance;
    public float minWanderWaitTime;
    public float maxWanderWaitTime;

    [Header("Combat")] public int damage;
    public float attackRate;
    private float lastAttackTime;
    public float attackDistance;

    private float playerDistance;
    public float fieldOfView=120f;
    
    private Animator animator;
    private SkinnedMeshRenderer[] meshRenderer;
    private IDamageIbe _damageIbeImplementation;

    private void Awake()
    {
        agent = GetComponent<NavMeshAgent>();
        animator = GetComponent<Animator>();
        meshRenderer = GetComponentsInChildren<SkinnedMeshRenderer>();
    }

    private void Start()
    {
        SetState(AIState.Wandering);
    }

    private void Update()
    {
        playerDistance = Vector3.Distance(transform.position,CharacterManager.Instance.player.transform.position);
        animator.SetBool(IsMoving, aiState != AIState.Idle);

        switch (aiState)
        {
            case AIState.Idle:
            case AIState.Wandering:
                PassiveUpdate();
                break;
            case AIState.Attacking:
                AttackingUpdate();
                break;
        }
    }

    public void SetState(AIState _aiState)
    {
        aiState = _aiState;

        switch (aiState)
        {
            case AIState.Idle:
                agent.speed = walkSpeed;
                agent.isStopped = true;
                break;
            case AIState.Wandering:
                agent.speed = walkSpeed;
                agent.isStopped = false;
                break;
            case AIState.Attacking:
                agent.speed = runSpeed;
                agent.isStopped = false;
                break;
        }

        animator.speed = agent.speed / walkSpeed;
    }

    void PassiveUpdate()
    {
        if (aiState == AIState.Wandering && agent.remainingDistance < 0.1f)
        {
            SetState(AIState.Idle);
            Invoke(nameof(WanderToNewLocation),Random.Range(minWanderWaitTime,maxWanderWaitTime));
        }

        if (playerDistance < detectDistance)
        {
            SetState(AIState.Attacking);
        }
    }

    void WanderToNewLocation()
    {
        if (aiState != AIState.Idle) return;
        
        SetState(AIState.Wandering);
        agent.SetDestination(GetWanderLocation());
    }

    Vector3 GetWanderLocation()
    {
        NavMeshHit hit;
        
        NavMesh.SamplePosition(transform.localPosition+
                               (Random.onUnitSphere*Random.Range(minWanderDistance,maxWanderDistance)),
            out hit,maxWanderDistance,NavMesh.AllAreas);

        int i = 0;
        
        
        while (Vector3.Distance(transform.position,hit.position) < detectDistance)
        {
            NavMesh.SamplePosition(transform.localPosition+
                                   (Random.onUnitSphere*Random.Range(minWanderDistance,maxWanderDistance)),
                out hit,maxWanderDistance,NavMesh.AllAreas);
            i++;
            if (i == 30) break;
        }
        return hit.position;
    }

    void AttackingUpdate()
    {
        if (playerDistance < attackDistance && isPlayerInFieldOfView())
        {
            agent.isStopped = true;
            if (Time.time - lastAttackTime > attackRate)
            {
                lastAttackTime = Time.time;
                CharacterManager.Instance.player.playerController.GetComponent<IDamageIbe>().TakePhysicalDamage(damage);
                animator.speed = 1;
                animator.SetTrigger(Attack);
            }
        }
        else
        {
            if (playerDistance < detectDistance)
            {
                agent.isStopped = false;
                NavMeshPath path = new NavMeshPath();
                if (agent.CalculatePath(CharacterManager.Instance.player.transform.position, path))
                {
                    agent.SetDestination(CharacterManager.Instance.player.transform.position);
                }
                else
                {
                    agent.SetDestination(transform.position);
                    agent.isStopped = true;
                    SetState(AIState.Wandering);

                }
            }
            else
            {
                agent.SetDestination(transform.position);
                agent.isStopped = true;
                SetState(AIState.Wandering);
            }
        }
    }
    bool isPlayerInFieldOfView()
    {
        Vector3 directionToPlayer=CharacterManager.Instance.player.transform.position - transform.position;
        float angle = Vector3.Angle(transform.forward, directionToPlayer);
        return angle<fieldOfView *0.5f;
    }

    public void TakePhysicalDamage(int _damage)
    {
        health -= _damage;
        if (health <= 0)
        {
            Die();
        }

        StartCoroutine(DamageFlash());
    }

    void Die()
    {
        for (int i = 0; i < dropOnDeath.Length; i++)
        {
            Instantiate(dropOnDeath[i].dropPrefab, transform.position + Vector3.up * 2, Quaternion.identity);
        }
        Destroy(gameObject);
    }

    IEnumerator DamageFlash()
    {
        for (int i = 0; i < meshRenderer.Length; i++)
        {
            meshRenderer[i].material.color = Color.red;
        }
        yield return new WaitForSeconds(0.1f);

        for (int i = 0; i < meshRenderer.Length; i++)
        {
            meshRenderer[i].material.color = Color.white;
        }
    }
}
