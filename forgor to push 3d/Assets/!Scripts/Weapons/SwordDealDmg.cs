using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SwordDealDmg : MonoBehaviour
{
    [SerializeField] Sword sword;

    private void OnTriggerEnter(Collider collision)
    {
        if (collision.GetComponent<Collider>().CompareTag("Enemy"))
            collision.GetComponent<Enemy>().TakeDamage(sword.attack);

    }
}


