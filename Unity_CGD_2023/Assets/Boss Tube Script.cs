using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class BossTubeLogic : MonoBehaviour
{
    #region [All References]
    [Header("Tube Prefabs")]

    public GameObject UnBrokenTube;
    public GameObject BrokenTube;


    // Bool
    public bool IsBroken;

    [Header("Choose what prefabs to spawn")]
    public List<GameObject> SpawnEntity;

    [Header("Color Pressure Plate")]
    public PressurePlateScript connectedPressurePlate;

    public GlobalIntegerScript globalIntegerScript;

    #endregion

    #region [Start and OnCollision]

    public void Start()
    {
        IsBroken = false;
    }



    public void OnCollisionEnter2D(Collision2D collision)
    {
        if (connectedPressurePlate != null && connectedPressurePlate.isFirstPressurePlateActivated && IsBroken == false)
        {
            // Check if the collision is with the object you want to trigger the destruction
            if (collision.gameObject.CompareTag("Bullet"))
            {
                Debug.Log("hit");
            IsBroken = true;
            entitySpawn();
            }
        }
    }

    #endregion


    public void entitySpawn()
    {
        if (SpawnEntity == null)
        {
            Debug.Log("Nothing was spawned");
        }

        if (SpawnEntity != null)
        {
            // Get enemy list, randomise what enemy should be spawned
            GameObject selectedEntity = SpawnEntity[Random.Range(0, SpawnEntity.Count)];


            Instantiate(BrokenTube, this.transform.position, Quaternion.identity);

            Instantiate(selectedEntity, this.transform.position, Quaternion.identity);
            Debug.Log("Random entity was spawned");

            Destroy(UnBrokenTube);
        }
    }

}
