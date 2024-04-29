using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pinball : MonoBehaviour
{
    public float strength = 10f;
    public float rotationSpeed = 1.0f;
    private bool rotating = false;
    private bool rotatingBack = false;
    private float rotatedFor = 0;

    private void Update()
    {
        if (rotating)
        {
            if (rotatedFor < 90.0f)
            {
                transform.Rotate(0, 0, rotationSpeed);
                rotatedFor += rotationSpeed;
            }
            if (rotatedFor >= 90.0f)
            {
                rotatingBack = true;
                rotating = false;
            }
        }

        if (rotatingBack)
        {
            if (rotatedFor > 0.0f)
            {
                transform.Rotate(0, 0, -rotationSpeed);
                rotatedFor -= rotationSpeed;
            }
            if (rotatedFor <= 0.0f)
            {
                rotatingBack = false;
            }
        }
    }


    private void OnTriggerEnter2D(Collider2D collision)
    {
        print(collision.name);
        if (collision.gameObject.tag == "Player" && !rotating && !rotatingBack)
        {
            Vector3 Dir = transform.up + (-transform.right);
            collision.gameObject.GetComponent<Rigidbody2D>().AddForce(Dir * strength, ForceMode2D.Impulse);
            rotating = true;

        }
    }
    
}
