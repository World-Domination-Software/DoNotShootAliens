using UnityEngine;
using UnityEngine.UI;
using ZombieRescue.Player;
using ZombieRescue.Scoring;

namespace ZombieRescue.UI
{
    /// <summary>
    /// Updates the in-game HUD each frame from live game data.
    /// </summary>
    public class HUDController : MonoBehaviour
    {
        [Header("Data Sources")]
        [SerializeField] private PlayerHealth          playerHealth;
        [SerializeField] private PlayerWeaponInventory weaponInventory;
        [SerializeField] private ScoreManager          scoreManager;

        [Header("UI Elements")]
        [SerializeField] private Text healthText;
        [SerializeField] private Text weaponNameText;
        [SerializeField] private Text ammoText;
        [SerializeField] private Text survivorCountText;
        [SerializeField] private Text objectiveText;

        // ── Lifecycle ──────────────────────────────────────────────────────────
        private void Update()
        {
            UpdateHealthDisplay();
            UpdateWeaponDisplay();
            UpdateScoreDisplay();
        }

        // ── Display helpers ────────────────────────────────────────────────────
        private void UpdateHealthDisplay()
        {
            if (healthText == null || playerHealth == null) return;
            healthText.text = $"HP: {Mathf.CeilToInt(playerHealth.CurrentHealth)} / {Mathf.CeilToInt(playerHealth.MaxHealth)}";
        }

        private void UpdateWeaponDisplay()
        {
            if (weaponInventory == null) return;

            var weapon = weaponInventory.CurrentWeapon;
            if (weaponNameText != null)
                weaponNameText.text = weapon != null ? weapon.Definition.WeaponName : "—";

            if (ammoText != null)
                ammoText.text = weapon != null
                    ? (weapon.IsReloading ? "Reloading…" : $"{weapon.CurrentAmmo} / {weapon.Definition.MagazineSize}")
                    : "—";
        }

        private void UpdateScoreDisplay()
        {
            if (survivorCountText != null && scoreManager != null)
                survivorCountText.text = $"Score: {scoreManager.TotalScore}";
        }

        // ── Public API ─────────────────────────────────────────────────────────
        public void SetObjectiveText(string text)
        {
            if (objectiveText != null)
                objectiveText.text = text;
        }
    }
}
