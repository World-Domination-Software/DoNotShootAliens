using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

namespace ZombieRescue.UI
{
    /// <summary>
    /// Drives the main menu screen buttons.
    /// </summary>
    public class MainMenuUI : MonoBehaviour
    {
        [Header("Buttons")]
        [SerializeField] private Button singlePlayerButton;
        [SerializeField] private Button hostCoopButton;
        [SerializeField] private Button joinCoopButton;
        [SerializeField] private Button quitButton;

        [Header("Scene Names")]
        [SerializeField] private string singlePlayerScene = "Gameplay";
        [SerializeField] private string lobbyScene        = "Lobby";

        // ── Lifecycle ──────────────────────────────────────────────────────────
        private void Start()
        {
            singlePlayerButton?.onClick.AddListener(OnSinglePlayerClicked);
            hostCoopButton?.onClick.AddListener(OnHostCoopClicked);
            joinCoopButton?.onClick.AddListener(OnJoinCoopClicked);
            quitButton?.onClick.AddListener(OnQuitClicked);
        }

        // ── Button handlers ────────────────────────────────────────────────────
        private void OnSinglePlayerClicked()
        {
            Debug.Log("[MainMenu] Single player selected.");
            SceneManager.LoadScene(singlePlayerScene);
        }

        private void OnHostCoopClicked()
        {
            Debug.Log("[MainMenu] Host co-op selected.");
            SceneManager.LoadScene(lobbyScene);
        }

        private void OnJoinCoopClicked()
        {
            Debug.Log("[MainMenu] Join co-op selected.");
            SceneManager.LoadScene(lobbyScene);
        }

        private void OnQuitClicked()
        {
            Debug.Log("[MainMenu] Quit.");
#if UNITY_EDITOR
            UnityEditor.EditorApplication.isPlaying = false;
#else
            Application.Quit();
#endif
        }
    }
}
