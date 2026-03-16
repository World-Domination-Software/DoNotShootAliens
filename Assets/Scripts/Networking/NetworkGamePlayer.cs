using System.Collections.Generic;
using Mirror;
using UnityEngine;

namespace ZombieRescue.Networking
{
    /// <summary>
    /// Tracks per-player identity and display info during a match.
    /// </summary>
    public class NetworkGamePlayer : NetworkBehaviour
    {
        // Shared static registry so other systems can find all connected players.
        public static readonly List<NetworkGamePlayer> AllPlayers = new List<NetworkGamePlayer>();

        [Header("Player Identity")]
        [SyncVar] public int PlayerIndex;
        [SyncVar] public string DisplayName = "Player";

        public bool IsLocalPlayer => isLocalPlayer;

        // ── Lifecycle ──────────────────────────────────────────────────────────
        public override void OnStartClient()
        {
            base.OnStartClient();
            AllPlayers.Add(this);
            Debug.Log($"[NetworkGamePlayer] Registered: {DisplayName} (index {PlayerIndex})");
        }

        public override void OnStopClient()
        {
            base.OnStopClient();
            AllPlayers.Remove(this);
        }

        public override void OnStartLocalPlayer()
        {
            base.OnStartLocalPlayer();
            // Request index assignment from server on connect.
            CmdRequestIndex();
        }

        [Command]
        private void CmdRequestIndex()
        {
            PlayerIndex = AllPlayers.Count - 1;
            DisplayName = $"Player {PlayerIndex + 1}";
        }
    }
}
