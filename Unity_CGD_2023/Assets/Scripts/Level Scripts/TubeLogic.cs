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

    [Header("Choose what prefabs to spawn")]
    public List<GameObject> SpawnEntity;

    #endregion

    #region [Start and OnCollision]

    //Audio
    public AudioSource audioSource; // Add Tube_Broken_2 as audio source

    public void Start()
    {
        IsBroken = false;
    }
    
    public virtual void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "Bullet" && IsBroken == false)
        {
            if (!audioSource.isPlaying)
                audioSource.Play();
         
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


            Instantiate(BrokenTube, this.transform.position, Quaternion.identity);

            Instantiate(selectedEntity, this.transform.position, Quaternion.identity);
            Debug.Log("Random entity was spawned");

            Destroy(UnBrokenTube);
        }
    }
}
