using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(
    fileName = "NewWave",
    menuName = "TASCCRCG/Waves/Wave Definition"

)]

public class WaveDefinition : ScriptableObject
{
    [SerializeField] private List<EnemySpawnEntry> enemies = new();

    public IReadOnlyList<EnemySpawnEntry> Enemies => enemies;
}
