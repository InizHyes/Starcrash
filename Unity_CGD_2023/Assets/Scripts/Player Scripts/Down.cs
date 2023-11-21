using System.Collections;
using System.Collections.Generic;
using UnityEditor.UI;
using UnityEngine;

public class Down : MonoBehaviour
{
    private bool downed = false;  /// change this to true when health reaches 0 (not yet implemented)
    private Rigidbody2D rb;   ///rigidbody referecne
    // Start is called before the first frame update

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        
    }
   

    // Update is called once per frame
    void Update()
    {
        ///downed = true;
        if (downed)   ///if downed then stop all movement
        {
            ///rb.velocity = new Vector2(0, 0);
            this.GetComponent<PlayerController>().playerControl.Disable();

        }
        
    }


    private void OnCollisionEnter2D(Collision2D collision)   ///experimental fun feature, when another bumps into you when downed then get back up. Links into movement
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            downed = false;
            this.GetComponent<PlayerController>().playerControl.Enable();
        }

    }
}
