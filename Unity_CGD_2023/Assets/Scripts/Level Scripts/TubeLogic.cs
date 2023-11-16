using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class TubeLogic : MonoBehaviour
{
    #region [All References]
    [Header("Tube Prefabs")]
    public GameObject UnBrokenTube;
    public GameObject BrokenTube;

    // Bool
    public bool IsBroken;

    [Header("Choose what prefabs to spawn and where")]
    public List<GameObject> SpawnEntity;
    public Transform SpawnEntityLocation;

    #endregion

    #region [Start and OnCollision]

    public void Start()
    {
        IsBroken = false;

        UnBrokenTube.SetActive(true);
        BrokenTube.SetActive(false);
    }

    public void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "Bullet" && IsBroken == false)
        {
            IsBroken = true;
            entitySpawn();
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

            Instantiate(selectedEntity, SpawnEntityLocation.position, Quaternion.identity);
            Debug.Log("Random entity was spawned");
        }
    }

}
