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

    public bool doSpawn;

    public bool triggerInput;

    public int chanceNumber;

    public int waveCurrent;

    public int waveMax;

    public int nPCTotal;

    public int nPCurrent;

    #endregion

    #endregion

    #region [Start and Update]

    // Start is called before the first frame update
    void Start()
    {
        readySpawn = false; // Make sure the process for doSpawn is ready

        doSpawn = false; // Do not spawn NPCs at start

        triggerInput = true; // Enable Trigger points at start 
    }

    // Update is called once per frame
    void Update()
    {
        // If player hits trigger point whilst TriggerInput == true and DoSpawn == false, start chance counter
        {
            ChanceCounter();
            triggerInput = false;
        }


        if (doSpawn == true)
        {
            WaveCounter();
        }
    }

    #endregion

    #region [Other functions]
    public void ChanceCounter()
    {
        //Genrate randomnumber

        // Outcome conditions
        if (chanceNumber == 1)
        {
            doSpawn = true;
        }

        if (chanceNumber != 1)
        {
            doSpawn = false;
        }

    }

    public void WaveCounter()
    {
        waveCurrent = 0;

        //Randomise max waves
        //waveMax;

        //nPCTotal;

        //nPCurrent;
    }


    #endregion
}
