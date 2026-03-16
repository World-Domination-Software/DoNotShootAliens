using UnityEngine;
using ZombieRescue.Core;

namespace ZombieRescue.AI
{
    /// <summary>
    /// Checks each frame whether the current target is within attack range and
    /// applies damage on cooldown.
    /// </summary>
    public class ZombieAttack : MonoBehaviour
    {
        [Header("Definition")]
        [SerializeField] private ZombieDefinition definition;

        [Header("References")]
        [SerializeField] private ZombieTargeting targeting;

        private float _attackTimer;

        // ── Lifecycle ──────────────────────────────────────────────────────────
        private void Update()
        {
            _attackTimer -= Time.deltaTime;

            Transform target = targeting != null ? targeting.CurrentTarget : null;
            if (target == null) return;

            float dist        = Vector3.Distance(transform.position, target.position);
            float attackRange = definition != null ? definition.AttackRange : 1.5f;

            if (dist <= attackRange && _attackTimer <= 0f)
            {
                var damageable = target.GetComponentInParent<IDamageable>();
                if (damageable != null)
                    Attack(damageable);
            }
        }

        // ── Attack ─────────────────────────────────────────────────────────────
        private void Attack(IDamageable target)
        {
            float cooldown = definition != null ? definition.AttackCooldown : 1f;
            float damage   = definition != null ? definition.AttackDamage   : 10f;

            _attackTimer = cooldown;
            target.TakeDamage(damage);
        }
    }
}
