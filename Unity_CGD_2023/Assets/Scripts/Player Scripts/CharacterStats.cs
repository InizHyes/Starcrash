using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterStats : MonoBehaviour
{
    [Header("Max Stats")]
    [SerializeField] public float maxHealth;

    [Header("Stats")]
    [SerializeField] public float health;

    [Header("Stats Flags")]
    [SerializeField] public bool isDead;

    

    AudioSource audioPlayer;

    [SerializeField] private AudioClip takeDMG;



    private void Start()
    {
        InitVariables();
    }
    public virtual void CheckStats()
    {
        if(this.GetComponent<Down>().downed == false)
        {
            isDead = false;
        }
        
        if(health <= 0)
        {
            health = 0;
            Die();
        }
        if(health >= maxHealth)
        {
            health = maxHealth;
        }
    }

    public virtual void Die()
    {
        this.GetComponent<Down>().downed = true;
        isDead = true;

    }

    public virtual void SetStatsTo(float healthSetTo)
    {
        health = healthSetTo;
        CheckStats();
    }

    public void TakeDamage(float damage)
    {
        audioPlayer.clip = takeDMG;
        audioPlayer.Play();
        float healthAfterDamage = health - damage;
        SetStatsTo(healthAfterDamage);
    }

    public void Heal(float heal)
    {
        float healthAfterHeal = health + heal;
        SetStatsTo(healthAfterHeal);
    }
    public virtual void InitVariables()
    {
        audioPlayer = GetComponent<AudioSource>();
        maxHealth = 100;
        SetStatsTo(maxHealth);
        isDead = false;
    }
}
