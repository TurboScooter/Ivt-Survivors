using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelUp : MonoBehaviour
{

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
    
    GameObject upgradeMenu;
    GameObject Button1;
    GameObject Button2;
    GameObject Button3;
    GameObject Button4;

    private void Start()
    {
        currentLevel = 1;
        //upgradeMenu.SetActive(false);
    }

    private void Update()
    {
        if(currentExp >= requiredExp)
        {
            currentLevel++;
            overflowExp =  currentExp -= requiredExp;
            currentExp = 0;
            requiredExp = Mathf.RoundToInt(Mathf.Pow(4 * (currentLevel + 1), 2.1f)) - Mathf.RoundToInt(Mathf.Pow(4 * currentLevel, 2.1f));
            currentExp = overflowExp;
            overflowExp = 0;

            Upgrades();

        }
    }

    void Upgrades()
    {
        upgradeMenu.SetActive(true);


    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.collider.CompareTag("Experience"))
        {
           currentExp += collision.collider.gameObject.GetComponent<PickUpExperience>().ExperienceAmount;
        }
    }
}
