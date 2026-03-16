using UnityEngine;

namespace ZombieRescue.AI
{
    /// <summary>
    /// Top-level zombie coordinator.
    /// Uses simple Transform.MoveTowards – no NavMesh dependency required.
    /// </summary>
    public class ZombieAIController : MonoBehaviour
    {
        private enum ZombieState { Idle, Chasing, Attacking }

        [Header("Definition")]
        [SerializeField] private ZombieDefinition definition;

        [Header("Components")]
        [SerializeField] private ZombieTargeting targeting;
        [SerializeField] private ZombieAttack    attack;
        [SerializeField] private ZombieHealth    health;

        private ZombieState _state = ZombieState.Idle;

        // ── Lifecycle ──────────────────────────────────────────────────────────
        private void Update()
        {
            if (!health.IsAlive) return;

            Transform target = targeting != null ? targeting.CurrentTarget : null;

            if (target == null)
            {
                _state = ZombieState.Idle;
                return;
            }

            float dist        = Vector3.Distance(transform.position, target.position);
            float attackRange = definition != null ? definition.AttackRange : 1.5f;
            float moveSpeed   = definition != null ? definition.MoveSpeed    : 2.5f;

            if (dist <= attackRange)
            {
                _state = ZombieState.Attacking;
                // Attack component handles damage timing; just face the target.
                FaceTarget(target.position);
            }
            else
            {
                _state = ZombieState.Chasing;
                MoveToward(target.position, moveSpeed);
            }
        }

        // ── Movement helpers ───────────────────────────────────────────────────
        private void MoveToward(Vector3 destination, float speed)
        {
            Vector3 direction = (destination - transform.position).normalized;
            transform.position = Vector3.MoveTowards(
                transform.position, destination, speed * Time.deltaTime);
            FaceTarget(destination);
        }

        private void FaceTarget(Vector3 destination)
        {
            Vector3 dir = destination - transform.position;
            dir.y = 0f;
            if (dir.sqrMagnitude > 0.001f)
                transform.rotation = Quaternion.LookRotation(dir);
        }
    }
}
