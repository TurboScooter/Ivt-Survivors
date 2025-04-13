using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Shop : MonoBehaviour
{
    public PlayerBase player;
    public ShopScriptableObj shop;


    public void PermAtkUpgrade()
    {
      if(shop.coins >= shop.atkUpgradeCost && shop.atkUpgradeCount != shop.maxAtkUpgrades)
        {
            player.attack *= 1.06f;
            shop.atkUpgradeCost += shop.atkUpgradeCostIncrease;
            shop.atkUpgradeCount++;
        }
    }
}
