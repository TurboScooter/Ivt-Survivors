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

    public void TakeDamage(int damage)
    {
        hitPoints -= damage;
        if (hitPoints <= 0) 
        Destroy(gameObject);
    }
}
