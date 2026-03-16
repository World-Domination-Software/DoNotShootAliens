using UnityEngine;
using UnityEngine.Events;
using ZombieRescue.Core;
using ZombieRescue.Player;
using ZombieRescue.Scoring;

namespace ZombieRescue.Survivors
{
    /// <summary>
    /// Represents an NPC survivor that can be rescued by a player.
    /// </summary>
    public class SurvivorController : MonoBehaviour, IDamageable
    {
        [Header("Health")]
        [SerializeField] private float maxHealth = 50f;

        private float         _currentHealth;
        private SurvivorState _state = SurvivorState.Idle;

        public SurvivorState          State            => _state;
        public PlayerSurvivorCollector AssignedRescuer { get; private set; }
        public bool IsAlive => _currentHealth > 0f;

        [Header("Events")]
        public UnityEvent OnRescued = new UnityEvent();
        public UnityEvent OnDied    = new UnityEvent();

        private SurvivorFollowTarget _followTarget;

        // ── Lifecycle ──────────────────────────────────────────────────────────
        private void Awake()
        {
            _currentHealth = maxHealth;
            _followTarget  = GetComponent<SurvivorFollowTarget>();
        }

        // ── Rescue ─────────────────────────────────────────────────────────────
        /// <summary>Called by <see cref="PlayerSurvivorCollector"/> when in range.</summary>
        public void Rescue(PlayerSurvivorCollector rescuer)
        {
            if (_state != SurvivorState.Idle) return;

            AssignedRescuer = rescuer;
            _state          = SurvivorState.Following;

            if (_followTarget != null)
                _followTarget.SetTarget(rescuer.transform);

            OnRescued.Invoke();
        }

        // ── IDamageable ────────────────────────────────────────────────────────
        public void TakeDamage(float amount)
        {
            if (_state == SurvivorState.Dead || _state == SurvivorState.Rescued) return;
            if (!IsAlive) return;

            _currentHealth = Mathf.Max(0f, _currentHealth - amount);
            if (_currentHealth <= 0f)
                Die();
        }

        private void Die()
        {
            _state = SurvivorState.Dead;
            AssignedRescuer?.RemoveFollower(this);
            OnDied.Invoke();
            Destroy(gameObject, 0.1f);
        }
    }
}
