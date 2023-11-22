using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Gas_Release : MonoBehaviour
{
    public GameObject gasObject;
    public float MinTime;
    public float MaxTime;
    private float timer;
    private float timeWithoutGas;
    private float timeWithGas;


    // Start is called before the first frame update
    void Start()
    {
        timer = Random.Range(MinTime, MaxTime);
    }

    // Update is called once per frame
    void Update()
    {
        timeWithoutGas += Time.deltaTime;

        if (timer<=timeWithoutGas)
        {
            gasObject.SetActive(true);
            if (timer<=timeWithGas)
            {
                timer = Random.Range(MinTime, MaxTime);
                timeWithoutGas = 0;
                timeWithGas = 0;
                
            }
            else
            {
                timeWithGas += Time.deltaTime;
            }
            
        }
        else
        {
            gasObject.GetComponent<GasDamage>().timeBeforeDamage = 0;
            gasObject.SetActive(false);
        }

    }
}
