using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FirstAid : MonoBehaviour
{
    public float healAmount = 5f;
    private PlayerStats playerStats;
    private AudioSource healingSound;

    private void Start()
    {
        healingSound = GetComponent<AudioSource>();
        playerStats = GameObject.FindWithTag("Player").GetComponent<PlayerStats>();
    }

    void OnTriggerEnter(Collider other)
    {
        Transform hit = other.transform;
        if(hit.CompareTag("Player") && playerStats.Health < 100.0f)
        {
            playerStats.Heal(healAmount);
            Destroy(gameObject);
        }
    }
}
