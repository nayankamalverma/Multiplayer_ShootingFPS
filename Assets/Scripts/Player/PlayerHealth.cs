using System.Collections;
using Photon.Pun;
using TMPro;
using UnityEngine;

namespace MultiFPS_Shooting.Scripts.Player
{
    public class PlayerHealth : MonoBehaviour
    {
        [SerializeField] private int initHealth = 100;
        [Header("UI")]
        [SerializeField] private GameObject playerGamePlayUI; 
        [SerializeField] private TextMeshProUGUI healthText;

        private float health;
        public bool isLocal=false;

        private void Start()
        {
            health = initHealth;
            if(isLocal)playerGamePlayUI.SetActive(true);
            UpdateText();
        }

        [PunRPC]
        public void TakeDamage(float damage)
        {
            Debug.Log("Player " + gameObject.name + " damaged " + damage);
            health -= damage;
            UpdateText();
            if (health <= 0)
            {
                if(isLocal)RoomManager.instance.Respawn();
                Destroy(gameObject);
            }
        }

        private void UpdateText()
        {
            healthText.text = "Health: " + health;
        }
        
    }
}