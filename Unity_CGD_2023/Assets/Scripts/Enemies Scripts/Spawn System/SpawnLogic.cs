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

    #endregion

    #region [Trigger wave list]
    [Header("Trigger wave list")]

    //Get the trigger points in the room
    public List<GameObject> TriggerPoints;

    #endregion

    #region [Spawn location list]
    [Header("Spawn location list")]

    //Get the spawn points in the room
    public List<Transform> spawnPoints;

    #endregion

    #region [Wave system]
    [Header("Wave system")]

    public bool readySpawn;

    public bool triggerInput;

    public int waveCurrent;

    public int waveMax;

    public int nPCTotal;

    public int nPCCounter;

    #endregion

    #endregion

    #region [Start and Update]

    // Start is called before the first frame update
    void Start()
    {
        readySpawn = true; //Do not spawn NPCs at start

        triggerInput = false; // Enable Trigger points at start 

        nPCCounter = 0; // Set maxiumim amount of NPCs on screen;
    }

    // Update is called once per frame
    void Update()
    {
        // If player hits trigger point, start chance counter
        if (CompareTag("SpawnTrigger") && triggerInput == true && readySpawn == false)
        {
            WaveRestart();
            triggerInput = false;
        }

        // Start wave and NPC spawn after set up is done / whilst keeping to max screen limit
        if (readySpawn == true && nPCCounter <= 5)
        {
            SpawnEnemyNPC();
            nPCCounter += 1;

            // (Need to try to make sure there is no enemy overlaping spawn point in future!)
        }

        // If all NPCs are dead, do not spawn anymore
        if (nPCCounter == 0)
        {
            readySpawn = false;

            // end the wave and start the next wave after some time            

            StartCoroutine(WaveNext());
        }

        if (waveCurrent > waveMax)
        {
            // end wave system altogether

            StopCoroutine(WaveNext());

            triggerInput = true;
            readySpawn = false;

            //This ends the wave sytem loop, return to trigger mode
        }
    }

    #endregion

    #region [Other functions]

    // Randomise lengh of wave total
    public void WaveRestart()
    {
        waveCurrent = 1; //Set wave system to begining

        //Randomise max waves
        waveMax = (Random.Range(1, 10));
        
        Debug.Log("The max wave number is set to: " + waveMax);

        NPCCounter();
    }

    //Randomise max total number of NPCs to spawn each wave
    public void NPCCounter()
    {
        nPCTotal = (Random.Range(10, 50));

        Debug.Log("For wave " + waveCurrent + ", spawn: " + nPCTotal + "Enemies");

        readySpawn = true; // Both WaveResart or WaveNext and NPCCounter are done
    }

    // 1 function trigger = 1 enemy spawn at random location
    public void SpawnEnemyNPC()
    {
        // Get enemy list, randomise what enemy should be spawned
        GameObject selectedEnemy = NPCEnemies[Random.Range(0, NPCEnemies.Count)];

        // Spawn random enemy at random spawn point
        Transform spawnPoint = spawnPoints[Random.Range(0, spawnPoints.Count)];

        Instantiate(selectedEnemy, spawnPoint.position, Quaternion.identity);
    }

    // Small Delay before new wave spawn
    public IEnumerator WaveNext()
    {
        yield return new WaitForSeconds(5);

        StopCoroutine(WaveNext());

        waveCurrent += 1;

        NPCCounter();

        Debug.Log("Next wave has started: Wave " + waveCurrent);
    }

    public void NPCdeath()
    {
        //If player kills a NPC allow another NPC to spawn if other conditions are vaild 
        nPCCounter -= 1;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "Player")
        {
            triggerInput = true;
        }
    }

    #endregion

}
