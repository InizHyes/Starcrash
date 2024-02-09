using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class laserscript : MonoBehaviour
{
    public LayerMask layersToHit;

    [SerializeField]
    private float beamLength;

    private PlayerController Player;

    // Start is called before the first frame update
    void Start()
    {
        Player = GetComponentInParent<PlayerController>();
    }

    // Update is called once per frame
    void Update()
    {
        float angle = Player.transform.eulerAngles.z * Mathf.Deg2Rad;
        Vector2 dir = new Vector2(Mathf.Cos(angle), Mathf.Sin(angle));

        RaycastHit2D hit = Physics2D.Raycast(transform.position, dir, beamLength, layersToHit);
        if (hit.collider == null)
        {
            transform.localScale = new Vector3(beamLength, transform.localScale.y, 1);
            return;
        }
        transform.localScale = new Vector3(hit.distance, transform.localScale.y, 1);

    }
}
