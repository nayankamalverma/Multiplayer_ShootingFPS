using UnityEngine;

namespace MultiFPS_Shooting.Input.Input
{
    public class InputManager : MonoBehaviour
    {
        private static InputManager instance;
        public static InputManager Instance
        {
            get { return instance; }
        }

        private PlayerInputAction playerInputAction;
        
        private void Awake()
        {
            if (instance == null)
            {
                instance = this;
            }
            else
            {
                Destroy(instance.gameObject);
            }

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