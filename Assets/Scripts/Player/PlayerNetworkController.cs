using Mirror;
using UnityEngine;

namespace ZombieRescue.Player
{
    /// <summary>
    /// Moves and rotates the player character.
    /// On the local player the input adapter drives movement directly;
    /// for remote players the server-authoritative position is applied via sync.
    /// </summary>
    public class PlayerNetworkController : NetworkBehaviour
    {
        [Header("Movement")]
        [SerializeField] private float moveSpeed = 5f;
        [SerializeField] private float rotationSpeed = 720f;

        [Header("References")]
        [SerializeField] private InputAdapter inputAdapter;
        [SerializeField] private CharacterController characterController;

        private const float Gravity = -9.81f;
        private float _verticalVelocity;

        // ── Lifecycle ──────────────────────────────────────────────────────────
        private void Update()
        {
            if (!isLocalPlayer) return;

            Vector2 input = inputAdapter != null ? inputAdapter.MovementInput : Vector2.zero;
            HandleMovement(input);
            HandleRotation(input);
        }

        // ── Movement ───────────────────────────────────────────────────────────
        private void HandleMovement(Vector2 input)
        {
            Vector3 move = new Vector3(input.x, 0f, input.y).normalized;

            // Apply gravity
            if (characterController != null && characterController.isGrounded)
                _verticalVelocity = -0.5f; // keep grounded
            else
                _verticalVelocity += Gravity * Time.deltaTime;

            move.y = _verticalVelocity;

            if (characterController != null)
                characterController.Move(move * moveSpeed * Time.deltaTime);
            else
                transform.Translate(move * moveSpeed * Time.deltaTime, Space.World);

            // Sync position to server
            if (move.sqrMagnitude > 0f)
                CmdMove(transform.position, transform.rotation);
        }

        private void HandleRotation(Vector2 input)
        {
            Vector3 direction = new Vector3(input.x, 0f, input.y);
            if (direction.sqrMagnitude < 0.01f) return;

            Quaternion targetRot = Quaternion.LookRotation(direction, Vector3.up);
            transform.rotation = Quaternion.RotateTowards(
                transform.rotation, targetRot, rotationSpeed * Time.deltaTime);
        }

        // ── Server authority ───────────────────────────────────────────────────
        [Command]
        private void CmdMove(Vector3 position, Quaternion rotation)
        {
            // Basic server-side position validation could go here.
            RpcApplyTransform(position, rotation);
        }

        [ClientRpc]
        private void RpcApplyTransform(Vector3 position, Quaternion rotation)
        {
            if (isLocalPlayer) return; // local player moves itself
            transform.SetPositionAndRotation(position, rotation);
        }
    }
}
