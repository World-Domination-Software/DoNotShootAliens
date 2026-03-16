using Mirror;
using UnityEngine;
using UnityEngine.Events;
using ZombieRescue.Core;

namespace ZombieRescue.Player
{
    /// <summary>
    /// Tracks player hit points with server-authoritative damage and sync.
    /// </summary>
    public class PlayerHealth : NetworkBehaviour, IDamageable
    {
        [Header("Health")]
        [SerializeField] private float maxHealth = 100f;

        [SyncVar(hook = nameof(OnHealthSynced))]
        private float currentHealth;

        [Header("Events")]
        public UnityEvent OnDeath = new UnityEvent();
        public UnityEvent<float, float> OnHealthChanged = new UnityEvent<float, float>();

        public bool IsAlive => currentHealth > 0f;
        public float CurrentHealth => currentHealth;
        public float MaxHealth => maxHealth;

        // ── Lifecycle ──────────────────────────────────────────────────────────
        public override void OnStartServer()
        {
            base.OnStartServer();
            currentHealth = maxHealth;
        }

        // ── IDamageable ────────────────────────────────────────────────────────
        public void TakeDamage(float amount)
        {
            if (isServer)
                ApplyDamage(amount);
            else
                CmdTakeDamage(amount);
        }

        [Command]
        private void CmdTakeDamage(float amount) => ApplyDamage(amount);

        [Server]
        private void ApplyDamage(float amount)
        {
            if (!IsAlive) return;

            currentHealth = Mathf.Max(0f, currentHealth - amount);
            if (currentHealth <= 0f)
                RpcDie();
        }

        [ClientRpc]
        private void RpcDie() => Die();

        private void Die()
        {
            // currentHealth is already 0 here; just fire the event.
            OnDeath.Invoke();
            Debug.Log($"[PlayerHealth] {gameObject.name} died.");
        }

        // SyncVar hook – fires on all clients when currentHealth changes.
        private void OnHealthSynced(float oldVal, float newVal)
        {
            OnHealthChanged.Invoke(newVal, maxHealth);
        }
    }
}
