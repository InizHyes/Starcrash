using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerStats : CharacterStats
{
    private GameObject player;

    private void Start()
    {
        GetReferences();
        InitVariables();
        player = this.gameObject;
    }

    private void GetReferences()
    {
    }
    private void Update()
    {
        CheckStats();
    }

    public override void CheckStats()
    {
        base.CheckStats();
    }

    public override void InitVariables()
    {
        base.InitVariables();
    }
}
