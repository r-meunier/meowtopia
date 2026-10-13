using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class UIManager : MonoBehaviour {
    public static UIManager Instance { get; private set; }

    [System.Serializable]
    public class UIScreen {
        public string ScreenId;
        public GameObject ScreenObject;
        public bool DeactivateWhenHidden = true;
    }

    [Header("UI Screens")]
    [SerializeField] private List<UIScreen> screens = new List<UIScreen>();
    [SerializeField] private string initialScreenId = "Loading";
    [SerializeField] private string currentScreenId;

    [Header("Common UI Elements")]
    [SerializeField] private TextMeshProUGUI coinsText;
    [SerializeField] private TextMeshProUGUI gemsText;
    [SerializeField] private GameObject loadingOverlay;
    [SerializeField] private Slider loadingBar;
    [SerializeField] private TextMeshProUGUI notificationText;
    [SerializeField] private Animator notificationAnimator;

    private Stack<string> screenHistory = new Stack<string>();
    private Dictionary<string, UIScreen> screenDictionary = new Dictionary<string, UIScreen>();

    private void Awake() {
        if (Instance != null && Instance != this) { Destroy(gameObject); return; }
        Instance = this;
        // DontDestroyOnLoad(gameObject); // With this, UIManager is kept along screens, without it it's screen-local

        foreach (var screen in screens) {
            if (!string.IsNullOrEmpty(screen.ScreenId))
                screenDictionary[screen.ScreenId] = screen;
        }
    }

    private void Start() {
        if (SaveSystem.Current == null) SaveSystem.LoadOrCreate();

        var user = SaveSystem.Current?.User;

        // Initialize wallet display once from save
        if (SaveSystem.Current != null && user != null) {
            UpdateWallet(user.Coins, user.Gems);
        }

        if (!string.IsNullOrEmpty(initialScreenId) && screenDictionary.ContainsKey(initialScreenId))
            ShowScreen(initialScreenId);

        if (loadingOverlay != null) loadingOverlay.SetActive(false);
    }

    public void UpdateWallet(int coins, int gems) {
        if (coinsText) coinsText.text = coins.ToString();
        if (gemsText) gemsText.text = gems.ToString();
    }

    public void ShowScreen(string screenId, string parameter = null) {
        if (string.IsNullOrEmpty(screenId) || !screenDictionary.ContainsKey(screenId)) {
            Debug.LogError($"Screen not found: {screenId}");
            return;
        }

        if (!string.IsNullOrEmpty(currentScreenId) && screenDictionary.ContainsKey(currentScreenId)) {
            screenHistory.Push(currentScreenId);
            var currentScreen = screenDictionary[currentScreenId];
            if (currentScreen.DeactivateWhenHidden) {
                currentScreen.ScreenObject.SetActive(false);
            } else {
                var cg = currentScreen.ScreenObject.GetComponent<CanvasGroup>();
                if (cg != null) { cg.alpha = 0; cg.interactable = false; cg.blocksRaycasts = false; }
            }
        }

        var newScreen = screenDictionary[screenId];
        newScreen.ScreenObject.SetActive(true);

        var newCg = newScreen.ScreenObject.GetComponent<CanvasGroup>();
        if (newCg != null) { newCg.alpha = 1; newCg.interactable = true; newCg.blocksRaycasts = true; }

        currentScreenId = screenId;

        var initializer = newScreen.ScreenObject.GetComponent<UIScreenInitializer>();
        if (initializer != null) initializer.Initialize(parameter);

        Debug.Log($"Showing screen: {screenId}");
    }

    public void GoBack() {
        if (screenHistory.Count > 0) {
            string previous = screenHistory.Pop();
            ShowScreen(previous, null);
            if (screenHistory.Count > 0 && screenHistory.Peek() == currentScreenId) screenHistory.Pop();
        } else {
            ShowScreen("MainMenu");
        }
    }

    public void ShowLoadingOverlay(bool show) {
        if (loadingOverlay != null) {
            loadingOverlay.SetActive(show);
            if (loadingBar != null) loadingBar.value = 0;
        }
    }

    public void UpdateLoadingProgress(float progress) {
        if (loadingBar != null) loadingBar.value = Mathf.Clamp01(progress);
    }

    public void ShowNotification(string message, float duration = 2f) {
        if (notificationText != null && notificationAnimator != null) {
            notificationText.text = message;
            notificationAnimator.SetTrigger("Show");
            StartCoroutine(HideNotificationAfterDelay(duration));
        } else {
            Debug.Log($"Notification: {message}");
        }
    }

    private IEnumerator HideNotificationAfterDelay(float delay) {
        yield return new WaitForSeconds(delay);
        if (notificationAnimator != null) notificationAnimator.SetTrigger("Hide");
    }
}

// Keep this for screens that accept parameters
public interface UIScreenInitializer { void Initialize(string parameter); }
