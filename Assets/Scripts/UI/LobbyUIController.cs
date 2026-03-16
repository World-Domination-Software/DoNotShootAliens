using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using ZombieRescue.Networking;

namespace ZombieRescue.UI
{
    /// <summary>
    /// Controls the co-op pre-game lobby screen.
    /// </summary>
    public class LobbyUIController : MonoBehaviour
    {
        [Header("Buttons")]
        [SerializeField] private Button readyButton;
        [Tooltip("Only visible for the session host.")]
        [SerializeField] private Button startButton;

        [Header("Player List")]
        [SerializeField] private Transform playerListContainer;
        [SerializeField] private Text      playerCountText;

        // ── Public API ─────────────────────────────────────────────────────────
        public void SetReadyState(bool ready)
        {
            Debug.Log($"[Lobby] Local player ready: {ready}");
            // Propagate to the NetworkRoomPlayer component on the local player.
        }

        public void RefreshPlayerList(List<CoopPlayerRoomSlot> slots)
        {
            if (playerCountText != null)
                playerCountText.text = $"Players: {slots.Count} / 4";

            // Collect into a list before destroying to avoid mutating the collection mid-iteration.
            var children = new System.Collections.Generic.List<Transform>();
            foreach (Transform child in playerListContainer)
                children.Add(child);
            foreach (var child in children)
                Destroy(child.gameObject);

            foreach (var slot in slots)
            {
                var entry = new GameObject($"Slot_{slot.SlotIndex}");
                entry.transform.SetParent(playerListContainer, worldPositionStays: false);
                var label = entry.AddComponent<Text>();
                label.text = $"{slot.PlayerName}  [{(slot.IsReady ? "READY" : "waiting")}]";
            }
        }

        public void SetHostView(bool isHost)
        {
            if (startButton != null)
                startButton.gameObject.SetActive(isHost);
        }

        // ── Lifecycle ──────────────────────────────────────────────────────────
        private void Start()
        {
            readyButton?.onClick.AddListener(() => SetReadyState(true));
            startButton?.onClick.AddListener(OnStartClicked);
            SetHostView(false);
        }

        private void OnStartClicked()
        {
            Debug.Log("[Lobby] Host started the match.");
        }
    }
}
