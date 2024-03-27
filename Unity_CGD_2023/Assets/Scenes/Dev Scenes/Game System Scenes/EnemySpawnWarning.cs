using System.Collections;
using System.Collections.Generic;
using UnityEngine;


// A function to place them

// a function to remove them

public class EnemySpawnWarning : MonoBehaviour
{
    GameObject spawnControllerObject;
    SpawnLogic spawnLogic;

    // List to hold enemy spawn transforms
    public List<Transform> spawnTransforms;

    Transform eclamationMarkPrefab;

    public List<Transform> InstanitedObjects;

    // Start is called before the first frame update
    void Start()
    {
        spawnControllerObject = GameObject.Find("SpawnController");
        spawnLogic = spawnControllerObject.GetComponent<SpawnLogic>();

        eclamationMarkPrefab = Resources.Load<Transform>("EnemySpawnWarning/EnemySpawnWarning") as Transform;

        //Loop through all children of this GameObject
        foreach (Transform child in spawnControllerObject.transform)
        {
            // Add the transform of each child to the list
            Transform warning = Instantiate(eclamationMarkPrefab, child);
            spawnTransforms.Add(child);
            InstanitedObjects.Add(warning);
        }

        //for (int i = 0; i < spawnTransforms.Count; i++)
        //{
        //    // Accessing the element at index i
        //    GameObject element = InstanitedObjects[i];
        //    Transform OTHER = spawnTransforms[i];
        //    //element.transform = OTHER;
        //    Debug.Log("Element at index " + i + ": " + element);
        //}

        print("worked " + eclamationMarkPrefab.name);
    }

    private void placeWarnings()
    {
        foreach (Transform child in spawnControllerObject.transform)
        {
            // Add the transform of each child to the list
            Instantiate(eclamationMarkPrefab, child);
            //Transform warning = Instantiate(eclamationMarkPrefab, child);
            //spawnTransforms.Add(child);
            //InstanitedObjects.Add(warning);
        }
    }

    // Update is called once per frame
    void Update()
    {
        if(spawnLogic.readySpawn)
        {
           
        }
        
    }
}
