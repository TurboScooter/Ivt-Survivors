using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[CreateAssetMenu(fileName = "Data", menuName = "ScriptableObjects/Shop", order = 2)]

public class ShopScriptableObj : ScriptableObject
{
    public float coins;
    [Space]
    public float maxAtkUpgrades;
    public float atkUpgradeCost;
    public float atkUpgradeCostIncrease;
    public float atkUpgradeCount;
    [Space]

    public float maxCritRateUpgrades;
    public float critRateUpgradeCost;
    public float critRateUpgradeCostIncrease;
    public float critRateUpgradeCount;
    [Space]

    public float maxCritDamageUpgrades;
    public float critDamageUpgradeCost;
    public float critDamageUpgradeCostIncrease;
    public float critDamageUpgradeCount;
    [Space]

    public float maxHpUpgrades;
    public float hpUpgradeCost;
    public float hpUpgradeCostIncrease;
    public float hpUpgradeCount;
    [Space]

    public float maxMovementSpeedUpgrades;
    public float movementSpeedUpgradeCost;
    public float movementSpeedUpgradeCostIncrease;
    public float movementSpeedUpgradeCount;

}
