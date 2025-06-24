using MultiFPS_Shooting.Input.Input;
using UnityEngine;

namespace MultiFPS_Shooting.Scripts.Utilities.Weapon
{
    public class WeaponSway : MonoBehaviour
    {
        [Header("Settings")] 
        [SerializeField] private float swayClamp = 0.09f;
        [SerializeField] private float smoothTime = 3f;
        [Space]
        [SerializeField] private InputManager inputManager;

        private Vector3 originalPosition;

        private void Start()
        {
            originalPosition = transform.localPosition;
        }

        private void Update()
        {
            Vector2 input = inputManager.GetMouseDelta();
            
            input.x = Mathf.Clamp(input.x, -swayClamp, swayClamp);
            input.y = Mathf.Clamp(input.y, -swayClamp, swayClamp);
            
            Vector3 target = new Vector3(input.x, input.y, 0f);
            
            transform.localPosition = Vector3.Lerp(transform.localPosition, target + originalPosition, Time.deltaTime * smoothTime);
        }
    }
}
