using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class player : MonoBehaviour
{
   public PlayerBase playerBase;
    public ShopScriptableObj shop;
    float finalDamage;
    public float attack;
    public float attackSpeed;
    public float hitPoints;
    public float movementSpeed;
    public float pickUpRadius;
    public float critRate;
    public float critDamage;
    public float areaOfEffect;
    public float collectedCoins;

    void Start()
    {
        attack = playerBase.attack;
        attackSpeed = playerBase.attackSpeed;
        hitPoints = playerBase.hitPoints; 
        movementSpeed = playerBase.movementSpeed;
        pickUpRadius = playerBase.pickUpRadius;
        critRate = playerBase.critRate; 
        critDamage = playerBase.critDamage;
        areaOfEffect = playerBase.areaOfEffect;
    }

    private void FixedUpdate()
    {
        Movement();
    }
    public void Movement()
    {
        Vector3 moveForward = Input.GetAxis("Vertical") * transform.forward;
        Vector3 moveHorizontal = Input.GetAxis("Horizontal") * transform.right;
        Vector3 Movement = moveForward + moveHorizontal;

        transform.position += Movement * playerBase.movementSpeed * Time.deltaTime;
    }
    public void DamageTaken(float damage)
    {
        playerBase.hitPoints -= damage;
        if(playerBase.hitPoints <= 0)
        {
            shop.coins += collectedCoins;
            SceneManager.LoadScene(2);
        }
    }
    private void OnCollisionEnter(Collision collision)
    {
        if(collision.gameObject.CompareTag("Enemy"))
        DamageTaken(collision.gameObject.GetComponent<Enemy>().attackDamage);
    }

    
}
