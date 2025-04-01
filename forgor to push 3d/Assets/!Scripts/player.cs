using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class player : MonoBehaviour
{
   public PlayerBase playerBase;
    float finalDamage;
    void Start()
    {
        
    }

    void Update()
    {
        
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
            Destroy(gameObject);
        }
    }
    private void OnCollisionEnter(Collision collision)
    {
        if(collision.gameObject.CompareTag("Enemy"))
        DamageTaken(collision.gameObject.GetComponent<Enemy>().attackDamage);
    }
}
