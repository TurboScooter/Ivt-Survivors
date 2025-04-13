using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class FireWand : MonoBehaviour
{
    [SerializeField] GameObject Projectile_Melee;
    public player player;
    public float attackSpeed;
    public float finalAttackSpeed;
    public float critRate;
    public float critDamage;
    public float attack;
    public float areaOfEffect;
    public float projectileSpeed;
    public int amount;
    public float duration;
    public float durationAmount;
    float cooldown;
    public bool unlock = false;
    public int Level;
    public float finalDamage;

    private void Start()
    {
        duration = durationAmount;
    }
    void Update()
    {
        finalDamage = player.attack * attack;
        areaOfEffect = player.areaOfEffect;
        critDamage = player.critDamage;
        finalAttackSpeed = player.attackSpeed * attackSpeed;

        duration -= Time.deltaTime;
        cooldown -= Time.deltaTime;
        if (unlock)
            Attack();
    }

    public void WeaponUpgradesAndUnlock()
    {
        if (Level != 7)
        {
            if (!unlock)
            {
                Level = 1;
                unlock = true;
            }
            else
                Level++;

            if (Level == 7)
                attack = 1.8f;
            else if (Level == 6)
                attack = 1.6f;
            else if (Level == 5)
                attack = 1.5f;
            else if (Level == 4)
                attack = 1.4f;
            else if (Level == 3)
                attack = 1.3f;
            else if (Level == 2)
                attack = 1.2f;
            else if (Level == 1)
                attack = 1.1f;
        }

    }

    public void Attack()
    {
        if (cooldown <= 0)
        {
            Instantiate(Projectile_Melee, gameObject.transform);
            cooldown = attackSpeed;
            duration = durationAmount;
        }
    }
}