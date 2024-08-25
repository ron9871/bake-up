
using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.AI;

public class Boss1 : MonoBehaviour
{
    // Jump attack
    [SerializeField] float jumpForce = 10f;
    [SerializeField] float jumpTime = 1f;
    [SerializeField] float waveRadius = 5f;
    [SerializeField] float damage = 20f;
    [SerializeField] LayerMask playerLayer;
    [SerializeField] LayerMask ground;
    [SerializeField] float cooldown = 5f;
    [SerializeField] ParticleSystem[] particleSystem = new ParticleSystem[0];
    [SerializeField] float delay = 3.0f;

    private bool inCooldown;
    private Rigidbody rb;
    private Animator animator;
    private EnemyBase enemyBase;
    private bool isGrounded;
    private NavMeshAgent agent;

    // Start is called before the first frame update
    void Start()
    {
        foreach (var particle in particleSystem)
            particle.Stop();
        rb = GetComponent<Rigidbody>();
        animator = GetComponent<Animator>();
        enemyBase = GetComponent<EnemyBase>();
        agent = GetComponent<NavMeshAgent>();
        inCooldown = false;
    }

    private void Update()
    {
/*        if (enemyBase.canAttack && !enemyBase.isAttacking)*/
        isGrounded = Physics.Raycast(transform.position, Vector3.down, 3f, ground);
        if (enemyBase.canAttack && !enemyBase.isAttacking && !inCooldown)
        {
            PerformJumpAttack();
            StartCoroutine(Attacking(jumpTime));
        }
    }

    IEnumerator Attacking(float time)
    {
        enemyBase.isAttacking = true;
        yield return new WaitForSeconds(time);
        enemyBase.isAttacking = false;
        inCooldown = true;
        StartCoroutine(CooldownRoutine());
    }

    IEnumerator CooldownRoutine()
    {
        yield return new WaitForSeconds(cooldown);
        inCooldown = false;
    }

    public void PerformJumpAttack()
    {
        if (isGrounded)
        {
            // Disable the NavMeshAgent
            agent.enabled = false;

            // Make the boss jump
            rb.AddForce(Vector3.up * jumpForce, ForceMode.Impulse);

            // Trigger jump animation
            animator.CrossFade("jump", 0.1f);
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Ground") && enemyBase.isAttacking)
        {
            // Trigger ground hit animation
            animator.CrossFade("GroundHit", 0.1f);

            StartCoroutine(PlayParticleSystemWithDelay());

            // Re-enable the NavMeshAgent
            agent.enabled = true;

        }
    }
    IEnumerator PlayParticleSystemWithDelay()
    {
        yield return new WaitForSeconds(delay);

        foreach (var particle in particleSystem)
            particle.Play();
        
        // Call method to create the energy wave
        CreateEnergyWave();
    }

    private void CreateEnergyWave()
    {
        Collider[] hitPlayers = Physics.OverlapSphere(transform.position, waveRadius, playerLayer);

        foreach (Collider player in hitPlayers)
        {
            PlayerCombat playerCombat = player.GetComponent<PlayerCombat>();
            if (playerCombat != null)
            {
                playerCombat.TakeDamage(damage);
            }
            else
            {
                Debug.Log("PlayerCombat component is null");
            }
        }

        // Visual or particle effect for the energy wave
        // Instantiate(waveEffectPrefab, transform.position, Quaternion.identity);
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, waveRadius);
    }
}

