using Mirror;
using UnityEngine;

namespace ZombieRescue.Networking
{
    /// <summary>
    /// Represents a player's slot in the pre-game room lobby.
    /// </summary>
    public class CoopPlayerRoomSlot : NetworkRoomPlayer
    {
        [Header("Slot State")]
        [SyncVar] public bool IsReady;
        [SyncVar] public int SlotIndex;
        [SyncVar] public string PlayerName = "Player";

        public override void ReadyStateChanged(bool oldReadyState, bool newReadyState)
        {
            base.ReadyStateChanged(oldReadyState, newReadyState);
            Debug.Log($"[RoomSlot] {PlayerName} (slot {SlotIndex}) ready state: {newReadyState}");
        }

        public override void OnStartLocalPlayer()
        {
            base.OnStartLocalPlayer();
            CmdSetPlayerName($"Player {SlotIndex + 1}");
        }

        [Command]
        private void CmdSetPlayerName(string name)
        {
            PlayerName = name;
        }
    }
}
