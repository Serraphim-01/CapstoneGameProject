using UnityEngine;
using UnityEngine.UIElements;
using UnityEngine.SceneManagement;

public class LobbyMenu : MonoBehaviour
{
    [Header("UIDocuments")]
    public UIDocument startMenuUIDocument;
    public UIDocument instructionsUIDocument;
    public UIDocument shopUIDocument;

    void Start()
    {
        Debug.Log("LobbyMenu Start Called");

        if (startMenuUIDocument == null)
        {
            Debug.LogError("Start Menu UIDocument is NOT assigned!");
            return;
        }

        var startRoot = startMenuUIDocument.rootVisualElement;
        if (startRoot == null)
        {
            Debug.LogError("startMenuUIDocument.rootVisualElement is NULL!");
            return;
        }
        else
        {
            Debug.Log("✅ Start Menu Root VisualElement found.");
        }

        var startButton = startRoot.Q<Button>("StartButton");
        var quitButton = startRoot.Q<Button>("Quit");
        var instructionsButton = startRoot.Q<Button>("Instructions");
        var shopButton = startRoot.Q<Button>("ShopButton");

        // Log each button presence
        Debug.Log(startButton != null ? "✅ Start Button found." : "❌ Start Button NOT found!");
        Debug.Log(quitButton != null ? "✅ Quit Button found." : "❌ Quit Button NOT found!");
        Debug.Log(instructionsButton != null ? "✅ Instructions Button found." : "❌ Instructions Button NOT found!");
        Debug.Log(shopButton != null ? "✅ Shop Button found." : "❌ Shop Button NOT found!");

        // Start Button
        if (startButton != null)
        {
            startButton.clicked += () =>
            {
                UIAudioManager.Instance?.PlayClickSound();
                Debug.Log("▶️ Start Button CLICKED! Loading next scene...");
                SceneManager.LoadScene("Level1");
            };
        }

        // Quit Button
        if (quitButton != null)
        {
            quitButton.clicked += () =>
            {
                UIAudioManager.Instance?.PlayCloseSound();
                Debug.Log("🛑 Quit Button CLICKED. Quitting...");
#if UNITY_EDITOR
                UnityEditor.EditorApplication.isPlaying = false;
#else
                Application.Quit();
#endif
            };
        }

        // Instructions Panel
        if (instructionsButton != null)
        {
            instructionsButton.clicked += () =>
            {
                UIAudioManager.Instance?.PlayClickSound();
                Debug.Log("📘 Instructions Button CLICKED");
                if (instructionsUIDocument != null)
                    instructionsUIDocument.rootVisualElement.style.display = DisplayStyle.Flex;
            };
        }

        // Shop Panel
        if (shopButton != null)
        {
            shopButton.clicked += () =>
            {
                UIAudioManager.Instance?.PlayClickSound();
                Debug.Log("🛒 Shop Button CLICKED");
                if (shopUIDocument != null)
                    shopUIDocument.rootVisualElement.style.display = DisplayStyle.Flex;
            };
        }

        // Hide panels initially
        if (instructionsUIDocument != null)
        {
            instructionsUIDocument.rootVisualElement.style.display = DisplayStyle.None;
            Debug.Log("🔒 Instructions Panel hidden on start.");
        }

        if (shopUIDocument != null)
        {
            shopUIDocument.rootVisualElement.style.display = DisplayStyle.None;
            Debug.Log("🔒 Shop Panel hidden on start.");
        }

        // Close buttons
        if (instructionsUIDocument != null)
        {
            var instructionRoot = instructionsUIDocument.rootVisualElement;
            var closeInstructionsBtn = instructionRoot.Q<Button>("CloseButton");
            if (closeInstructionsBtn != null)
            {
                Debug.Log("✅ Instructions CloseButton found.");
                closeInstructionsBtn.clicked += () =>
                {
                    UIAudioManager.Instance?.PlayCloseSound();
                    Debug.Log("❌ Closing Instructions Panel");
                    instructionRoot.style.display = DisplayStyle.None;
                };
            }
            else
            {
                Debug.LogWarning("❌ CloseButton in Instructions Panel not found.");
            }
        }

        if (shopUIDocument != null)
        {
            var shopRoot = shopUIDocument.rootVisualElement;
            var shopCloseBtn = shopRoot.Q<VisualElement>("ShopPanel")?.Q<VisualElement>("BG")?.Q<Button>();
            if (shopCloseBtn != null)
            {
                Debug.Log("✅ Shop CloseButton found.");
                shopCloseBtn.clicked += () =>
                {
                    UIAudioManager.Instance?.PlayCloseSound();
                    Debug.Log("❌ Closing Shop Panel");
                    shopRoot.style.display = DisplayStyle.None;
                };
            }
            else
            {
                Debug.LogWarning("❌ CloseButton in Shop Panel not found.");
            }
        }
    }
}
