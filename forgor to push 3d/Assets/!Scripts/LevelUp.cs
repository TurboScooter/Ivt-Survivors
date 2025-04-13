using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelUp : MonoBehaviour
{
    [SerializeField] Upgrades upgrades;
    float hp;
    float atk;
    float movementSpeed;
    float attackSpeed;
    float pickUpRange;
    [SerializeField]float requiredExp;
    [SerializeField] float currentExp;
    float expIncrease;
    float overflowExp;
    [SerializeField]float currentLevel;
    
    public GameObject upgradeMenu;



    private void Start()
    {
        currentLevel = 1;
        upgradeMenu.SetActive(false);
    }

    private void Update()
    {
        if(currentExp >= requiredExp)
        {
            overflowExp =  currentExp -= requiredExp;
            currentExp = 0;
            requiredExp = Mathf.RoundToInt(Mathf.Pow(4 * (currentLevel + 1), 2.1f)) - Mathf.RoundToInt(Mathf.Pow(4 * currentLevel, 2.1f));
            currentExp = overflowExp;
            overflowExp = 0;
            currentLevel++;

            Upgrades();

        }
    }
    bool listReset = false;
    void Upgrades()
    {
        upgradeMenu.SetActive(true);
        
        if(!listReset)
        upgrades.baseNumberList = upgrades.possibleNumbers;

        upgrades.pickUpgrades();

    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.collider.CompareTag("Experience"))
        {
           currentExp += collision.collider.gameObject.GetComponent<PickUpExperience>().ExperienceAmount;
        }
    }
}
