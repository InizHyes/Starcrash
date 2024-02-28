using System.Collections;
using System.Collections.Generic;
using Unity.Burst.CompilerServices;
using Unity.VisualScripting;
using UnityEngine;

public class SpawnLogic : MonoBehaviour
{

    #region [All References]

    #region [Enemies list]
    [Header("Enemies list")]

    //Get the Enemy prefabs
    public List<GameObject> NPCEnemies;

    //Spawn bounds
    [SerializeField] private int minimumEnemiesSpawned = 10;
    [SerializeField] private int maximumEnemiesSpawned = 20;
    [SerializeField] private int spawnCount = 5;


    #endregion

    #region [Spawn location list]
    [Header("Spawn location list")]

    //Get the spawn points in the room
    [Tooltip("If true - spawn enemies on a random location in the array below, if false - iterate through the array in order")][SerializeField] private bool spawnRandomly = true;
    private int spawnPointID = 0;
    public List<Transform> spawnPoints;

    #endregion

    #region [Live variables]
    [Header("Live variables")]

    public bool readySpawn;

    public int nPCTotal;

    public int nPCCounter;

    private Collider2D boxCollider;

    #endregion

    #endregion

    #region [Start and Update]

    // Start is called before the first frame update
    void Start()
    {
        readySpawn = false;

        // Get the Collider2D component attached to the GameObject
        boxCollider = GetComponent<Collider2D>();

        boxCollider.enabled = true;
    }

    // Update is called once per frame
    public void Update()
    {
        // Start wave and NPC spawn after set up is done / whilst keeping to max screen limit
        if (readySpawn == true && nPCCounter < spawnCount)
        {
            SpawnEnemyNPC();
        }
    }

    #endregion

    #region [Other functions]

    //Randomise max total number of NPCs to spawn
    public void NPCCounter()
    {
        nPCTotal = Random.Range(minimumEnemiesSpawned, maximumEnemiesSpawned);

        //Debug.Log("Current enemies spawn: " + nPCTotal + "Enemies");

        readySpawn = true;
    }

    // 1 function trigger = 1 enemy spawn at random location
    public void SpawnEnemyNPC()
    {
        // Get enemy list, randomise what enemy should be spawned
        GameObject selectedEnemy = NPCEnemies[Random.Range(0, NPCEnemies.Count)];

        // Spawn random enemy at random spawn point
        Transform spawnPoint;
        if (spawnRandomly)
        {
            spawnPoint = spawnPoints[Random.Range(0, spawnPoints.Count)];
        }
        // Spawn in sequence
        else
        {
            if (spawnPointID > spawnPoints.Count - 1)
            {
                // Reset on array end
                spawnPointID = 0;
            }
            spawnPoint = spawnPoints[spawnPointID];
            spawnPointID++;
        }

        // Set new enemy variables
        GameObject newEnemy = Instantiate(selectedEnemy, spawnPoint.position, Quaternion.identity);
        newEnemy.transform.SetParent(this.transform);
        newEnemy.GetComponentInChildren<EnemyClass>().NPCdeathCheck = this;

        nPCTotal -= 1;

        nPCCounter += 1;

        // If all NPCs are dead, do not spawn anymore
        if (nPCTotal <= 0)
        {
            readySpawn = false;
        }
    }

    public void NPCdeath()
    {
        //If player kills a NPC allow another NPC to spawn if other conditions are vaild 
        nPCCounter -= 1;

        if (nPCCounter <= 0)
        {
            AllEnemiesDead();
        }
    }

    public void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "Player")
        {
            NPCCounter();

            boxCollider.enabled = false;
        }
    }

    #endregion

    public void AllEnemiesDead()
    {
        /*
         * Put logic for end of enemy spawning here
         * E.g. doors opening logic
         */

        print("All enemies dead");
    }

}
