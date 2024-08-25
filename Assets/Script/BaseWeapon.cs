using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BaseWeapon : MonoBehaviour
{
    [SerializeField] float swardDameg;

    //player info
    [SerializeField] GameObject player;
    [HideInInspector] PlayerCombat PlayerCombat;

    private void OnTriggerEnter(Collider other)
    {
        if (other != null && other.GetComponent<EnemyBase>() && PlayerCombat.isAttackung)
        {
            var enemyBase = other.GetComponent<EnemyBase>();
            enemyBase.TakeDamage(swardDameg);
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        PlayerCombat = player.GetComponent<PlayerCombat>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
