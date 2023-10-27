using System.Collections;
using System.Collections.Generic;
using UnityEditor.ShaderGraph.Serialization;
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

    public bool SetupWaveBool;

    public bool triggerInput;

    public int chanceNumber;

    public int waveCurrent;

    public int waveMax;

    public int nPCTotal;

    public int nPCurrent;

    public int nPCMaxOnScreen;

    #endregion

    #endregion

    #region [Start and Update]

    // Start is called before the first frame update
    void Start()
    {
        readySpawn = false; // Make sure the process for SetupWaveBool is ready

        SetupWaveBool = false; // Do not spawn NPCs at start

        triggerInput = true; // Enable Trigger points at start 

        nPCMaxOnScreen = 5; // Set maxiumim amount of NPCs on screen;
    }

    // Update is called once per frame
    void Update()
    {
        // If player hits trigger point, start chance counter
        if (CompareTag("SpawnTrigger") && triggerInput == true && SetupWaveBool == false && readySpawn == false)
        {
            ChanceCounter();
            triggerInput = false;
        }

        // Trigger wave set up function
        if (SetupWaveBool == true)
        {
            WaveRestart();
        }

        // Start wave and NPC spawn after set up is done / whilst keeping to max screen limit
        if (readySpawn == true && nPCurrent != 0 && nPCMaxOnScreen >= 5)
        {
            SpawnEnemyNPC();
            nPCMaxOnScreen += 1;

            // (Need to try to make sure there is no enemy overlaping spawn point in future!)
        }

        // If all NPCs are dead, do not spawn anymore
        if (nPCurrent == 0)
        {
            readySpawn = false;
            // end the wave and start the next wave after some time

            
        }

        if (waveCurrent > waveMax)
        {
            // end wave system altogether

            triggerInput = true;
            SetupWaveBool = false;
            readySpawn = false;

            //This ends the wave sytem loop, return to trigger mode
        }
    }

    #endregion

    #region [Other functions]

    //Will wave set up happen?
    public void ChanceCounter()
    {
        //Genrate randomnumber
        chanceNumber = (Random.Range(1, 4));

        // Outcome conditions
        if (chanceNumber == 1)
        {
            SetupWaveBool = true;
            WaveRestart();
            Debug.Log("Start wave set up");
        }

        if (chanceNumber != 1)
        {
            SetupWaveBool = false;
            triggerInput = true; //Rearm trigger point
            Debug.Log("Do not set up waves");
        }

    }

    // Randomise lengh of wave total
    public void WaveRestart()
    {
        waveCurrent = 1; //Set wave system to begining

        //Randomise max waves
        waveMax = (Random.Range(1, 10));
        
        Debug.Log("The max wave number is set to: " + waveMax);

        SetupWaveBool = false;

        NPCCounter();
    }

    //Randomise max total number of NPCs to spawn each wave
    public void NPCCounter()
    {
        nPCTotal = (Random.Range(10, 50));

        nPCTotal = nPCurrent;

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

        waveCurrent += 1;

        NPCCounter();

        Debug.Log("Next wave has started: Wave " + waveMax);
    }
    #endregion

}
