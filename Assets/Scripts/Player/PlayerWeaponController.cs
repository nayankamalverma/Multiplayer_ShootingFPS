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

        [Header("Recoil Settings")]
        //[Range(0,1)]
        //[SerializeField] private float recoilPercent = 0.3f;
        [Range(0,2)]
        [SerializeField] private float recoverPercent = 0.7f;
        [Space] 
        [SerializeField] private float recoilUp = 1f;
        [SerializeField] private float recoilBack = 1f;

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
        private int magAmmo;
        //recoil
        private Vector3 originalPos;
        private Vector3 recoilVelocity = Vector3.zero;


        private float recoilLength;
        private float recoverLength;

        private bool recoiling;
        private bool recovering;


        private void Start()
        {
            ammo = maxAmmo;
            magAmmo = maxMagAmmo;
            UpdateAmmoText();
            originalPos = transform.localPosition;

            recoilLength = 0;
            recoverLength = 1 / fireRate * recoverPercent;
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
            if(ammo >0 &&  (inputManager.IsReloadPressed() && magAmmo<maxMagAmmo)|| magAmmo == 0) Reload();
            //recoil
            if(recoiling) Recoil();
            if(recovering) Recover();
        }

        private void Fire()
        {
            recoiling = true;
            recovering = false;

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

        private void Recoil()
        {
            Vector3 finalPos = new Vector3(originalPos.x, originalPos.y + recoilUp, originalPos.z - recoilBack);
            transform.localPosition = Vector3.SmoothDamp(transform.localPosition, finalPos, ref recoilVelocity, recoilLength);

            if (transform.localPosition == finalPos)
            {
                recoiling = false;
                recovering = true;
            }

        }
        private void Recover()
        {
            Vector3 finalPos = originalPos;
            transform.localPosition = Vector3.SmoothDamp(transform.localPosition, finalPos, ref recoilVelocity, recoverLength);

            if (transform.localPosition == finalPos)
            {
                recoiling = false;
                recovering = false;
            }

        }
    }
}