using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class speeen : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        transform.Rotate(Time.deltaTime * 0, 0, 1, Space.Self);
    }
}
