using UnityEngine;
using UnityEngine.UI;

public class PlayerHealthUI : MonoBehaviour
{
    public PlayerHealth playerHealth;
    public Slider healthSlider;

    void Start()
    {
        if (playerHealth != null)
            healthSlider.maxValue = playerHealth.maxHealth;
    }

    void Update()
    {
        if (playerHealth != null && healthSlider != null)
            healthSlider.value = playerHealth.CurrentHealth; // We'll add a getter
    }
}
