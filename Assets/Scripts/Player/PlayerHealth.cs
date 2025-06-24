using System;
using Photon.Pun;
using UnityEngine;

public class PlayerHealth : MonoBehaviour
{
    [SerializeField] private int initHealth=100;

    private float health;

    private void Start()
    {
        health = initHealth;
    }

    [PunRPC]
    public void TakeDamage(float damage)
    {
        Debug.Log("Player " + gameObject.name + " damaged " + damage);
        health -= damage;
        if (health <= 0)
        {
            Destroy(gameObject);
        }
    }
}
