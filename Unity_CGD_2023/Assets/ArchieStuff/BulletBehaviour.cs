using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletBehaviour : MonoBehaviour
{

    private void OnCollisionEnter2D(Collision2D collision)
    {
            print("Bullet Collided with Object");
            Destroy(this.gameObject);
    }
}
