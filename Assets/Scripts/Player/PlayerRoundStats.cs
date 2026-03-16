using UnityEngine;

namespace ZombieRescue.Player
{
    /// <summary>
    /// Accumulates per-round statistics for a single player.
    /// Call StartTracking() when the match begins and StopTracking() when the player dies or match ends.
    /// </summary>
    public class PlayerRoundStats : MonoBehaviour
    {
        // ── Raw counters ───────────────────────────────────────────────────────
        private int   _shotsFired;
        private int   _shotsHit;
        private int   _zombiesKilled;
        private int   _survivorsRescued;
        private float _damageTaken;
        private float _timeAlive;
        private float _trackingStartTime;

        // ── Read-only accessors ────────────────────────────────────────────────
        public int   ShotsFired       => _shotsFired;
        public int   ShotsHit         => _shotsHit;
        public int   ZombiesKilled    => _zombiesKilled;
        public int   SurvivorsRescued => _survivorsRescued;
        public float DamageTaken      => _damageTaken;
        public float TimeAlive        => _timeAlive;

        /// <summary>Shot accuracy in [0,1]. Returns 0 if no shots have been fired.</summary>
        public float Accuracy => _shotsFired > 0 ? (float)_shotsHit / _shotsFired : 0f;

        // ── Recording methods ──────────────────────────────────────────────────
        public void RecordShot()                      => _shotsFired++;
        public void RecordHit()                       => _shotsHit++;
        public void RecordZombieKill()                => _zombiesKilled++;
        public void RecordSurvivorRescue()            => _survivorsRescued++;
        public void RecordDamageTaken(float amount)   => _damageTaken += amount;

        // ── Tracking lifecycle ─────────────────────────────────────────────────
        public void StartTracking()
        {
            _trackingStartTime = Time.time;
        }

        public void StopTracking()
        {
            _timeAlive = Time.time - _trackingStartTime;
        }
    }
}
