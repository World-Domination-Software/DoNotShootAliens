using UnityEngine;

namespace ZombieRescue.Camera
{
    /// <summary>
    /// Smoothly follows a target with a configurable offset and optional look-ahead.
    /// Attach to the main camera.
    /// </summary>
    public class ThirdPersonCameraFollow : MonoBehaviour
    {
        [Header("Target")]
        [SerializeField] private Transform target;

        [Header("Position")]
        [SerializeField] private Vector3 offset          = new Vector3(0f, 5f, -7f);
        [SerializeField] private float   smoothSpeed     = 5f;
        [SerializeField] private float   lookAheadDistance = 2f;

        [Header("Rotation")]
        [SerializeField] private bool lookAtTarget = true;

        // ── Lifecycle ──────────────────────────────────────────────────────────
        private void LateUpdate()
        {
            if (target == null) return;

            // Desired position: offset relative to target + optional look-ahead.
            Vector3 lookAhead       = target.forward * lookAheadDistance;
            Vector3 desiredPosition = target.position + lookAhead + offset;

            transform.position = Vector3.Lerp(
                transform.position, desiredPosition, smoothSpeed * Time.deltaTime);

            if (lookAtTarget)
                transform.LookAt(target.position + Vector3.up); // look slightly above foot pivot
        }

        // ── Public API ─────────────────────────────────────────────────────────
        public void SetTarget(Transform newTarget)
        {
            target = newTarget;
        }
    }
}
