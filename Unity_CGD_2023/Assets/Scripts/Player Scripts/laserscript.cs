using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class laserscript : MonoBehaviour
{
    public LayerMask layersToHit;

    [SerializeField]
    private float beamLength;

    private PlayerController Player;

    private Player newPlayer;

    // Start is called before the first frame update
    void Start()
    {
        Player = GetComponentInParent<PlayerController>();

        newPlayer = GetComponentInParent<Player>();
    }

    // Update is called once per frame
    void Update()
    {
        //float angle = Player.transform.eulerAngles.z * Mathf.Deg2Rad;
        float angle2 = newPlayer.transform.eulerAngles.z * Mathf.Deg2Rad;

        //Vector2 dir = new Vector2(Mathf.Cos(angle), Mathf.Sin(angle));
        Vector2 dir2 = new Vector2(Mathf.Cos(angle2), Mathf.Sin(angle2));

       // RaycastHit2D hit = Physics2D.Raycast(transform.position, dir, beamLength, layersToHit);
        RaycastHit2D hit2 = Physics2D.Raycast(transform.position, dir2, beamLength, layersToHit);

        /*if (hit.collider == null)
        {
            transform.localScale = new Vector3(beamLength, transform.localScale.y, 1);
            return;
        }*/
        if (hit2.collider == null)
        {
            transform.localScale = new Vector3(beamLength, transform.localScale.y, 1);
            return;
        }

       // transform.localScale = new Vector3(hit.distance, transform.localScale.y, 1);
        transform.localScale = new Vector3(hit2.distance, transform.localScale.y, 1);

    }
}
