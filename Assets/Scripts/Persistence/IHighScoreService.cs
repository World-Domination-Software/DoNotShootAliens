using System.Collections.Generic;
using ZombieRescue.Scoring;

namespace ZombieRescue.Persistence
{
    /// <summary>
    /// Contract for saving and loading match results (high scores).
    /// </summary>
    public interface IHighScoreService
    {
        void SaveResult(MatchResults result);
        List<MatchResults> LoadResults();
        void ClearResults();
    }
}
