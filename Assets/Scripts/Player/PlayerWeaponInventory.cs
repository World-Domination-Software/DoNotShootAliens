using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using ZombieRescue.Weapons;

namespace ZombieRescue.Player
{
    /// <summary>
    /// Owns and manages the player's weapon loadout.
    /// Weapons are initialised from ScriptableObject definitions at runtime.
    /// </summary>
    public class PlayerWeaponInventory : MonoBehaviour
    {
        [Header("Weapon Definitions")]
        [SerializeField] private WeaponDefinition pistolDefinition;
        [SerializeField] private WeaponDefinition rifleDefinition;

        [Header("Events")]
        public UnityEvent<WeaponRuntime> OnWeaponSwitched = new UnityEvent<WeaponRuntime>();

        private readonly List<WeaponRuntime> _weapons = new List<WeaponRuntime>();
        private int _currentWeaponIndex;

        public WeaponRuntime CurrentWeapon => _weapons.Count > 0 ? _weapons[_currentWeaponIndex] : null;

        // ── Lifecycle ──────────────────────────────────────────────────────────
        private void Start()
        {
            if (pistolDefinition != null) _weapons.Add(new WeaponRuntime(pistolDefinition));
            if (rifleDefinition  != null) _weapons.Add(new WeaponRuntime(rifleDefinition));

            if (_weapons.Count > 0)
                OnWeaponSwitched.Invoke(CurrentWeapon);
        }

        // ── Public API ─────────────────────────────────────────────────────────
        /// <summary>Cycles to the next weapon in the loadout.</summary>
        public void SwitchWeapon()
        {
            if (_weapons.Count == 0) return;
            SwitchToWeapon((_currentWeaponIndex + 1) % _weapons.Count);
        }

        public void SwitchToWeapon(int index)
        {
            if (index < 0 || index >= _weapons.Count) return;
            _currentWeaponIndex = index;
            OnWeaponSwitched.Invoke(CurrentWeapon);
        }
    }
}
