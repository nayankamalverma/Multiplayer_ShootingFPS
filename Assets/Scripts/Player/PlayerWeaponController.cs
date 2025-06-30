using System;
using MultiFPS_Shooting.Input.Input;
using Photon.Pun;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;

namespace MultiFPS_Shooting.Scripts.Player
{
    public class PlayerWeaponController : MonoBehaviour
    {
        [SerializeField] private float damage = 20;
        [SerializeField] private float fireRate = 10;
        [Header("Ammo")]
        [SerializeField] private int maxAmmo = 120;
        [SerializeField] private int maxMagAmmo = 30;
        [SerializeField] private int magAmmo = 30;

        [Header("Animation")]
        [SerializeField] private Animation animation;
        [SerializeField] private AnimationClip reloadClip;

        [Header("VFX")]
        [SerializeField] private GameObject hitVFX;

        [Header("UI")]
        [SerializeField] private TextMeshProUGUI ammoText;

        [Header("Game Components")]
        [SerializeField] private Camera playerCamera;
        [SerializeField] private InputManager inputManager;

        private float nextFire;
        private int ammo;

        private void Start()
        {
            ammo = maxAmmo;
            UpdateAmmoText();
        }

        private void Update()
        {
            if (nextFire > 0) nextFire -= Time.deltaTime;
            if (inputManager.IsShootPressed() && nextFire <= 0 && ammo+magAmmo >0 && !animation.isPlaying)
            {
                nextFire = 1 / fireRate;
                magAmmo--;
                Fire();
                UpdateAmmoText();
            }
            if(inputManager.IsReloadPressed() || magAmmo == 0) Reload();
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

        private void Reload()
        {

            if (ammo > 0)
            {
                animation.Play(reloadClip.name);

                int remainingMagAmmo = magAmmo;
                magAmmo = 0; // Empty the magazine first

                int ammoToReload = Math.Min(maxMagAmmo, ammo + remainingMagAmmo);
                magAmmo = ammoToReload;
                ammo = (ammo + remainingMagAmmo) - ammoToReload;
            }
            UpdateAmmoText();
        }

        private void UpdateAmmoText()
        {
            ammoText.text = "Ammo: " + magAmmo + "/" + ammo;
        }
    }
}