using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PickUpExperience : MonoBehaviour
{
    public float ExperienceAmount;

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.collider.CompareTag("Player"))
        Destroy(gameObject);
    }
}
