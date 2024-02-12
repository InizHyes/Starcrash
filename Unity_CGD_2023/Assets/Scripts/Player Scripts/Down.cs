using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Down : MonoBehaviour
{
    public bool downed = false;  /// change this to true when health reaches 0 (not yet implemented)
    private Rigidbody2D rb;   ///rigidbody referecne
    private bool reviving2 = false;
    private bool notRevivingBool = false;
    public int reviveTimer = 0;
    public int reviveTimeTotal = 3;
    
    // Start is called before the first frame update

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        
    }
    IEnumerator reviving() 
    {
        yield return new WaitForSeconds(1f);
        reviveTimer++;
        reviving2 = false;
        


    }
    IEnumerator reduceRevive()
    {
        yield return new WaitForSeconds(1f);
        reviveTimer--;
        notRevivingBool = false;


    }




        // Update is called once per frame
        void Update()
    {
        ///downed = true;
        if (downed)   ///if downed then stop all movement
        {
            this.GetComponent<PlayerController>().playerControl.Disable(); 
            if (this.GetComponent<reviveBox>().reviving == true && reviving2 == false) ///if a player is nearby then add a second to the revive timer
            {
                reviving();
                reviving2 = true;


            }
            else if (this.GetComponent<reviveBox>().reviving == false && reviveTimer > 0 && notRevivingBool == false) ///if a player leaves then slowly reduce the revive timer
            {
                reduceRevive();
                notRevivingBool = true;


            }
            if (reviveTimer == reviveTimeTotal) ///if the revive timer is == the amount of time needed to revive (change this is editor) then the player revives
            {
                this.GetComponent<CharacterStats>().Heal(20);
                downed = false;
                this.GetComponent<PlayerController>().playerControl.Enable();

            }

        }
        
        
    }


    private void OnCollisionEnter2D(Collision2D collision)   ///experimental fun feature, when another bumps into you when downed then get back up. Links into movement
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            this.GetComponent<CharacterStats>().Heal(20);
            downed = false;
            this.GetComponent<PlayerController>().playerControl.Enable();
        }

    }
}
