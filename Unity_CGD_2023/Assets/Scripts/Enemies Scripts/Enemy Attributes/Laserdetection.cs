using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Laserdetection : MonoBehaviour
{
    // layers that the laser can hit (editable)
    private Animator animate;
    public LayerMask detectedLayers;
    public int laserState = 2; // 0 is off, 1 is charging and 2 is firing
    private bool doOnce = true;
    private bool doOnce2 = true;
    private bool doOnce3 = true;
    void Start()
    {
        animate = GetComponent<Animator>();
    }

    void Update()
    {
        float angle = transform.eulerAngles.z * Mathf.Deg2Rad;
        Vector2 dir = new Vector2(Mathf.Cos(angle), Mathf.Sin(angle));

        if (laserState == 2)
        {
            if (doOnce)
            {
                animate.Play("LaserActive");
                doOnce = false;
                doOnce2 = true;
                doOnce3 = true;
            }
            
            RaycastHit2D laserhit = Physics2D.Raycast(transform.position, dir, 50f, detectedLayers);
            if (laserhit.collider == null)
            {
                transform.localScale = new Vector3(50f, transform.localScale.y, 1);
                return;
            }
            transform.localScale = new Vector3(laserhit.distance, transform.localScale.y, 1);
            if (laserhit.collider.tag == "Player")
            {
                //DO DAMAGE
                Destroy(laserhit.collider.gameObject);
            }
        }
        else if (laserState == 1)
        {
            if (doOnce2)
            {
                animate.Play("LaserBeginCharge");
                doOnce2 = false;
                doOnce = true;
                doOnce3 = true;
            }
            
            transform.localScale = new Vector3((transform.localScale.y), (transform.localScale.y), 1);
        }
        else
        {
            if (doOnce3)
            {
                animate.Play("LaserBeginCharge");
                doOnce3 = false;
                doOnce = true;
                doOnce2 = true;
            }
            transform.localScale = new Vector3(0, transform.localScale.y, 1);
            animate.Play("LaserActive");
        }

    }
}
