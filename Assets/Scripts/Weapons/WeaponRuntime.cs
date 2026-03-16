using System;

namespace ZombieRescue.Weapons
{
    /// <summary>
    /// Mutable runtime state for one weapon instance carried by a player.
    /// Wraps a WeaponDefinition and tracks ammo, cooldown and reload state.
    /// </summary>
    public class WeaponRuntime
    {
        public WeaponDefinition Definition { get; }
        public int  CurrentAmmo   { get; private set; }
        public bool IsReloading   { get; private set; }

        /// <summary>Seconds remaining before the weapon can fire again. Decremented externally.</summary>
        public float FireCooldown { get; set; }

        /// <summary>Raised when the last round is expended and a reload is needed.</summary>
        public event Action OnReloadNeeded;

        public WeaponRuntime(WeaponDefinition definition)
        {
            Definition  = definition ?? throw new ArgumentNullException(nameof(definition));
            CurrentAmmo = definition.MagazineSize;
        }

        // ── State transitions ──────────────────────────────────────────────────
        public void StartReload()
        {
            IsReloading = true;
        }

        public void FinishReload()
        {
            CurrentAmmo = Definition.MagazineSize;
            IsReloading = false;
        }

        /// <returns>True when the weapon is ready to fire.</returns>
        public bool CanFire() => !IsReloading && FireCooldown <= 0f && CurrentAmmo > 0;

        /// <summary>
        /// Decrements ammo by one. Raises <see cref="OnReloadNeeded"/> when the magazine empties.
        /// </summary>
        public void ConsumeAmmo()
        {
            if (CurrentAmmo <= 0) return;
            CurrentAmmo--;
            if (CurrentAmmo == 0)
                OnReloadNeeded?.Invoke();
        }
    }
}
