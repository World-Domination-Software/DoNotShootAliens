using UnityEngine;

namespace ZombieRescue.Player
{
    /// <summary>
    /// Desktop input via the legacy Unity Input Manager.
    /// WASD = move, Mouse = look, LMB = fire, R = reload, Q/ScrollWheel = switch weapon.
    /// </summary>
    public class DesktopInputAdapter : InputAdapter
    {
        public override Vector2 MovementInput =>
            new Vector2(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical"));

        public override Vector2 LookInput =>
            new Vector2(Input.GetAxisRaw("Mouse X"), Input.GetAxisRaw("Mouse Y"));

        public override bool FirePressed =>
            Input.GetMouseButtonDown(0);

        public override bool FireHeld =>
            Input.GetMouseButton(0);

        public override bool ReloadPressed =>
            Input.GetKeyDown(KeyCode.R);

        public override bool WeaponSwitchPressed =>
            Input.GetKeyDown(KeyCode.Q) || Input.GetAxisRaw("Mouse ScrollWheel") != 0f;
    }
}
