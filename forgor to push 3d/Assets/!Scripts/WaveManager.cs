using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WaveManager : MonoBehaviour
{
    
    public List<GameObject> spawnableEnemys;
    public float timer;
    public float seconds;
    public float minutes;
    Transform playerLocation;
    Vector3 spawnPosition;
    Vector3 maxDistance;
    [SerializeField] float spawnRadius;
    float despawnDistance;
    float spawnCooldown;
    [SerializeField] float cdAmount;
    void Start()
    {
        
    }

    void Update()
    {
        TimersTime();

        if(spawnCooldown <= 0)
        {

        }
    }

    public void TimersTime()
    {
        timer += Time.deltaTime;
        seconds += Time.deltaTime;
        if(seconds >= 60)
        {
            seconds -= 60;
            minutes++;
        }
    }

    public void Spawning()
    {
        Vector3 spawnPos = Random.insideUnitCircle.normalized * spawnRadius;
        spawnPosition = new Vector3(spawnPos.x, playerLocation.position.y, spawnPos.z);
        if(timer >= 5 && timer <= 120)
        {
            Instantiate(spawnableEnemys[0], spawnPosition, Quaternion.identity);
        }
    }
    public void Despawn()
    {
        despawnDistance = Vector3.Distance(playerLocation.position, maxDistance);
    }
}



//if timer is between x and x spawn this enemy 
//if timer hits x spawn miniboss
//if timer hits x spawn boss
//do spawn using a circle around the player outside the screen
// randomize spawning
// spawn enemies in group from x to x
// if enemy walks into screen turn on despawnable if enemy goes certain range from screen respawn on the otherside on the outside of the screen
