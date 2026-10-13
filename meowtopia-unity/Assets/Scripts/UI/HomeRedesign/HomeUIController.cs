using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.SceneManagement;

public class HomeUIController : MonoBehaviour {
    [Header("Top Bar")]
    public TextMeshProUGUI coinsText;

    [Header("Stats")]
    public Slider hungerSlider;
    public Slider energySlider;
    public Slider happinessSlider;

    [Header("Buttons")]
    public Button feedButton;

    void Start() {
        if (feedButton != null) {
            feedButton.onClick.AddListener(OnFeedClicked);
        }

        // Ensure data exists
        if (SaveSystem.Current == null) SaveSystem.LoadOrCreate();
    }

    void Update() {
        if (SaveSystem.Current == null) return;
        var user = SaveSystem.Current.User;
        var cat = SaveSystem.Current.Cat;

        // Update Coins
        if (coinsText != null) {
            coinsText.text = user.Coins.ToString("N0");
        }

        // Update Sliders
        if (cat != null) {
            if (hungerSlider != null) hungerSlider.value = cat.Hunger / 100f;
            if (energySlider != null) energySlider.value = cat.Energy / 100f;
            if (happinessSlider != null) happinessSlider.value = cat.Happiness / 100f;
        }
    }

    void OnFeedClicked() {
        SceneManager.LoadScene("04_Pantry");
    }
}
