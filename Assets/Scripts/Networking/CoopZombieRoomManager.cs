using Mirror;
using UnityEngine;

namespace ZombieRescue.Networking
{
    /// <summary>
    /// Network room manager for co-op sessions. Handles up to 4 players.
    /// </summary>
    public class CoopZombieRoomManager : NetworkRoomManager
    {
        [Header("Room Settings")]
        [Tooltip("Maximum simultaneous players in a co-op session.")]
        [SerializeField] private int maxPlayerCount = 4;

        public override void Awake()
        {
            base.Awake();
            maxConnections = maxPlayerCount;
        }

        // ── Server callbacks ───────────────────────────────────────────────────
        public override void OnRoomServerPlayersReady()
        {
            base.OnRoomServerPlayersReady();
            Debug.Log("[CoopRoom] All players ready – starting game.");
        }

        public override void OnRoomServerConnect(NetworkConnectionToClient conn)
        {
            base.OnRoomServerConnect(conn);
            Debug.Log($"[CoopRoom] Client connected: connId={conn.connectionId}");
        }

        public override void OnRoomServerDisconnect(NetworkConnectionToClient conn)
        {
            base.OnRoomServerDisconnect(conn);
            Debug.Log($"[CoopRoom] Client disconnected: connId={conn.connectionId}");
        }

        // ── Client callbacks ───────────────────────────────────────────────────
        public override void OnRoomClientSceneChanged()
        {
            base.OnRoomClientSceneChanged();
            Debug.Log("[CoopRoom] Client scene changed.");
        }

        public override void OnClientConnect()
        {
            base.OnClientConnect();
            Debug.Log("[CoopRoom] Connected to host.");
        }

        public override void OnClientDisconnect()
        {
            base.OnClientDisconnect();
            Debug.Log("[CoopRoom] Disconnected from host.");
        }
    }
}
