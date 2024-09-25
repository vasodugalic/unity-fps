using System.Collections;
using UnityEngine;


public class PlayerStats : MonoBehaviour
{
    [SerializeField] float maxHealth;
    float currentHealth;

    public HealthBar healthBar;
    public GameObject hitImage;
    private AudioSource healingSound;

    void Start()
    {
        currentHealth = maxHealth;
        healthBar.SetSliderMax(maxHealth);
        healingSound = GetComponent<AudioSource>();
    }

    public void TakeDamage(float amount)
    {
        currentHealth -= amount;
        healthBar.SetSlider(currentHealth);
        hitImage.SetActive(true);

        IEnumerator DeactivateHitImage()
        {
            yield return new WaitForSeconds(0.1f);
            hitImage.SetActive(false);
        }
        StartCoroutine(DeactivateHitImage());
    }

    public void Heal(float amount)
    {
        currentHealth += amount;
        if(currentHealth > maxHealth)
            currentHealth = maxHealth;
        healthBar.SetSlider(currentHealth);
        PlayHealingSound();
    }

    void Update()
    {
        if(currentHealth <= 0)
            Die();
    }

    public float Health => currentHealth;

    void Die()
    {
        Global.isPlayerDead = true;
        Global global = GameObject.FindWithTag("Global").GetComponent<Global>();
        global.gameScreen.SetActive(false);
        global.endScreen.SetActive(true);
        UnityEngine.Cursor.lockState = CursorLockMode.None;
        Time.timeScale = 0f;
    }

    private void PlayHealingSound()
    {
        healingSound.Play();
    }
}
