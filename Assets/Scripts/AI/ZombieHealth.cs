using UnityEngine;
using UnityEngine.Events;
using ZombieRescue.Core;
using ZombieRescue.Scoring;

namespace ZombieRescue.AI
{
    /// <summary>
    /// Tracks zombie hit points and triggers death logic.
    /// </summary>
    public class ZombieHealth : MonoBehaviour, IDamageable
    {
        [Header("Definition")]
        [SerializeField] private ZombieDefinition definition;

        [Header("Score (optional)")]
        [Tooltip("If assigned, awards score when this zombie dies.")]
        [SerializeField] private ScoreManager scoreManager;

        private float _currentHealth;

        public bool IsAlive => _currentHealth > 0f;

        [Header("Events")]
        public UnityEvent       OnDeath   = new UnityEvent();
        public UnityEvent<float> OnDamaged = new UnityEvent<float>();

        // ── Lifecycle ──────────────────────────────────────────────────────────
        private void Awake()
        {
            _currentHealth = definition != null ? definition.MaxHealth : 100f;
        }

        // ── IDamageable ────────────────────────────────────────────────────────
        public void TakeDamage(float amount)
        {
            if (!IsAlive) return;

            _currentHealth = Mathf.Max(0f, _currentHealth - amount);
            OnDamaged.Invoke(amount);

            if (_currentHealth <= 0f)
                Die();
        }

        private void Die()
        {
            scoreManager?.AwardZombieKill(-1); // -1 = no specific player attribution
            OnDeath.Invoke();
            Destroy(gameObject, 0.1f);
        }
    }
}
