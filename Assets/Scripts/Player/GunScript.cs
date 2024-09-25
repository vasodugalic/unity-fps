
using Unity.VisualScripting;
using UnityEngine;

public class GunScript : MonoBehaviour
{
    public float damage = 10f;
    public float maxRange = 100f;
    public float shootCooldown = 2f;
    bool alreadyShot;

    public Camera cam;
    public ParticleSystem flash;
    AudioSource shootingSound;
    Global global;

    void Awake()
    {
        shootingSound = GetComponent<AudioSource>();
        global = GameObject.FindWithTag("Global").GetComponent<Global>();
    }

    void Update()
    {
        if (Global.isPaused)
            return;

        if (Input.GetButton("Fire1") && !alreadyShot)
        {
            shootingSound.Play();
            flash.Play();
            Shoot();
            alreadyShot = true;
            Invoke(nameof(ResetCooldown), shootCooldown);
        }
        
    }

    void ResetCooldown()
    {
        alreadyShot = false;
    }

    void Shoot()
    {
        RaycastHit hit;
        if (Physics.Raycast(cam.transform.position, cam.transform.forward, out hit, maxRange))
        {
            if (LayerMask.LayerToName(hit.transform.gameObject.layer).Equals("Enemy"))
            {
                Enemy enemy = hit.transform.GetComponent<Enemy>();
                if (enemy.isAlive)
                    enemy.TakeDamage(damage);
            }
        }
    }
}
