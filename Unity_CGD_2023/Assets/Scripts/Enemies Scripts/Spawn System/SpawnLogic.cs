using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnLogic : MonoBehaviour
{

    #region [All References]

    #region [Enemies list]
    [Header("Enemies list")]

    //Get the Enemy prefabs
    public List<GameObject> NPCEnemies;

    //Spawn bounds
    [SerializeField] [Range(1, 50)] private int minimumEnemiesSpawned = 10;
    [SerializeField] [Range(1, 50)] private int maximumEnemiesSpawned = 20;


    #endregion

    #region [Spawn location list]
    [Header("Spawn location list")]

    //Get the spawn points in the room
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
        if (readySpawn == true && nPCCounter !<= 5)
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
        Transform spawnPoint = spawnPoints[Random.Range(0, spawnPoints.Count)];

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

}
