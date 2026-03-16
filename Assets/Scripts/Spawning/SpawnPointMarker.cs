using UnityEngine;

namespace ZombieRescue.Spawning
{
    /// <summary>
    /// Tags a Transform as a spawn location and shows a coloured gizmo in the editor.
    /// </summary>
    public class SpawnPointMarker : MonoBehaviour
    {
        [SerializeField] private SpawnMarkerType markerType;

        public SpawnMarkerType MarkerType => markerType;

        // ── Editor gizmo ───────────────────────────────────────────────────────
        private void OnDrawGizmos()
        {
            Gizmos.color = markerType switch
            {
                SpawnMarkerType.Player   => Color.blue,
                SpawnMarkerType.Zombie   => Color.red,
                SpawnMarkerType.Survivor => Color.green,
                _                        => Color.white
            };
            Gizmos.DrawSphere(transform.position, 0.4f);
            Gizmos.DrawWireSphere(transform.position, 0.5f);
        }
    }
}
