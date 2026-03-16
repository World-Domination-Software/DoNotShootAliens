using System.Collections.Generic;
using UnityEngine;
using ZombieRescue.Scoring;

namespace ZombieRescue.Persistence
{
    // TODO: Integrate with Unity Gaming Services Leaderboards in Phase 3.
    /// <summary>
    /// Stub implementation of IHighScoreService backed by Unity Gaming Services.
    /// All methods are unimplemented and log a warning until Phase 3 wires them up.
    /// </summary>
    public class UnityLeaderboardHighScoreService : MonoBehaviour, IHighScoreService
    {
        public void SaveResult(MatchResults result)
        {
            Debug.LogWarning("[Leaderboard] TODO: SaveResult – Unity Gaming Services not yet integrated.");
        }

        public List<MatchResults> LoadResults()
        {
            Debug.LogWarning("[Leaderboard] TODO: LoadResults – Unity Gaming Services not yet integrated.");
            return new List<MatchResults>();
        }

        public void ClearResults()
        {
            Debug.LogWarning("[Leaderboard] TODO: ClearResults – Unity Gaming Services not yet integrated.");
        }
    }
}
