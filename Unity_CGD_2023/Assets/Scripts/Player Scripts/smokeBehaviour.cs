using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class smokeBehaviour : MonoBehaviour
{
    [SerializeField]
    public Animator smoke;

    [SerializeField]
    public RuntimeAnimatorController animController;

    private shootingScript gunScript;

    [SerializeField]
    public AnimationClip clip;

    public float smokeTime;

    private string animName;

    void Start()
    {
        smoke = GetComponentInChildren<Animator>();
        gunScript = GetComponentInParent<shootingScript>();

        animName = "Base Layer." + clip.name;
    }

    // Update is called once per frame
    void Update()
    {
        if (gunScript.playMuzzleSmoke)
        {
            Smoking();
        }
    }
    private void Smoking()
    {
        smoke.SetTrigger("StartAnim");

        smoke.Play(animName, 0, 0);

        smoke.ResetTrigger("StartAnim");

        gunScript.playMuzzleSmoke = false;
    }


}
