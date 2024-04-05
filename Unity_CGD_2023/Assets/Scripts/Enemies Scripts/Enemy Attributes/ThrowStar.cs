using System.Collections;
using System.Collections.Generic;
using Unity.Burst.Intrinsics;
using UnityEngine;

public class ThrowStar : MonoBehaviour
{
    [HideInInspector] public int damage;

    private void Start()
    {

        Physics2D.IgnoreLayerCollision(3, 7);
        Physics2D.IgnoreLayerCollision(7, 7);
    }

    private void Awake()
    {
        //gameObject.transform.rotation = Quaternion.Euler(0, 0, 90);
    }

    public void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.tag != "Enemy" && collision.gameObject.tag != "EnemyBullet")
        {
            if (collision.gameObject.tag == "Player")
            {
                //Do large damage to player
                collision.gameObject.GetComponent<PlayerStats>().TakeDamage(damage);
            }

            Destroy(gameObject);
        }
    }
}
