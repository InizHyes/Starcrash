using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class smokeBehaviour : MonoBehaviour
{
    [SerializeField]
    public Animator smoke;

    [SerializeField]
    public RuntimeAnimatorController animController;

    private SpriteRenderer spriteRenderer;

    private shootingScript gunScript;

    [SerializeField]
    public AnimationClip clip;

    public float smokeTime;

    private string animName;

    [SerializeField]
    private Sprite empty;

    void Start()
    {
        smoke = GetComponentInChildren<Animator>();
        gunScript = GetComponentInParent<shootingScript>();
        spriteRenderer = GetComponentInChildren<SpriteRenderer>();

        animName = "Base Layer." + clip.name;
    }

    private void Awake()
    {
        
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

    public void resetSpriteFrame()
    {
        spriteRenderer.sprite = empty;
    }

}
