using UnityEngine;
using UnityEngine.SceneManagement; // 씬 재시작을 위해 필요

public class PlayerHealth : MonoBehaviour
{
    [SerializeField] private int maxHealth = 3; // 플레이어의 최대 체력
    private int currentHealth;

    private void Start()
    {
        currentHealth = maxHealth; // 시작 시 체력을 최대 체력으로 초기화
    }

    public void TakeDamage(int damage)
    {
        currentHealth -= damage;
        Debug.Log($"Player Health: {currentHealth}");

        if (currentHealth <= 0)
        {
            Die();
        }
    }

    private void Die()
    {
        Debug.Log("Player Died. Restarting Scene...");
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }
}
