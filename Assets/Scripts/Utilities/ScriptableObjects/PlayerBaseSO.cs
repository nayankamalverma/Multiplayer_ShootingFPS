using UnityEngine;

[CreateAssetMenu(fileName = "PlayerBaseSO", menuName = "Scriptable Objects/PlayerBaseSO")]
public class PlayerBaseSO : ScriptableObject
{
    
    [Header("Movement")]
    public float walkSpeed = 5f;
    public float sprintSpeed = 10f;
    [Header("Look")]
    public float mouseSensitivity =2f;
    public float cameraClampAngleMin = -75f;
    public float cameraClampAngleMax = 75f;
    [Header("Gravity")]
    public float gravity = -15f;
    public float jumpHeight = 5f;
    [Header("Ground Check")]
    public float groundedOffset = 0f;
    public float groundedRadius = .28f;
    public LayerMask groundLayer;
}
