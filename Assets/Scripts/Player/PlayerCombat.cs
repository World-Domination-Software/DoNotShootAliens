using UnityEngine;
using ZombieRescue.Core;

namespace ZombieRescue.Player
{
    /// <summary>
    /// Reads fire/reload input, performs raycast hit detection, and applies damage.
    /// Tracks combat statistics via PlayerRoundStats.
    /// </summary>
    public class PlayerCombat : MonoBehaviour
    {
        [Header("References")]
        [SerializeField] private PlayerWeaponInventory weaponInventory;
        [SerializeField] private InputAdapter inputAdapter;
        [SerializeField] private Transform firePoint;
        [SerializeField] private PlayerRoundStats stats;

        [Header("Targeting")]
        [SerializeField] private LayerMask targetLayers;
        [Tooltip("Maximum raycast distance, overridden by weapon range if shorter.")]
        [SerializeField] private float defaultRange = 50f;

        private float _reloadTimer;

        // ── Lifecycle ──────────────────────────────────────────────────────────
        private void Start()
        {
            // Auto-reload when the magazine empties via ConsumeAmmo.
            if (weaponInventory != null)
                weaponInventory.OnWeaponSwitched.AddListener(weapon =>
                    weapon.OnReloadNeeded += () => StartReload(weapon));
        }

        private void Update()
        {
            if (inputAdapter == null || weaponInventory == null) return;

            HandleReload();
            HandleFire();
            HandleWeaponSwitch();

            // Tick weapon fire cooldown
            var weapon = weaponInventory.CurrentWeapon;
            if (weapon != null && weapon.FireCooldown > 0f)
                weapon.FireCooldown -= Time.deltaTime;

            // Tick reload
            if (weapon != null && weapon.IsReloading)
            {
                _reloadTimer -= Time.deltaTime;
                if (_reloadTimer <= 0f)
                    weapon.FinishReload();
            }
        }

        // ── Private helpers ────────────────────────────────────────────────────
        private void HandleFire()
        {
            var weapon = weaponInventory.CurrentWeapon;
            if (weapon == null || !weapon.CanFire()) return;

            bool shouldFire = weapon.Definition.IsAutomatic
                ? inputAdapter.FireHeld
                : inputAdapter.FirePressed;

            if (!shouldFire) return;

            stats?.RecordShot();
            weapon.FireCooldown = 1f / weapon.Definition.FireRate;

            // Raycast from fire point forward
            Vector3 origin    = firePoint != null ? firePoint.position : transform.position;
            Vector3 direction = firePoint != null ? firePoint.forward  : transform.forward;
            float   range     = Mathf.Min(defaultRange, weapon.Definition.Range);

            if (Physics.Raycast(origin, direction, out RaycastHit hit, range, targetLayers))
            {
                stats?.RecordHit();
                var damageable = hit.collider.GetComponentInParent<IDamageable>();
                damageable?.TakeDamage(weapon.Definition.Damage);
            }

            // ConsumeAmmo raises OnReloadNeeded when empty; subscribe in Start to handle auto-reload.
            weapon.ConsumeAmmo();
        }

        private void HandleReload()
        {
            var weapon = weaponInventory.CurrentWeapon;
            if (weapon == null || weapon.IsReloading) return;

            if (inputAdapter.ReloadPressed)
                StartReload(weapon);
        }

        private void StartReload(Weapons.WeaponRuntime weapon)
        {
            weapon.StartReload();
            _reloadTimer = weapon.Definition.ReloadDuration;
        }

        private void HandleWeaponSwitch()
        {
            if (inputAdapter.WeaponSwitchPressed)
                weaponInventory.SwitchWeapon();
        }
    }
}
