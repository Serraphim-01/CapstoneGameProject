using UnityEngine;
using UnityEngine.UI;

public class EnemyHealthBar : MonoBehaviour
{
    public EnemyHealth enemyHealth;
    public Slider healthSlider;

    private Transform player; // player to face
    private Camera worldCamera; // assigned at runtime

    void Start()
    {
        if (enemyHealth != null)
        {
            healthSlider.maxValue = enemyHealth.maxHealth;
            healthSlider.value = enemyHealth.maxHealth;
        }

        player = GameObject.FindGameObjectWithTag("Player")?.transform;

        // Automatically assign the world space camera
        worldCamera = Camera.main;

        var canvas = GetComponent<Canvas>();
        if (canvas != null && canvas.renderMode == RenderMode.WorldSpace)
        {
            canvas.worldCamera = worldCamera;
        }
    }

    void Update()
    {
        if (enemyHealth == null || player == null) return;

        healthSlider.value = enemyHealth.CurrentHealth;

        // Make the health bar face the player
        Vector3 dirToPlayer = (player.position - transform.position).normalized;
        transform.forward = dirToPlayer;
    }
}