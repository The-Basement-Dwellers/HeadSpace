using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

[CreateAssetMenu(fileName = "New Enemy", menuName = "Enemy")]
public class EnemyTemplate : ScriptableObject
{
    public float health;
    public float maxHealth;
    public float damage;

}

