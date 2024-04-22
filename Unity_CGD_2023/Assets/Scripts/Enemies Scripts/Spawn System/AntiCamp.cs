using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AntiCamp : MonoBehaviour
{

    #region [All References]

    #region [Turrets list]
    [Header("Turrets list")]

    //Trigger points
    [Tooltip("Drag and drop enemy turret prefabs to spawn")] public List<GameObject> Turrets;

    #endregion

    //int

    private int turretSpawnTimer = 5;

    // Transform

    public Transform spawnLocation;

    #endregion

    #region [OnTrigger]

    public void OnTriggerEnter2D(Collider2D collision)
    {
        // if player is in the conner and enemy is not overlaping triggerpoints, start countdown
        if (collision.tag == "Player")
        {
            StartCoroutine(WaitSpawn());
        }
    }

    public void OnTriggerExit2D(Collider2D collision)
    {
        // if player is not in the conner, stop countdown
        if (collision.tag == "Player")
        {
            StopCoroutine(WaitSpawn());
        }
    }

    #endregion

    IEnumerator WaitSpawn()
    {
        //Spawn rate of turrets is based on countdown
        yield return new WaitForSeconds(turretSpawnTimer);

        // When timer is 0, run function to spwan the turrets in list
        spawnTurret();
    }

    public void spawnTurret()
    {
        GameObject selectedTurret;
        {
            selectedTurret = Turrets[Random.Range(0, Turrets.Count)];
        }

        GameObject newAntiCamp = Instantiate(selectedTurret, spawnLocation.position, Quaternion.identity);
        StopCoroutine(WaitSpawn());
        turretSpawnTimer = 5;
        shutdown();
    }

    // Use this function along side SpawnLogic when all enemies or players are dead
    public void shutdown()
    {
        Destroy(gameObject);
    }

    //During the merge process with "dev" branch, please place these lines of code in SpawnLogic under
    //"AllEnemiesDead()" function, as the last process.

    //Once all enemies are dead, tell AntiCamp to shut itself down for current room
    //AntiCamp shutdownFunction = GetComponent<AntiCamp>();
    //shutdownFunction.shutdown();
}
