using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class Down : MonoBehaviour
{
    public bool downed = false;  /// change this to true when health reaches 0 (not yet implemented)
    private Rigidbody2D rb;   ///rigidbody referecne
    private bool reviving2 = false;
    private bool notRevivingBool = false;
    public int reviveTimer = 0;
    public int reviveTimeTotal = 3;
    private bool doOnce = false;
    ///public GameObject playermanager23;
    public GameObject revivebox;
    public GameObject reviveText;

    // Start is called before the first frame update

    private void Awake()
    {
        reviveTimer = reviveTimeTotal;


        rb = GetComponent<Rigidbody2D>();

    }
    IEnumerator reviving()
    {
        yield return new WaitForSeconds(1f);
        reviveTimer--;
        if (reviveTimer < 0)
        {
            reviveTimer = 0;
        }
        reviving2 = false;
        ///print(reviveTimer);
        reviveText.GetComponent<TextMeshProUGUI>().text = ("Reviving in " + reviveTimer);



    }
    IEnumerator reduceRevive()
    {
        yield return new WaitForSeconds(1f);
        reviveTimer++;
        if (reviveTimer > reviveTimeTotal)
        {
            reviveTimer = reviveTimeTotal;
        }
        notRevivingBool = false;
        ///print(reviveTimer);
        reviveText.GetComponent<TextMeshProUGUI>().text = ("Reviving in " + reviveTimer);


    }




    // Update is called once per frame
    void Update()
    {

        /// && playermanager23 != null

        // Does nothing if playerManager isn't assigned to stop error spam - Oliver.C
        if (downed)   ///if downed then stop all movement
        {
            ///print(this.GetComponentInChildren<reviveBox>().reviving);
            reviveText.SetActive(true);



            if (!doOnce)
            {
                ///playermanager23.GetComponent<PlayerManager>().numberofdowns += 1;
                doOnce = true;
            }
            this.GetComponent<Player>().playerInput.currentActionMap.Disable();
            if (this.GetComponentInChildren<reviveBox>().reviving == true && reviving2 == false) ///if a player is nearby then add a second to the revive timer
            {
                StartCoroutine(reviving());
                reviving2 = true;


            }
            else if (this.GetComponentInChildren<reviveBox>().reviving == false && reviveTimer > 0 && notRevivingBool == false) ///if a player leaves then slowly reduce the revive timer
            {
                StartCoroutine(reduceRevive());
                notRevivingBool = true;


            }
            if (reviveTimer == 0) ///if the revive timer is == the amount of time needed to revive (change this is editor) then the player revives
            {
                ///this.GetComponent<CharacterStats>().Heal(20);
                ///playermanager23.GetComponent<playerManager>().numberofdowns -= 1;
                this.GetComponent<PlayerStats>().health = this.GetComponent<PlayerStats>().maxHealth;
                downed = false;
                this.GetComponent<Player>().playerInput.currentActionMap.Enable();
                doOnce = false;
                reviveTimer = reviveTimeTotal;

            }

        }
        else
        {
            ///print("not reviving");
            reviveTimer = reviveTimeTotal;
            downed = false;
            reviveText.SetActive(false);
        }


    }



}