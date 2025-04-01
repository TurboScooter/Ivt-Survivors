using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class WeaponBase : MonoBehaviour
{
    public List<WeaponStats> stats;
    public int level;

    public void levelUp()
    {
        if(level < stats.Count - 1)
        {
            level++;
        }
    }

    

}
[System.Serializable]
public class WeaponStats
{
    public float attackSpeed;
    public float critRate;
    public float critDamage;
    public float attack;
    public float areaOfEffect;  
    public float projectileSpeed;
    public int amount;
    public float duration;
}

