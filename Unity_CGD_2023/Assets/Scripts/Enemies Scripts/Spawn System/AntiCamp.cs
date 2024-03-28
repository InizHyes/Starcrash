using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AntiCamp : MonoBehaviour
{

    #region [All References]

    #region [Triggers list]
    [Header("Triggers list")]

    //Trigger points
    [Tooltip("Click and drag Trigger points on scene for the Players to collide with." +
        "This will also allow for the enemies to spawn at.")] public List<Transform> TriggerPoints;

    [Tooltip("If true - spawn enemies on a random location in the array below, if false - iterate through the array in order")][SerializeField] private bool spawnRandomly = true;
    private int spawnPointID = 0;

    #endregion

    #region [Turrets list]
    [Header("Turrets list")]

    //Trigger points
    [Tooltip("Drag and drop enemy turret prefabs to spawn")] public List<GameObject> Turrets;

    [SerializeField] private bool chooseRandomly = true;
    private int nextEnemyID = 0;

    #endregion

    //int

    private int turretSpawnTimer = 5;

    #endregion

    #region [OnTrigger]

    public void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "Player")
        {
            StartCoroutine(WaitSpawn());
        }
    }

    public void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.tag == "Player")
        {
            StopCoroutine(WaitSpawn());
        }
    }

    #endregion

    IEnumerator WaitSpawn()
    {
        yield return new WaitForSeconds(turretSpawnTimer);

        spawnTurret();
    }

    public void spawnTurret()
    {
        GameObject selectedTurret;
        // Get enemy list, randomise what enemy should be spawned
        if (chooseRandomly)
        {
            selectedTurret = Turrets[Random.Range(0, Turrets.Count)];
        }
        // Spawn in sequence
        else
        {
            if (nextEnemyID > Turrets.Count - 1)
            {
                // Reset on array end
                nextEnemyID = 0;
            }
            selectedTurret = Turrets[nextEnemyID];
            nextEnemyID++;
        }

        // Spawn random enemy at random spawn point
        Transform spawnPoint;
        if (spawnRandomly)
        {
            spawnPoint = TriggerPoints[Random.Range(0, TriggerPoints.Count)];
        }
        // Spawn in sequence
        else
        {
            if (spawnPointID > TriggerPoints.Count - 1)
            {
                // Reset on array end
                spawnPointID = 0;
            }
            spawnPoint = TriggerPoints[spawnPointID];
            spawnPointID++;
        }

        turretSpawnTimer = 5;
    }

    // Use this function along side SpawnLogic when all enemies or players are dead
    public void shutdown()
    {
        //Get all the triggerPoints from the list and run this function for each of them all
        foreach (Transform triggerPoint in TriggerPoints)
        {
            // Disable colliders within the GameObject containing the Transform
            Collider[] colliders = triggerPoint.GetComponentsInChildren<Collider>();
            foreach (Collider collider in colliders)
            {
                collider.enabled = false;
            }
        }

        turretSpawnTimer = 5;

        StopCoroutine(WaitSpawn());
    }


}
