using System;
using System.Collections.Generic;

namespace ZombieRescue.Scoring
{
    /// <summary>Per-player breakdown for the end-of-match results screen.</summary>
    [Serializable]
    public class PlayerResultEntry
    {
        public string playerName;
        public int    zombiesKilled;
        public int    survivorsRescued;
        public float  damageTaken;
        public int    shotsFired;
        public int    shotsHit;
        public float  accuracy;
        public int    score;
    }

    /// <summary>Complete snapshot of a finished match, suitable for serialisation to JSON.</summary>
    [Serializable]
    public class MatchResults
    {
        public bool   isVictory;
        public float  matchDuration;
        public int    totalSurvivorsRescued;
        public int    totalSurvivorsLost;
        public int    totalZombiesKilled;

        public List<PlayerResultEntry> playerResults = new List<PlayerResultEntry>();

        /// <summary>Index of the top-scoring player in co-op, or -1 for single-player.</summary>
        public int winnerIndex = -1;
    }
}
