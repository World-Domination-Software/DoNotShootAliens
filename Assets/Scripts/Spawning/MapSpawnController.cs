using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using ZombieRescue.Survivors;

namespace ZombieRescue.Spawning
{
    /// <summary>
    /// Instantiates and tracks players, zombies, and survivors at the designated spawn points.
    /// </summary>
    public class MapSpawnController : MonoBehaviour
    {
        [Header("Spawn Points")]
        [SerializeField] private List<SpawnPointMarker> spawnPoints = new List<SpawnPointMarker>();

        [Header("Prefabs")]
        [SerializeField] private GameObject zombiePrefab;
        [SerializeField] private GameObject survivorPrefab;
        [SerializeField] private GameObject playerPrefab;

        [Header("Level Config")]
        [SerializeField] private MapLevelConfig levelConfig;

        // ── Runtime collections ────────────────────────────────────────────────
        private readonly List<GameObject>         _spawnedZombies   = new List<GameObject>();
        private readonly List<SurvivorController> _spawnedSurvivors = new List<SurvivorController>();
        private readonly List<GameObject>         _spawnedPlayers   = new List<GameObject>();

        // ── Read-only properties ───────────────────────────────────────────────
        public int AliveZombieCount   => _spawnedZombies.Count(z => z != null);
        public int AliveSurvivorCount => _spawnedSurvivors.Count(s => s != null && s.IsAlive);

        // ── Spawn API ──────────────────────────────────────────────────────────
        public void SpawnPlayers(int count)
        {
            if (playerPrefab == null) return;

            var markers = GetMarkersOfType(SpawnMarkerType.Player);
            int toSpawn = Mathf.Min(count, markers.Count);

            for (int i = 0; i < toSpawn; i++)
            {
                var go = Instantiate(playerPrefab, markers[i].transform.position, markers[i].transform.rotation);
                _spawnedPlayers.Add(go);
            }
        }

        public void SpawnZombies()
        {
            if (zombiePrefab == null || levelConfig == null) return;

            var markers = GetMarkersOfType(SpawnMarkerType.Zombie);
            int toSpawn = Mathf.Min(levelConfig.zombieCount, markers.Count);

            for (int i = 0; i < toSpawn; i++)
            {
                var go = Instantiate(zombiePrefab, markers[i].transform.position, markers[i].transform.rotation);
                _spawnedZombies.Add(go);
            }
        }

        public void SpawnSurvivors()
        {
            if (survivorPrefab == null || levelConfig == null) return;

            var markers = GetMarkersOfType(SpawnMarkerType.Survivor);
            int toSpawn = Mathf.Min(levelConfig.survivorCount, markers.Count);

            for (int i = 0; i < toSpawn; i++)
            {
                var go = Instantiate(survivorPrefab, markers[i].transform.position, markers[i].transform.rotation);
                var sc = go.GetComponent<SurvivorController>();
                if (sc != null) _spawnedSurvivors.Add(sc);
            }
        }

        public void DespawnAll()
        {
            foreach (var go in _spawnedZombies)   if (go != null) Destroy(go);
            foreach (var sc in _spawnedSurvivors) if (sc != null) Destroy(sc.gameObject);
            foreach (var go in _spawnedPlayers)   if (go != null) Destroy(go);

            _spawnedZombies.Clear();
            _spawnedSurvivors.Clear();
            _spawnedPlayers.Clear();
        }

        // ── Helpers ────────────────────────────────────────────────────────────
        private List<SpawnPointMarker> GetMarkersOfType(SpawnMarkerType type)
            => spawnPoints.Where(s => s != null && s.MarkerType == type).ToList();
    }
}
