using System.Collections;
using System.Collections.Generic;
using System.Drawing;
using Unity.VisualScripting;
using UnityEngine;

public class FireWandDmg : MonoBehaviour
{
    [SerializeField] FireWand yes;
    [SerializeField] GameObject closestEnemy;
    [SerializeField] Vector2 point;
    [SerializeField] Vector2 moveTo;
    bool enemyChecked;
    [SerializeField] float distance;
    [SerializeField] float nearestDistance;
    [SerializeField] Vector3 nearestObject;
    [SerializeField] GameObject nearestGameObject;
    bool nearestFound = false;
    [SerializeField] float radius;
    LayerMask enemy;

    private void Start()
    {
        LocateClosest();
    }
    private void Update()
    {
        if(yes.duration <= 0)
        {
            Destroy(gameObject);
        }
    }
    private void OnTriggerEnter(Collider collision)
    {
        if (collision.GetComponent<Collider>().CompareTag("Enemy"))
            collision.GetComponent<Enemy>().TakeDamage(yes.attack);

    }

    void LocateClosest()
    {
        point = new Vector2(transform.position.x, transform.position.y);
        Collider2D[] hitColliders = Physics2D.OverlapCircleAll(point, radius, enemy);
        foreach (Collider2D hitCollider in hitColliders)
        {
            distance = Vector2.Distance(transform.position, hitCollider.transform.position);

            if (distance < nearestDistance)
            {
                int i = 0;
                i++;
                nearestObject = hitColliders[i].transform.position;
                if (!nearestFound)
                {
                    nearestGameObject = hitColliders[i].gameObject;
                    nearestFound = true;
                }
                nearestDistance = distance;
                enemyChecked = true;
            }
        }
    }

    void MoveProjectile()
    {

    }
}


