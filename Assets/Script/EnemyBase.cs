using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.AI;

public class EnemyBase : MonoBehaviour
{
    //health
    [SerializeField] float health;
    [SerializeField] string swordTag;
    [SerializeField] float deathTime;
    [SerializeField] bool deade = false;

    //player info
    [SerializeField] GameObject player;
    [HideInInspector] PlayerCombat PlayerCombat;

    //movment
    [SerializeField] NavMeshAgent agent;
    [HideInInspector] Animator animator;   
    [SerializeField] bool isOfMash;
    [SerializeField] float timeJumping = .7f;
    [SerializeField] float timeAtacing = .8f;
    [SerializeField] float distensFromePlayer;
    public bool canAttack = false;
    public bool isAttacking = false;

    private void Start()
    {
        PlayerCombat = player.GetComponent<PlayerCombat>();
        agent = GetComponent<NavMeshAgent>();
        animator = GetComponent<Animator>();
    }
    private void OnValidate()
    {
        if (agent != null) agent = GetComponent<NavMeshAgent>();
        if (animator != null) animator = GetComponent<Animator>();
    }

    public void TakeDamage(float amount)
    {
        health -= amount;
        animator.CrossFade("hit", .2f);
        if (health <= 0f)
        {
            Die();
        }
    }
    private void Die()
    {
        // Handle player death
        animator.CrossFade("death", .2f);
        Debug.Log("boss 1 Died");
    }

    // Update is called once per frame
    void Update()
    {
        if (agent.hasPath && !isAttacking)
        {
            if (agent.isOnOffMeshLink)
            {
                if (!isOfMash)
                {
                    isOfMash = true;
                    var link = agent.currentOffMeshLinkData;
                    StartCoroutine(DoOfMashLink(link));
                }
            }
            else
            {
                isOfMash = false;
                var dir = (agent.steeringTarget - transform.position).normalized;
                var animDir = transform.InverseTransformDirection(dir);
                var isFacingMoveDirection = Vector3.Dot(dir, transform.forward) > .5f;

                animator.SetFloat("Horizontal", isFacingMoveDirection ? animDir.x : 0, .5f, Time.deltaTime);
                animator.SetFloat("Vertical", isFacingMoveDirection ? animDir.z : 0, .5f, Time.deltaTime);

                transform.rotation = Quaternion.RotateTowards(transform.rotation, Quaternion.LookRotation(dir), 180 * Time.deltaTime);

                if (agent.remainingDistance <= agent.stoppingDistance)
                {
                    agent.ResetPath();
                    canAttack = true;
                }
                else
                {
                    canAttack = false;
                }
            }
            
        }
        else
        {
            animator.SetFloat("Horizontal", 0, .25f, Time.deltaTime);
            animator.SetFloat("Vertical", 0, .25f, Time.deltaTime);
        }
        if (!isAttacking && agent.enabled)
        {
            agent.SetDestination(player.transform.position);
        }
    }

    IEnumerator DoOfMashLink(OffMeshLinkData link)
    {
        animator.CrossFade("jump", .2f);
        var time = timeJumping;
        var totalTime = time;

        while (time > 0)
        {
            time = Mathf.Max(0, time - Time.deltaTime);
            transform.position = Vector3.Lerp(link.startPos, link.endPos, 1 - time / totalTime);
            yield return new WaitForSeconds(0);
        }
        agent.CompleteOffMeshLink();
    }

    private void OnDrawGizmos()
    {
        if (agent.hasPath)
        {
            for (var i = 0; i < agent.path.corners.Length -1; i++)
            {
                Debug.DrawLine(agent.path.corners[i], agent.path.corners[i + 1], Color.blue);
            }
        }
    }
}

