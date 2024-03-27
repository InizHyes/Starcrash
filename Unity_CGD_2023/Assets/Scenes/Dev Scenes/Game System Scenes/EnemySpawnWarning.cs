using System.Collections;
using System.Collections.Generic;
using UnityEngine;


// A function to place them

// a function to remove them

public class EnemySpawnWarning : MonoBehaviour
{
    // Warning prefab
    GameObject warningPrefab;

    // SPawncontroller object in scene
    GameObject spawnControllerObject;

    // Script object
    SpawnLogic spawnLogic;

    // List to hold warningPrefabs
    public List<GameObject> InstanitedWarningPrefabs = new List<GameObject>();

    bool booleon = false;

    // Start is called before the first frame update
    void Start()
    {
        spawnControllerObject = GameObject.Find("SpawnController");
        spawnLogic = spawnControllerObject.GetComponent<SpawnLogic>();
        warningPrefab = Resources.Load<GameObject>("EnemySpawnWarning/EnemySpawnWarning") as GameObject;

        print("worked " + warningPrefab.name);
    }

    private void PlaceWarnings()
    {
        foreach (Transform child in spawnControllerObject.transform)
        {
            //Instantiate warning prefab at the spawnPoints transform position
            GameObject warning = Instantiate(warningPrefab, child.transform);

            // Add to list
            InstanitedWarningPrefabs.Add(warning);
        }
    }

    private void RemoveWarnings()
    {
        for (int i = InstanitedWarningPrefabs.Count - 1; i >= 0; i--)
        {
            // Destroy the GameObject
            Destroy(InstanitedWarningPrefabs[i]);

            // Remove the GameObject from the list
            InstanitedWarningPrefabs.RemoveAt(i);
        }
    }

    // Update is called once per frame
    void Update()
    {
        if(!spawnLogic.readySpawn && !booleon)
        {
            booleon = true;
           // PlaceWarnings();
           // RemoveWarnings();
            print("happens");
        }
        
    }
}
