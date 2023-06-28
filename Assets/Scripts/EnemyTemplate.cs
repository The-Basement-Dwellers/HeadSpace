using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

[CreateAssetMenu(fileName = "New Enemy", menuName = "Enemy")]
public class EnemyTemplate : ScriptableObject, ISerializationCallbackReceiver
{
    public float enemyHealth;
    public float enemymaxHealth;
    public float enemyDamage;

    
    public float liveenemyHealth;
    public float liveenemyDamage;

    public void OnAfterDeserialize()
    {
        liveenemyHealth = enemyHealth;
        liveenemyDamage = enemyDamage;
    }

    public void OnBeforeSerialize()
    {
    }
}

