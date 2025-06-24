using System;
using MultiFPS_Shooting.Assets.Scripts.Player.Utilities;
using MultiFPS_Shooting.Input.Input;
using Photon.Pun;
using UnityEngine;
using UnityEngine.InputSystem;

namespace MultiFPS_Shooting.Assets.Scripts.Player
{
    public class PlayerView : MonoBehaviour
    {
        [SerializeField] private InputManager inputManager;
        [SerializeField] private PlayerBaseSO playerBase;
        [SerializeField] private CharacterController charController;
        [SerializeField] private Transform cameraTransform;

        private float playerSpeed;
        private Vector3 movement;
        private bool isWalking;
        private bool isGrounded;
        private float verticalVelocity;
        private bool isJumping;

        private PhotonView photonView;

        private void Start()
        {
            photonView = GetComponent<PhotonView>();
            if (photonView.IsMine)
            {
                cameraTransform.gameObject.SetActive(true);
            }
            else
            {
                cameraTransform.gameObject.SetActive(false);
            }
        }

        private void OnEnable()
        {
            LockCursor();
        }

        private void LockCursor()
        {
            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false;
        }


        private void Update()
        {
            if (!photonView.IsMine)
                return;

            GroundCheck();
            Jump();
            ApplyGravity();
            HandelPlayerMovement();
            HandlePlayerLook();
        }

        private void GroundCheck()
        {
            isGrounded = Physics.CheckSphere(new Vector3(transform.position.x, transform.position.y - playerBase.groundedOffset, transform.position.z), playerBase.groundedRadius, playerBase.groundLayer,
                QueryTriggerInteraction.Ignore);
        }

        private void HandelPlayerMovement()
        {
            isWalking = inputManager.IsShiftHeld();
            playerSpeed = isWalking ? playerBase.walkSpeed : playerBase.sprintSpeed;

            Vector2 inputVector = inputManager.GetPlayerInput();
            movement = (cameraTransform.right * (inputVector.x * playerSpeed)) +
                        (cameraTransform.forward * (inputVector.y * playerSpeed));
            movement.y = verticalVelocity;
            charController.Move(movement * Time.deltaTime);
        }

        private void Jump()
        {
            isJumping = inputManager.IsJumpPressed();
            if (isJumping && isGrounded)
            {
                isJumping = false;
                // the square root of H * -2 * G = how much velocity needed to reach desired height
                verticalVelocity = Mathf.Sqrt(playerBase.jumpHeight * -2f * playerBase.gravity);
            }
        }
        private void HandlePlayerLook()
        {
            Vector2 mouseDelta = inputManager.GetMouseDelta();
            mouseDelta *= playerBase.mouseSensitivity * Time.deltaTime;

            // Handle horizontal rotation (yaw) on player
            float newPlayerYaw = transform.eulerAngles.y + mouseDelta.x;
            transform.rotation = Quaternion.Euler(0f, newPlayerYaw, 0f);

            // Handle vertical rotation (pitch) on camera
            float newCameraPitch = cameraTransform.eulerAngles.x - mouseDelta.y;
            newCameraPitch = Helper.ClampAngle(newCameraPitch, playerBase.cameraClampAngleMin, playerBase.cameraClampAngleMax);
            cameraTransform.rotation = Quaternion.Euler(newCameraPitch, newPlayerYaw, 0f);
        }
        
        private void ApplyGravity()
        {
            verticalVelocity += playerBase.gravity * Time.deltaTime;
            if (isGrounded && verticalVelocity < 0.0f)
            {
                verticalVelocity = -2f;
            }
        }
        private void OnDrawGizmosSelected()
        {
            Color transparentGreen = new Color(0.0f, 1.0f, 0.0f, 0.35f);
            Color transparentRed = new Color(1.0f, 0.0f, 0.0f, 0.35f);

            Gizmos.color = isGrounded ? transparentGreen : transparentRed;
            Gizmos.DrawSphere(new Vector3(transform.position.x, transform.position.y - playerBase.groundedOffset, transform.position.z), playerBase.groundedRadius);
        }

        private void OnDisable()
        {
            UnlockCursor();
        }

        private void UnlockCursor()
        {
            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;
        }
        private void OnDestroy()
        {
            cameraTransform.gameObject.SetActive(false);
        }
    }
}
