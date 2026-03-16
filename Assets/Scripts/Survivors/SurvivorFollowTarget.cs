using UnityEngine;

namespace ZombieRescue.Survivors
{
    /// <summary>
    /// Moves the survivor toward an assigned follow target while maintaining a comfortable distance.
    /// </summary>
    public class SurvivorFollowTarget : MonoBehaviour
    {
        [Header("Follow Settings")]
        [SerializeField] private float followSpeed    = 3f;
        [SerializeField] private float followDistance = 2f;
        [SerializeField] private float stopDistance   = 1.5f;

        private Transform _target;

        // ── Public API ─────────────────────────────────────────────────────────
        public void SetTarget(Transform t) => _target = t;

        // ── Lifecycle ──────────────────────────────────────────────────────────
        private void Update()
        {
            if (_target == null) return;

            float dist = Vector3.Distance(transform.position, _target.position);

            // Only move if farther away than the desired follow distance
            if (dist <= stopDistance) return;

            Vector3 destination = _target.position - (_target.forward * followDistance);
            transform.position  = Vector3.MoveTowards(
                transform.position, destination, followSpeed * Time.deltaTime);

            // Face the target
            Vector3 dir = (_target.position - transform.position);
            dir.y = 0f;
            if (dir.sqrMagnitude > 0.001f)
                transform.rotation = Quaternion.LookRotation(dir);
        }
    }
}
