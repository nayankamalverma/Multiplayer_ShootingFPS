using UnityEngine;

namespace MultiFPS_Shooting.Input.Input
{
    public class InputManager : MonoBehaviour
    {
        private PlayerInputAction playerInputAction;
        private void Awake()
        {
            playerInputAction = new PlayerInputAction();
        }

        private void OnEnable()
        {
            playerInputAction.Enable();
        }

        private void OnDisable()
        {
            playerInputAction.Disable();
        }

        public Vector2 GetPlayerInput()
        {
            return playerInputAction.Player.Movement.ReadValue<Vector2>();
        }

        public Vector2 GetMouseDelta()
        {
            return playerInputAction.Player.Look.ReadValue<Vector2>();
        }

        public bool IsJumpPressed()
        {
            return playerInputAction.Player.Jump.triggered;
        }

        public bool IsReloadPressed()
        {
            return playerInputAction.Player.Reload.triggered;
        }

        public bool IsShiftHeld()
        {
            return playerInputAction.Player.Walk.IsPressed();
        }

        public bool IsShootPressed()
        {
            return playerInputAction.Player.Shoot.triggered;
        }
    }
}