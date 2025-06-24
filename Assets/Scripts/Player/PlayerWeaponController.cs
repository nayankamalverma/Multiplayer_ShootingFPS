using MultiFPS_Shooting.Input.Input;
using Photon.Pun;
using UnityEngine;

public class PlayerWeaponController : MonoBehaviour
{
    [SerializeField] private float damage=20;
    [SerializeField] private float fireRate=10;
    

    [SerializeField] private Camera playerCamera;
    [SerializeField] private InputManager inputManager;
    private float nextFire;
    private void Update()
    {
        if (nextFire > 0) nextFire -= Time.deltaTime;
        if (inputManager.IsShootPressed() && nextFire <= 0)
        {
            Debug.Log("ifre1");
            nextFire = 1/fireRate;
            Fire();
        }
    }

    private void Fire()
    {
        
        Ray ray = new Ray(playerCamera.transform.position, playerCamera.transform.forward);
        
        RaycastHit hit;
        if (Physics.Raycast(ray.origin, ray.direction, out hit, 100f))
        { Debug.Log("Fire 2");
            if (hit.transform.gameObject.GetComponent<PlayerHealth>())
            {Debug.Log("Fire 3");
                hit.transform.gameObject.GetComponent<PhotonView>().RPC("TakeDamage",RpcTarget.All, damage);
            }
        }
    }
}
