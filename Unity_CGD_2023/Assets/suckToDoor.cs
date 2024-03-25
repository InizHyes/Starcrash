using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class suckToDoor : MonoBehaviour
{

    public GameObject DoorMarker;
    public float SuctionPower = 1;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }


    private void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            
            var dir = new Vector2();
            dir.x = (DoorMarker.transform.position.x - collision.transform.position.x);
            dir.y = (DoorMarker.transform.position.y - collision.transform.position.y);

            collision.gameObject.GetComponent<Player>().rb.velocity += dir * SuctionPower;

        }

    }


    
}
