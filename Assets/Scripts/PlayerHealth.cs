using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class PlayerHealth : MonoBehaviour
{
    public float maxHealth = 10f;
    [SerializeField] private Image healthBarFill; // Reference to the UI Image for the health bar fill

    private float currentHealth;
    private bool isDead = false;

    void Start()
    {
        currentHealth = maxHealth;
        if (healthBarFill != null)
        {
            healthBarFill.fillAmount = 1f; // Initialize health bar fill to full
        }
    }

    public void TakeDamage(float damage)
    {
        if (isDead) return; // Prevent taking damage if already dead

        currentHealth -= damage;

        healthBarFill.fillAmount = currentHealth / maxHealth; // Update health bar fill
        currentHealth = Mathf.Clamp(currentHealth, 0, maxHealth);

        if (currentHealth <= 0)
        {
            Die();
        }       
    }

    void Die()
    {
        isDead = true;
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
        Debug.Log("Player has died!");
    }
}
