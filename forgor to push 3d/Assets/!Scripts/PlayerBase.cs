using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Data", menuName = "ScriptableObjects/Player", order = 1)]
public class PlayerBase : ScriptableObject
{
    public float attack;
    public float attackSpeed;
    public float hitPoints;
    public float movementSpeed;
    public float pickUpRadius;
    public float critRate;
    public float critDamage;
    public float areaOfEffect;
}
