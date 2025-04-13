using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class WeaponBase : WeaponStats
{


    public bool fwUnlock = false;
    public int fwLevel;
    public void fireWandManager()
    {
        if(fwUnlock)
        {

        }   
    }

    public bool mwUnlock = false;
    public int mwLevel;
    public void MageWandManager()
    {
        if(mwUnlock)
        {

        }
    }

    public bool tbUnlock = false;
    public int tbLevel;
    public void TrowingBottleManager()
    {
        if(tbUnlock)
        {

        }
    }

    public bool lbUnlock = false;
    public int lbLevel;
    public void LavaBucketManager()
    {
        if(lbUnlock)
        {

        }
    }

    public bool whipUnlock = false;
    public int whipLevel;
    public void WhipManager()
    {
        if (whipUnlock)
        {

        }
    }
    

}
[System.Serializable]
public class WeaponStats : MonoBehaviour
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

