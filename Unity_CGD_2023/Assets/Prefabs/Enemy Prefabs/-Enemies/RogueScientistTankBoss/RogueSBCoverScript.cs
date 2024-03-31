using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RogueSBCoverScript : MonoBehaviour
{
    public string stateOfExistence = "alive";
    BoxCollider2D theBox;
    public GameObject reticlePrefab;
    public Transform targetPos;
    private Transform abovePos;
    private GameObject theTarget;
    // Start is called before the first frame update
    void Start()
    {
        theBox = gameObject.GetComponent<BoxCollider2D>();
        theBox.enabled = false;
        theTarget = Instantiate(reticlePrefab, transform.position, transform.rotation);
        theTarget.transform.localScale = new Vector2(2f, 1f);
        theTarget.GetComponent<ExplodingCrosshair>().isBlue = true;
        transform.position = transform.position + new Vector3(0, 6);
        abovePos = transform;
        targetPos = theTarget.transform;
    }

    // Update is called once per frame
    void Update()
    {
        if (transform.position == targetPos.transform.position)
        {
            transform.rotation = theTarget.transform.rotation;
            theTarget.transform.parent = gameObject.transform;
            theTarget.GetComponent<ExplodingCrosshair>().theSprite.enabled = false;
            theBox.enabled = true;
        }
        else
        {
            transform.Rotate(0, 0, 900 * Time.deltaTime);
            transform.position = Vector2.MoveTowards(transform.position, targetPos.transform.position, 0.05f);
        }

        if (stateOfExistence == "die")
        {
            Object.Destroy(this.gameObject);
        }
    }
}
