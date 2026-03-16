using UnityEngine;

namespace ZombieRescue.Player
{
    /// <summary>
    /// Abstract base that decouples input reading from player logic.
    /// Concrete adapters target desktop, mobile, or other platforms.
    /// </summary>
    public abstract class InputAdapter : MonoBehaviour
    {
        /// <summary>Normalised movement direction (x = strafe, y = forward).</summary>
        public abstract Vector2 MovementInput { get; }

        /// <summary>Look/aim direction in screen or world space.</summary>
        public abstract Vector2 LookInput { get; }

        /// <summary>True on the frame the fire button is pressed.</summary>
        public abstract bool FirePressed { get; }

        /// <summary>True while the fire button is held (for auto weapons).</summary>
        public abstract bool FireHeld { get; }

        /// <summary>True on the frame the reload key is pressed.</summary>
        public abstract bool ReloadPressed { get; }

        /// <summary>True on the frame the weapon-switch key is pressed.</summary>
        public abstract bool WeaponSwitchPressed { get; }
    }
}
