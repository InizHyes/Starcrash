using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Gun_UI : MonoBehaviour
{
    public Sprite sp1, sp2;
    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.A))
        {
            GetComponent<SpriteRenderer>().sprite = sp1;
        }
        if (Input.GetKeyDown(KeyCode.H))
        {
            GetComponent<SpriteRenderer>().sprite = sp2;
        }
    }
}
