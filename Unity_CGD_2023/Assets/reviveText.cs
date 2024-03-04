using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;

public class ReviveText : MonoBehaviour
{

    public Transform child;

    void Update()
    {
        ///child.transform.rotation = Quaternion.Euler(0.0f, 0.0f, gameObject.transform.rotation.z * -1.0f);
        ///child.transform.position = new Vector2(this.transform.position.x + 0.5f, this.transform.position.y + 0.3f);
        child.transform.position = transform.localPosition;
    }

}