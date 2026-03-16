using System.Text;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using ZombieRescue.Scoring;

namespace ZombieRescue.UI
{
    /// <summary>
    /// Displays the end-of-match results screen.
    /// </summary>
    public class MatchResultsUI : MonoBehaviour
    {
        [Header("UI Elements")]
        [SerializeField] private Text   resultHeaderText;
        [SerializeField] private Text   summaryText;
        [SerializeField] private Button returnToMenuButton;

        [Header("Scene")]
        [SerializeField] private string mainMenuScene = "MainMenu";

        // ── Lifecycle ──────────────────────────────────────────────────────────
        private void Start()
        {
            returnToMenuButton?.onClick.AddListener(OnReturnToMenuClicked);
        }

        // ── Public API ─────────────────────────────────────────────────────────
        public void ShowResults(MatchResults results)
        {
            if (resultHeaderText != null)
                resultHeaderText.text = results.isVictory ? "VICTORY!" : "DEFEAT";

            if (summaryText != null)
                summaryText.text = BuildSummaryText(results);

            gameObject.SetActive(true);
        }

        // ── Helpers ────────────────────────────────────────────────────────────
        private string BuildSummaryText(MatchResults results)
        {
            var sb = new StringBuilder();
            sb.AppendLine($"Duration : {results.matchDuration:F1}s");
            sb.AppendLine($"Zombies  : {results.totalZombiesKilled}");
            sb.AppendLine($"Rescued  : {results.totalSurvivorsRescued}");
            sb.AppendLine($"Lost     : {results.totalSurvivorsLost}");
            sb.AppendLine();

            for (int i = 0; i < results.playerResults.Count; i++)
            {
                var p = results.playerResults[i];
                sb.AppendLine($"── {p.playerName} ──");
                sb.AppendLine($"  Kills    : {p.zombiesKilled}");
                sb.AppendLine($"  Rescued  : {p.survivorsRescued}");
                sb.AppendLine($"  Accuracy : {p.accuracy * 100f:F1}%");
                sb.AppendLine($"  Score    : {p.score}");
            }

            if (results.winnerIndex >= 0 && results.playerResults.Count > 1)
                sb.AppendLine($"\nMVP: {results.playerResults[results.winnerIndex].playerName}");

            return sb.ToString();
        }

        private void OnReturnToMenuClicked()
        {
            SceneManager.LoadScene(mainMenuScene);
        }
    }
}
