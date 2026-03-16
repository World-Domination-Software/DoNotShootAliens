using System.Collections.Generic;
using UnityEngine;
using ZombieRescue.AI;
using ZombieRescue.Scoring;

namespace ZombieRescue.Spawning
{
    /// <summary>
    /// ScriptableObject that defines everything needed to configure a single map/level.
    /// </summary>
    [CreateAssetMenu(fileName = "MapLevelConfig", menuName = "ZombieRescue/Map Level Config")]
    public class MapLevelConfig : ScriptableObject
    {
        [Header("Map Identity")]
        [SerializeField] public string mapName = "Unnamed Map";

        [Header("Spawn Counts")]
        [SerializeField] public int playerSpawnCount = 4;
        [SerializeField] public int zombieCount      = 20;
        [SerializeField] public int survivorCount    = 10;

        [Header("Enemy Types")]
        [SerializeField] public List<ZombieDefinition> allowedZombieTypes = new List<ZombieDefinition>();

        [Header("Win / Lose Conditions")]
        [SerializeField] public int targetSurvivorsToRescue = 5;
        [SerializeField] public int maxSurvivorsLost        = 3;

        [Header("Time Limit")]
        [Tooltip("Match time limit in seconds. 0 = no limit.")]
        [SerializeField] public float timeLimitSeconds = 0f;

        [Header("Scoring")]
        [SerializeField] public ScoreProfile scoreProfile;
    }
}
