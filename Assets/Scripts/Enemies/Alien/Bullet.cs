using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    public float damage = 15f;
    Transform player;

    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player").transform;
    }

    void Update()
    {
        float distanceFromPlayer = (player.position - transform.position).magnitude;
        if (distanceFromPlayer >= 50f)
            Destroy(gameObject);
    }

    private void OnCollisionEnter(Collision collision)
    {
        Transform hit = collision.transform;
        if(hit.CompareTag("Player"))
            hit.transform.GetComponent<PlayerStats>().TakeDamage(damage);
        Destroy(gameObject);
    }
}
