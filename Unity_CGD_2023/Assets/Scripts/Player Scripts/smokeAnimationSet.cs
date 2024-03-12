using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class smokeAnimationSet : MonoBehaviour
{
    private smokeBehaviour smokeBehaviour;

    void Start()
    {
        smokeBehaviour = GetComponentInParent<smokeBehaviour>();
        GetComponent<Animator>().runtimeAnimatorController = smokeBehaviour.animController;
    }
}
