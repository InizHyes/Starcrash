using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveLeft : MonoBehaviour
{
    void Update()
    {
        this.transform.position = this.transform.position + new Vector3(-0.01f, 0);
    }
}
