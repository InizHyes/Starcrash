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

    //bool

    private bool enemyOverlap;

    #endregion

    private void Awake()
    {

        enemyOverlap = false;
    }

    #region [OnTrigger]

    public void OnTriggerEnter2D(Collider2D collision)
    {
        // if player is in the conner and enemy is not overlaping triggerpoints, start countdown
        if (collision.tag == "Player" && enemyOverlap == false)
        {
            StartCoroutine(WaitSpawn());
        }

        // if enemy is overlaping triggerpoints, set bool to true
        if (collision.tag == "Enemy")
        {
            enemyOverlap = true;
        }
    }


    public void OnTriggerExit2D(Collider2D collision)
    {
        // if player is not in the conner, stop countdown
        if (collision.tag == "Player")
        {
            StopCoroutine(WaitSpawn());
        }

        // if enemy is not overlaping triggerpoints, set bool to false
        if (collision.tag == "Enemy")
        {
            enemyOverlap = false;
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
