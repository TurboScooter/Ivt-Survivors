using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Upgrades : MonoBehaviour
{
    [SerializeField] EmptyBottleItem emptyBottleItem;
    [SerializeField] Sword sword;
    [SerializeField] LevelUp levelUp;
    public List<GameObject> possibleUpgrades;
    public List<int> possibleNumbers;
    [SerializeField] List<int> weaponNumbers;
    [SerializeField] List<int> itemNumbers;
    public List<int> baseNumberList;
    [SerializeField] int pick1;
    [SerializeField] int pick2;
    [SerializeField] int pick3;
    [SerializeField] int pick4;
    [SerializeField] Transform posButton1;
    [SerializeField] Transform posButton2;
    [SerializeField] Transform posButton3;
    [SerializeField] Transform posButton4;
    int maxNumberOfWeapons = 6;
    int currentWeaponAmount = 0;
    bool unlockWeapon = false;
    int maxNumberOfItems = 6;
    int currentItems = 0;
    bool unlockItem = false;
    bool safetyCheck = false;
    private void Start()
    {
        baseNumberList = possibleNumbers;
    }
    private void Update()
    {
        if(unlockWeapon)
        {
            unlockWeapon = false;
            currentWeaponAmount = 0;
        }
    }
    public void pickUpgrades()
    {
            possibleNumbers = new List<int>(baseNumberList);
            pick1 = possibleNumbers[Random.Range(0, possibleNumbers.Count)];
            possibleNumbers.Remove(pick1);
            pick2 = possibleNumbers[Random.Range(0, possibleNumbers.Count)];
            possibleNumbers.Remove(pick2);
            pick3 = possibleNumbers[Random.Range(0, possibleNumbers.Count)];
            possibleNumbers.Remove(pick3);
            pick4 = possibleNumbers[Random.Range(0, possibleNumbers.Count)];
            possibleNumbers.Remove(pick4);

            possibleUpgrades[pick1].SetActive(true);
            possibleUpgrades[pick2].SetActive(true);
            possibleUpgrades[pick3].SetActive(true);
            possibleUpgrades[pick4].SetActive(true);
            possibleUpgrades[pick1].transform.position = posButton1.position;
            possibleUpgrades[pick2].transform.position = posButton2.position;
            possibleUpgrades[pick3].transform.position = posButton3.position;
            possibleUpgrades[pick4].transform.position = posButton4.position;
    }

    float maxSwordUpgradeAmount = 7;
    float maxSwordUpgradeCount = 0;

    public void SwordUpgradeManager()
    {
        sword.WeaponUpgradesAndUnlock();
        maxSwordUpgradeCount++;
        
        if (maxSwordUpgradeCount >= 7)
        {
            baseNumberList.Remove(0);
            possibleUpgrades.Remove(possibleUpgrades[0]);
        }

        possibleUpgrades[pick1].SetActive(false);
        possibleUpgrades[pick2].SetActive(false);
        possibleUpgrades[pick3].SetActive(false);
        possibleUpgrades[pick4].SetActive(false);
        levelUp.upgradeMenu.SetActive(false);

    }

    public void EmptyBottleItemManager()
    {
        emptyBottleItem.Manage();
        levelUp.upgradeMenu.SetActive(false);
    }

}
