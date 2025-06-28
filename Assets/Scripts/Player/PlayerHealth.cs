using System.Collections;
using Photon.Pun;
using TMPro;
using UnityEngine;

namespace MultiFPS_Shooting.Scripts.Player
{
    public class PlayerHealth : MonoBehaviour
    {
        [SerializeField] private int initHealth = 100;
        [Header("UI")] [SerializeField] private TextMeshProUGUI healthText;

        private float health;
        public bool isLocal=false;

        private void Start()
        {
            health = initHealth;
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
                Debug.Log(isLocal);
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