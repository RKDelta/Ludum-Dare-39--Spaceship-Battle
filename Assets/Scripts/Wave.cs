using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class EnemySpawnData
{
    public GameObject enemyPrefab;
    public int count = 1;
    public bool randomizePosition = true;
}

[CreateAssetMenu()]
public class Wave : ScriptableObject
{
    public EnemySpawnData[] enemies;
}