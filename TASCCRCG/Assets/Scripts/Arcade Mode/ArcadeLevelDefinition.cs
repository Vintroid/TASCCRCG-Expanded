using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(
    fileName = "NewArcadeLevel",
    menuName = "TASCCRCG/Arcade/Level Definition")]
public class ArcadeLevelDefinition : ScriptableObject // Script with Level Info
{
    [SerializeField] private WaveDefinition[] waves;
    [SerializeField] private float timeBetweenWaves = 3f;

    public WaveDefinition[] Waves => waves;
    public float TimeBetweenWaves => timeBetweenWaves;
}
