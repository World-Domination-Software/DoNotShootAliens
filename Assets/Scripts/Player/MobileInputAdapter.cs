using UnityEngine;

namespace ZombieRescue.Player
{
    /// <summary>
    /// Mobile input adapter – placeholder returning defaults.
    /// TODO: wire up on-screen joystick and touch buttons in Phase 2.
    /// </summary>
    public class MobileInputAdapter : InputAdapter
    {
        // TODO: connect to an on-screen joystick component.
        public override Vector2 MovementInput => Vector2.zero;

        // TODO: connect to a look/aim touch area.
        public override Vector2 LookInput => Vector2.zero;

        // TODO: connect to a fire button UI element.
        public override bool FirePressed => false;

        // TODO: connect to a fire button hold state.
        public override bool FireHeld => false;

        // TODO: connect to a reload button UI element.
        public override bool ReloadPressed => false;

        // TODO: connect to a weapon-switch button UI element.
        public override bool WeaponSwitchPressed => false;
    }
}
