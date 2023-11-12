using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ignoreLayerCollision : MonoBehaviour
{
    private void Start()
    {
        Physics2D.IgnoreLayerCollision(3, 6);
    }
}
