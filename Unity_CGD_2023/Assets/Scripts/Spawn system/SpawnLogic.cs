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

    //public GameObject NPC1;





    #endregion

    #region [Trigger wave list]
    [Header("Trigger wave list")]

    //Get the trigger points in the room
    public List<GameObject> TriggerPoints;

    //public Transform TriggerPoint1;



    #endregion

    #region [Spawn location list]
    [Header("Spawn location list")]

    //Get the spawn points in the room
    public List<GameObject> SpawnPoints;

    //public Transform SpawnPoint1;

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
    }

    // Update is called once per frame
    void Update()
    {
        // If player hits trigger point, start chance counter
        if (CompareTag("") && triggerInput == true && SetupWaveBool == false)
        {
            ChanceCounter();
            triggerInput = false;
        }

        if (SetupWaveBool == true)
        {
            WaveRestart();
        }

        if (nPCurrent == 0)
        {
            // end the wave and start the next wave after some time
        }

        if (waveCurrent > waveMax)
        {
            // end wave system altogether

            //This ends the wave sytem loop
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
            Debug.Log("Start wave set up");
        }

        if (chanceNumber != 1)
        {
            SetupWaveBool = false;
            triggerInput = true; //Rearm trigger point
            Debug.Log("Do not set up waves");
        }

    } //Done

    public void WaveRestart() //Need to double check
    {
        waveCurrent = 1;

        //Randomise max waves
        waveMax = (Random.Range(1, 10));
        
        Debug.Log("The max wave number is set to: " + waveMax);
    }

    public void NPCCounter() //Done, need to intergrate
    {
        //Randomise max total number of NPCs to spawn each wave
        nPCTotal = (Random.Range(10, 50));

        nPCTotal = nPCurrent;

        Debug.Log("For current wave, spawn: " + nPCTotal + "Enemies");
    }

    public void WaveBegin() // Work in progress
    {
        // Get enemy list, randomise what enemy should be spawned

        // If there is no enemy overlaping spawn point and nPCMaxOnScreen is not greater, then spawn that NPC.


    }

    public void WaveNext() //Need to double check
    {
        waveCurrent += 1;

        NPCCounter();

        Debug.Log("Next wave has started: Wave " + waveMax);
    }

    #endregion

}
