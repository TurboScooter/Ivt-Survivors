using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    [SerializeField] float hitPoints;
    [SerializeField] float movementSpeed;
    public float attackDamage;
    [SerializeField] GameObject target;
    [SerializeField] GameObject experienceOrb;
    void Start()
    {
        
    }

    void Update()
    {
        EnemyAi();
    }

    public void EnemyAi()
    {
        
    }

    public void TakeDamage(float damage)
    {
        hitPoints -= damage;
        if (hitPoints <= 0) 
        {
            Instantiate(experienceOrb);
            Destroy(gameObject);
        }
    }



}
