using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RazorBlade : MonoBehaviour
{
    public int spinSpeed = 250;

    private void Update()
    {
        transform.Rotate(0, 0, spinSpeed * Time.deltaTime, Space.Self);
    }
}
