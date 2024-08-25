using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class PlayerCombat : MonoBehaviour
{
    //player informashen
    [SerializeField] Transform orientation;
    [SerializeField] float health;
    [SerializeField] float healingPotions;
    [SerializeField] KeyCode healingKey = KeyCode.E;
    [SerializeField] float healingAmount;

    //enemy informashen
    [SerializeField] LayerMask layerMask;

    //enemy tracking
    [SerializeField] float maxDistens;
    [SerializeField] float radius;
    [SerializeField] RaycastHit hit;
    [SerializeField] Animator animator;

    //attack info
    public bool isAttackung = false;


    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(healingKey))
        {
            TakehHealingPotion(healingAmount);
        }
        if (Physics.SphereCast(transform.position, radius, orientation.forward, out hit, maxDistens, layerMask))
        {
            
        }
    }
    public void TakeDamage(float amount)
    {
        if (healingPotions > 0)
        {
            health -= amount;
            animator.CrossFade("hit", .2f);
            if (health <= 0f)
            {
                Die();
            }
        }
    }
    private void Die()
    {
        // Handle player death
        animator.CrossFade("death", .2f);
        Debug.Log("Player Died");
    }
    public void TakehHealingPotion(float amount)
    {
        healingPotions -= 1;
        health += amount;
        animator.CrossFade("healing", .2f);
        if (health <= 0f)
        {
            Die();
        }
    }

    private void OnDrawGizmos()
    {
        if (hit.transform != null)
        {
            Gizmos.color = Color.red;
            Gizmos.DrawSphere(hit.transform.position, radius);
        }
    }
}
