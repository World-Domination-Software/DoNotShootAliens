using UnityEngine;
using ZombieRescue.Core;

namespace ZombieRescue.Modes
{
    public class SinglePlayerModeController : MonoBehaviour, IGameModeController
    {
        private GameSessionManager _session;

        public void Initialize(GameSessionManager session)
        {
            _session = session;
        }

        public void OnMatchStart()
        {
            Debug.Log("[SinglePlayer] Match started.");
        }

        public void OnMatchEnd(bool victory)
        {
            Debug.Log($"[SinglePlayer] Match ended. Victory: {victory}");
        }

        public void OnPlayerDied(int playerIndex)
        {
            Debug.Log($"[SinglePlayer] Player {playerIndex} died – triggering defeat.");
            _session.EndMatch(false);
        }

        public void CheckVictoryConditions()
        {
            if (_session.CurrentMap == null) return;

            int rescued = _session.CurrentMap.targetSurvivorsToRescue;
            // ScoreManager tracks rescued count; access via singleton if needed.
            // Actual survivor count comparison is handled by ScoreManager/events.
            Debug.Log($"[SinglePlayer] Victory condition: rescue {rescued} survivors.");
        }
    }
}
