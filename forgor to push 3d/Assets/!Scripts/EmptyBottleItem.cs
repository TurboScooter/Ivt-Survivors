using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

//has 3 levels increases
public class EmptyBottleItem : MonoBehaviour
{
    player playerStats;
    public bool unlock = false;
    public int level;
   

    public void Manage()
    {
        if (!unlock)
        {
            level++;
            unlock = true;
        }else if (level != 3)
        {
            level++;
        }

        if (level == 3)
            playerStats.areaOfEffect =+ playerStats.areaOfEffect * 1.15f;
        if (level == 2)
            playerStats.areaOfEffect =+ playerStats.areaOfEffect * 1.1f;
        if (level == 1)
            playerStats.areaOfEffect =+ playerStats.areaOfEffect * 1.05f;
    }

}

