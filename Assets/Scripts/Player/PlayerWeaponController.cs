using MultiFPS_Shooting.Input.Input;
using Photon.Pun;
using UnityEngine;

namespace MultiFPS_Shooting.Scripts.Player
{
    public class PlayerWeaponController : MonoBehaviour
    {
        [SerializeField] private float damage = 20;
        [SerializeField] private float fireRate = 10;
        [Header("VFX")] [SerializeField] private GameObject hitVFX;

        [Header("Game Components")] [SerializeField]
        private Camera playerCamera;

        [SerializeField] private InputManager inputManager;
        private float nextFire;

        private void Update()
        {
            if (nextFire > 0) nextFire -= Time.deltaTime;
            if (inputManager.IsShootPressed() && nextFire <= 0)
            {
                nextFire = 1 / fireRate;
                Fire();
            }
        }

        private void Fire()
        {
            Ray ray = new Ray(playerCamera.transform.position, playerCamera.transform.forward);

            RaycastHit hit;
            if (Physics.Raycast(ray.origin, ray.direction, out hit, 100f))
            {
                PhotonNetwork.Instantiate(hitVFX.name, hit.point, Quaternion.identity);
                if (hit.transform.gameObject.GetComponent<PlayerHealth>())
                {
                    hit.transform.gameObject.GetComponent<PhotonView>().RPC("TakeDamage", RpcTarget.All, damage);
                }
            }
        }
    }
}