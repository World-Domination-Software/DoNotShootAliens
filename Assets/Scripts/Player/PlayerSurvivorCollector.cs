using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using ZombieRescue.Survivors;

namespace ZombieRescue.Player
{
    /// <summary>
    /// Detects survivors in trigger range and manages the follower group.
    /// </summary>
    public class PlayerSurvivorCollector : MonoBehaviour
    {
        [Header("Rescue Settings")]
        [SerializeField] private int maxFollowers = 5;

        [Header("Events")]
        public UnityEvent<SurvivorController> OnSurvivorRescued = new UnityEvent<SurvivorController>();

        private readonly List<SurvivorController> _followers = new List<SurvivorController>();

        public int FollowerCount => _followers.Count;

        // ── Trigger detection ──────────────────────────────────────────────────
        private void OnTriggerEnter(Collider other)
        {
            if (_followers.Count >= maxFollowers) return;

            var survivor = other.GetComponentInParent<SurvivorController>();
            if (survivor != null && survivor.State == SurvivorState.Idle)
                RescueSurvivor(survivor);
        }

        // ── Public API ─────────────────────────────────────────────────────────
        public void RescueSurvivor(SurvivorController survivor)
        {
            if (survivor == null || _followers.Contains(survivor)) return;

            _followers.Add(survivor);
            survivor.Rescue(this);
            OnSurvivorRescued.Invoke(survivor);
        }

        public void RemoveFollower(SurvivorController survivor)
        {
            _followers.Remove(survivor);
        }
    }
}
