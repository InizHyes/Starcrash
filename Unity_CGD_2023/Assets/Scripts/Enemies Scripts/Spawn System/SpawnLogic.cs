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

    [SerializeField] private bool chooseRandomly = true;
    private int nextEnemyID = 0;

    //Get the Enemy prefabs
    [Tooltip("Drag and drop enemy prefabs to spawn")] public List<GameObject> NPCEnemies;

    //Spawn bounds
    [SerializeField][Tooltip("The minimum range of random Total amount of enemies to spawn")] private int minimumEnemiesSpawned = 10;
    [SerializeField][Tooltip("The maximum range of random Total amount of enemies to spawn")] private int maximumEnemiesSpawned = 20;
    [SerializeField][Tooltip("The maximum amount of enemies to spawn on screen at anytime")] private int spawnCount = 5;

    //Enemy count scales with player count
    private PlayerManager playerManager;
    [SerializeField] private int playerCount;

    //Control Enemy scaling
    [Tooltip("Use the slider to control Current enemy spawn scaling")] [Range(1, 10)] private float enemyScale = 1;

    #endregion

    #region [Spawn location list]
    [Header("Spawn location list")]

    //Get the spawn points in the room
    [Tooltip("If true - spawn enemies on a random location in the array below, if false - iterate through the array in order")][SerializeField] private bool spawnRandomly = true;
    private int spawnPointID = 0;
    [Tooltip("Click and drag spawn points on scene for the enemies to spawn at")] public List<Transform> spawnPoints;
    [Tooltip("Seconds delay before enemies spawn on player collision enter")][SerializeField] private float spawnDelay = 3f;
    private float spawnDelayCounter = 0;
    [Tooltip("When all enemies are dead open doors when true")][SerializeField] private bool openDoors = true;

    #endregion

    #region [Live variables]
    [Header("Live variables")]

    [Tooltip("If true, the SpawnLogic is currently operating")] public bool readySpawn;

    [Tooltip("The Total amount of enemies left to spawn")] public int nPCTotal;

    [Tooltip("The current amount of enemies on screen at anytime")] public int nPCCounter;

    private Collider2D boxCollider;


    #endregion

    private EnemyClass initiateDeathCheck;

    #endregion

    #region [Start and Update]

    private void Awake()
    {
        readySpawn = false;

        // Get the Collider2D component attached to the GameObject
        boxCollider = GetComponent<Collider2D>();

        boxCollider.enabled = true;
    }

    // Start is called before the first frame update
    void Start()
    {


    }

    // Update is called once per frame
    public void Update()
    {
        // Timer for first time collision enter
        if (spawnDelayCounter > 0)
        {
            spawnDelayCounter -= Time.deltaTime;

            if (spawnDelayCounter < 0)
            {
                spawnDelayCounter = 0;

                //Scale with player count
                playerManager = FindAnyObjectByType(typeof(PlayerManager)) as PlayerManager;
                playerCount = playerManager.nextPlayerID;

                NPCCounter();
                boxCollider.enabled = false;
            }
        }

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
        if (playerCount == 0)
        {
            playerCount = 1;
        }

        nPCTotal = Random.Range(minimumEnemiesSpawned, maximumEnemiesSpawned) * playerCount;

        //Debug.Log("Current enemies spawn: " + nPCTotal + "Enemies");

        readySpawn = true;
    }

    // 1 function trigger = 1 enemy spawn at random location
    public void SpawnEnemyNPC()
    {
        GameObject selectedEnemy;
        // Get enemy list, randomise what enemy should be spawned
        if (chooseRandomly)
        {
            selectedEnemy = NPCEnemies[Random.Range(0, NPCEnemies.Count)];
        }
        // Spawn in sequence
        else
        {
            if (nextEnemyID > NPCEnemies.Count - 1)
            {
                // Reset on array end
                nextEnemyID = 0;
            }
            selectedEnemy = NPCEnemies[nextEnemyID];
            nextEnemyID++;
        }

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

        float scaleFactor = enemyScale;

        // Set new enemy variables
        GameObject newEnemy = Instantiate(selectedEnemy, spawnPoint.position, Quaternion.identity);
        newEnemy.transform.SetParent(this.transform);
        newEnemy.transform.localScale *= scaleFactor;
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
            spawnDelayCounter = spawnDelay;
            // Find the GameObject with the DoorManager script attached
            GameObject doorManagerObject = GameObject.Find("DoorManager");
            if (doorManagerObject != null)
            {
                DoorManager doorManager = doorManagerObject.GetComponent<DoorManager>();
                // Call LockDoors function after spawning enemy
                doorManager.LockDoors();
            }
        }
    }

    public void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.tag == "Enemy")
        {
            initiateDeathCheck = collision.GetComponent<EnemyClass>(); // Find the function
            initiateDeathCheck.initiateDeath(); // Kill the enemy

            //NPCdeath(); // Run function from within this script
            Debug.Log("Enemy went out side spawn zone");
        }
    }

    #endregion

    public void AllEnemiesDead()
    {
        /*
         * Put logic for end of enemy spawning here
         * E.g. doors opening logic
         */

        if (openDoors)
        {
            // Find the GameObject with the DoorManager script attached
            GameObject doorManagerObject = GameObject.Find("DoorManager");
            if (doorManagerObject != null)
            {
                DoorManager doorManager = doorManagerObject.GetComponent<DoorManager>();

                doorManager.OpenDoors();

                //print("All enemies dead");
            }
        }
    }

}
