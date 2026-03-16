using System.Collections.Generic;
using UnityEngine;
using ZombieRescue.Core;

namespace ZombieRescue.Modes
{
    public class CoopModeController : MonoBehaviour, IGameModeController
    {
        private GameSessionManager _session;
        private readonly List<int> _alivePlayers = new List<int>();

        public void Initialize(GameSessionManager session)
        {
            _session = session;
        }

        public void OnMatchStart()
        {
            _alivePlayers.Clear();
            // Populate with player indices – actual count known after spawn
            Debug.Log("[Coop] Match started.");
        }

        public void OnMatchEnd(bool victory)
        {
            Debug.Log($"[Coop] Match ended. Victory: {victory}");
        }

        public void OnPlayerDied(int playerIndex)
        {
            _alivePlayers.Remove(playerIndex);
            Debug.Log($"[Coop] Player {playerIndex} died. Alive: {_alivePlayers.Count}");

            if (_alivePlayers.Count == 0)
            {
                Debug.Log("[Coop] All players dead – triggering defeat.");
                _session.EndMatch(false);
            }
        }

        public void CheckVictoryConditions()
        {
            if (_session.CurrentMap == null) return;

            int target = _session.CurrentMap.targetSurvivorsToRescue;
            Debug.Log($"[Coop] Victory condition: rescue {target} survivors.");
        }

        /// <summary>Registers a player as alive at match start.</summary>
        public void RegisterPlayer(int playerIndex)
        {
            if (!_alivePlayers.Contains(playerIndex))
                _alivePlayers.Add(playerIndex);
        }
    }
}
