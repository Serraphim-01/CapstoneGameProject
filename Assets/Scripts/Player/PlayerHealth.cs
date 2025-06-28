using UnityEngine;
using UnityEngine.UIElements;
using System.Collections;

public class PlayerHealth : MonoBehaviour
{
    public int maxHealth = 100;
    private int currentHealth;
    public int CurrentHealth => currentHealth;
    private Animator animator;
    private int hurtLayerIndex;
    public bool IsDead { get; private set; }

    public UIDocument gameOverUIDocument;
    private VisualElement gameOverRoot;
    private VisualElement nextLevelButton;
    private Label youWinLabel;

    void Start()
    {
        currentHealth = maxHealth;
        animator = GetComponentInChildren<Animator>();
        hurtLayerIndex = animator.GetLayerIndex("Hurt");

        if (gameOverUIDocument != null)
        {
            gameOverRoot = gameOverUIDocument.rootVisualElement;
            gameOverRoot.style.display = DisplayStyle.None;

            // Cache specific UI elements
            nextLevelButton = gameOverRoot.Q<Button>("NextLevel");
            youWinLabel = gameOverRoot.Q<Label>("YouWin");
        }
    }

    public void TakeDamage(int amount)
    {
        if (IsDead) return;

        currentHealth -= amount;
        Debug.Log($"Player's current health: {currentHealth}");
        animator.SetLayerWeight(hurtLayerIndex, 1f);
        StartCoroutine(PlayHurtFeedback());

        if (currentHealth <= 0)
        {
            StartCoroutine(DieSequence());
        }
    }

    IEnumerator PlayHurtFeedback()
    {
        yield return new WaitForSeconds(0.4f);
        animator.SetLayerWeight(hurtLayerIndex, 0f);
    }

    IEnumerator DieSequence()
{
    if (IsDead) yield break;
    IsDead = true;

    Time.timeScale = 0.0001f;
    Time.fixedDeltaTime = 0.02f * Time.timeScale;

    animator.SetTrigger("Die");

    yield return new WaitForSecondsRealtime(1.5f); // Wait for animation

    if (gameOverRoot != null)
    {
        gameOverRoot.style.display = DisplayStyle.Flex;

        // ✅ Show GameOver text + Restart button
        var gameOverText = gameOverRoot.Q<Label>("GameOver");
        var restartButton = gameOverRoot.Q<Button>("Restart");

        if (gameOverText != null)
            gameOverText.style.display = DisplayStyle.Flex;

        if (restartButton != null)
            restartButton.style.display = DisplayStyle.Flex;

        // ❌ Hide NextLevel and YouWin if they exist
        if (nextLevelButton != null)
            nextLevelButton.style.display = DisplayStyle.None;

        if (youWinLabel != null)
            youWinLabel.style.display = DisplayStyle.None;
    }

    Time.timeScale = 0f;

    // Disable controls
    var controller = GetComponent<PlayerController>();
    if (controller) controller.enabled = false;

    var shooter = GetComponent<PlayerShoot>();
    if (shooter) shooter.enabled = false;
}

}
