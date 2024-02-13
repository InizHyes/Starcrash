using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class reviveBox : MonoBehaviour
{
    public bool reviving;
    // Start is called before the first frame update
  
    // Update is called once per frame
    private void OnCollisionStay2D(Collision2D collision)  /// small script to check if a player is in a small radius around the player, if it is it makes revivng true
                                                        /// which gets passed through to the down script
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            reviving = true;
        }
            
    }
    private void OnCollisionExit2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            reviving = false;
        }

    }
}
