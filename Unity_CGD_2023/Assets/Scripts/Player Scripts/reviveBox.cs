using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class reviveBox : MonoBehaviour
{
    public bool reviving;
    private List<GameObject> playersin = new List<GameObject>();
    // Start is called before the first frame update

   

    private void Update()
    {
        if (playersin.Count > 0)
        {
            foreach (var player in playersin)
            {
                if (player.GetComponent<Down>().downed == false)
                {
                    reviving = true ;
                    break;
                }
                else
                {
                    reviving = false;
                }
            }
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            playersin.Add(collision.gameObject);
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            playersin.Remove(collision.gameObject);
            reviving = false;
            ///print(reviving);
        }

    }

    

}