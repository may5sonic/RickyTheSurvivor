using UnityEngine;
using System.Collections;

public class PlayerHealth : MonoBehaviour
{
    public int maxHealth = 100;
    public bool destroyOnDeath;
    public float destroyDelaySeconds = 3f;
    public float gameOverDelaySeconds = 3.7f;

    [SerializeField]
    private int currentHealth;

    public int CurrentHealth => currentHealth;
    public float HealthPercent => maxHealth > 0 ? (float)currentHealth / maxHealth : 0f;

    private bool isDead;

    void Awake()
    {
        currentHealth = maxHealth;
    }

    public void TakeDamage(int amount)
    {
        if (isDead) return;
        if (amount <= 0) return;

        currentHealth -= amount;
        if (currentHealth <= 0)
        {
            currentHealth = 0;
            Die();
        }
    }

    void Die()
    {
        if (isDead) return;
        isDead = true;

        Animator animator = GetComponent<Animator>();
        if (animator != null)
        {
            animator.SetBool("Dead", true);
        }

        PlayerMovement playerMovement = GetComponent<PlayerMovement>();
        if (playerMovement != null) playerMovement.enabled = false;

        PlayerShoot playerShoot = GetComponent<PlayerShoot>();
        if (playerShoot != null) playerShoot.enabled = false;

        StartCoroutine(ShowGameOverAfterDelay());

        if (destroyOnDeath && destroyDelaySeconds > 0f)
        {
            Destroy(gameObject, destroyDelaySeconds);
        }
    }

    IEnumerator ShowGameOverAfterDelay()
    {
        if (gameOverDelaySeconds > 0f)
        {
            yield return new WaitForSeconds(gameOverDelaySeconds);
        }

        GameOverUI[] gameOverUis = Object.FindObjectsByType<GameOverUI>(FindObjectsInactive.Include, FindObjectsSortMode.None);
        if (gameOverUis == null || gameOverUis.Length == 0) yield break;

        gameOverUis[0].Show();
    }
}
