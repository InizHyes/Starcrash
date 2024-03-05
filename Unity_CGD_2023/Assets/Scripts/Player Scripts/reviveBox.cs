using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class reviveBox : MonoBehaviour
{
    public bool reviving;
    // Start is called before the first frame update

    // Update is called once per frame


    private void OnTriggerStay2D(Collider2D collision)
    {
        

        if (collision.gameObject.CompareTag("Player"))
        {
            print("colldingplayer");
            print(collision.gameObject.name);
            reviving = true;
            ///print(reviving);
        }

    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            reviving = false;
            ///print(reviving);
        }

    }

    

}