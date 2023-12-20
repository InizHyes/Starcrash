using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RazorBlade : MonoBehaviour
{
    [SerializeField] private GameObject attachToObject;
    [HideInInspector] public int DEFAULTSPINSPEED;
    public int spinSpeed = 250;
    public int maxSpinSpeed = 1000;

    private void Start()
    {
        DEFAULTSPINSPEED = spinSpeed;
    }

    private void Update()
    {
        transform.Rotate(0, 0, spinSpeed * Time.deltaTime, Space.Self);

        transform.position = attachToObject.transform.position;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        /*
         * Handle with RazorClass
         */

        if (collision.tag == "Player")
        {
            attachToObject.GetComponent<RazorClass>().areaTriggered();
        }
    }
}
