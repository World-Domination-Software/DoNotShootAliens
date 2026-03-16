using System.Collections.Generic;
using UnityEngine;
using ZombieRescue.Player;

namespace ZombieRescue.Scoring
{
    /// <summary>
    /// Central score tracker. Awards/penalises points and builds the MatchResults object.
    /// </summary>
    public class ScoreManager : MonoBehaviour
    {
        [Header("Profile")]
        [SerializeField] private ScoreProfile scoreProfile;

        // ── Runtime state ──────────────────────────────────────────────────────
        private int   _totalScore;
        private int   _survivorsRescued;
        private int   _survivorsLost;
        private int   _zombiesKilled;
        private float _totalDamageTaken;

        public int TotalScore => _totalScore;

        // ── Award methods ──────────────────────────────────────────────────────
        public void AwardZombieKill(int killerPlayerIndex)
        {
            _zombiesKilled++;
            _totalScore += scoreProfile != null ? scoreProfile.pointsPerZombieKill : 100;
            Debug.Log($"[Score] Zombie killed by player {killerPlayerIndex}. Total: {_totalScore}");
        }

        public void AwardSurvivorRescued(int rescuerPlayerIndex)
        {
            _survivorsRescued++;
            _totalScore += scoreProfile != null ? scoreProfile.pointsPerSurvivorRescued : 500;
            Debug.Log($"[Score] Survivor rescued by player {rescuerPlayerIndex}. Total: {_totalScore}");
        }

        public void PenalizeSurvivorLost()
        {
            _survivorsLost++;
            _totalScore -= scoreProfile != null ? scoreProfile.penaltyPerSurvivorLost : 200;
            Debug.Log($"[Score] Survivor lost. Total: {_totalScore}");
        }

        public void PenalizeDamageTaken(int playerIndex, float damage)
        {
            _totalDamageTaken += damage;
            int penalty = scoreProfile != null
                ? Mathf.RoundToInt(damage * scoreProfile.penaltyPerDamageTaken)
                : Mathf.RoundToInt(damage);
            _totalScore -= penalty;
        }

        // ── Results builder ────────────────────────────────────────────────────
        public MatchResults BuildResults(List<PlayerRoundStats> playerStats, float matchDuration, bool isVictory)
        {
            var results = new MatchResults
            {
                isVictory            = isVictory,
                matchDuration        = matchDuration,
                totalSurvivorsRescued = _survivorsRescued,
                totalSurvivorsLost   = _survivorsLost,
                totalZombiesKilled   = _zombiesKilled
            };

            int    bestScore        = int.MinValue;
            int    winnerIndex      = -1;
            int    pointsPerKill    = scoreProfile != null ? scoreProfile.pointsPerZombieKill       : 100;
            int    pointsPerRescue  = scoreProfile != null ? scoreProfile.pointsPerSurvivorRescued  : 500;
            int    penaltyPerDamage = scoreProfile != null ? scoreProfile.penaltyPerDamageTaken     : 1;

            for (int i = 0; i < playerStats.Count; i++)
            {
                var ps = playerStats[i];
                int playerScore = ps.ZombiesKilled    * pointsPerKill
                                + ps.SurvivorsRescued * pointsPerRescue
                                - Mathf.RoundToInt(ps.DamageTaken * penaltyPerDamage);

                var entry = new PlayerResultEntry
                {
                    playerName       = $"Player {i + 1}",
                    zombiesKilled    = ps.ZombiesKilled,
                    survivorsRescued = ps.SurvivorsRescued,
                    damageTaken      = ps.DamageTaken,
                    shotsFired       = ps.ShotsFired,
                    shotsHit         = ps.ShotsHit,
                    accuracy         = ps.Accuracy,
                    score            = playerScore
                };
                results.playerResults.Add(entry);

                if (playerScore > bestScore)
                {
                    bestScore   = playerScore;
                    winnerIndex = i;
                }
            }

            results.winnerIndex = playerStats.Count > 1 ? winnerIndex : -1;
            return results;
        }
    }
}
