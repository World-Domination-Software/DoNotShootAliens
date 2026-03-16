using UnityEngine;

namespace ZombieRescue.AI
{
    /// <summary>
    /// Periodically scans for the nearest player (and optionally survivor) within detection radius.
    /// </summary>
    public class ZombieTargeting : MonoBehaviour
    {
        [Header("Definition")]
        [SerializeField] private ZombieDefinition definition;

        [Header("Behaviour")]
        [SerializeField] private bool canTargetSurvivors = true;

        private Transform _currentTarget;
        private float     _updateInterval = 0.5f;
        private float     _nextUpdateTime;

        public Transform CurrentTarget => _currentTarget;

        // ── Lifecycle ──────────────────────────────────────────────────────────
        private void Update()
        {
            if (Time.time < _nextUpdateTime) return;
            _nextUpdateTime = Time.time + _updateInterval;
            UpdateTarget();
        }

        // ── Target selection ───────────────────────────────────────────────────
        public void UpdateTarget()
        {
            Transform nearest = GetNearestPlayer();

            if (canTargetSurvivors)
            {
                Transform survivor = GetNearestUnrescuedSurvivor();
                if (survivor != null)
                {
                    float playerDist   = nearest   != null ? Vector3.Distance(transform.position, nearest.position)  : float.MaxValue;
                    float survivorDist = Vector3.Distance(transform.position, survivor.position);
                    if (survivorDist < playerDist)
                        nearest = survivor;
                }
            }

            _currentTarget = nearest;
        }

        public Transform GetNearestPlayer()
        {
            float     radius  = definition != null ? definition.DetectionRadius : 15f;
            Collider[] hits   = Physics.OverlapSphere(transform.position, radius, LayerMask.GetMask("Player"));
            Transform  best   = null;
            float      bestDist = float.MaxValue;

            foreach (var col in hits)
            {
                float d = Vector3.Distance(transform.position, col.transform.position);
                if (d < bestDist)
                {
                    bestDist = d;
                    best     = col.transform;
                }
            }
            return best;
        }

        public Transform GetNearestUnrescuedSurvivor()
        {
            float      radius  = definition != null ? definition.DetectionRadius : 15f;
            Collider[] hits    = Physics.OverlapSphere(transform.position, radius, LayerMask.GetMask("Survivor"));
            Transform  best    = null;
            float      bestDist = float.MaxValue;

            foreach (var col in hits)
            {
                var sc = col.GetComponentInParent<Survivors.SurvivorController>();
                if (sc == null || sc.State != Survivors.SurvivorState.Idle) continue;

                float d = Vector3.Distance(transform.position, col.transform.position);
                if (d < bestDist)
                {
                    bestDist = d;
                    best     = col.transform;
                }
            }
            return best;
        }

        // ── Editor visualisation ───────────────────────────────────────────────
        private void OnDrawGizmosSelected()
        {
            if (definition == null) return;
            Gizmos.color = Color.red;
            Gizmos.DrawWireSphere(transform.position, definition.DetectionRadius);
        }
    }
}
