using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class MeowtopiaHomeScreen : MonoBehaviour
{
    public TextMeshProUGUI catNameText, feedbackText;
    public Image[] needFills;
    public TextMeshProUGUI[] needLabels;
    public Button[] careButtons;
    public MeowtopiaAnimatedCat catAnimation;
    public Button menuButton, closeMenuButton, saveMenuButton;
    public GameObject menuPanel;
    public MeowtopiaPlaySession PlaySession { get; private set; }
    private float tick, autosave;
    private bool dirty;
    private CatData Cat => SaveSystem.Current?.Cat;

    private void Awake() { Application.runInBackground = true; }

    private void Start()
    {
        // Opening Home directly in the Editor routes through the same initialization.
        if (SaveSystem.Current == null) SaveSystem.LoadOrCreate();
        if (Cat == null) { UnityEngine.SceneManagement.SceneManager.LoadScene("Meowtopia_Loading"); return; }
        careButtons[0].onClick.AddListener(() => Care(0));
        careButtons[1].onClick.AddListener(() => Care(1));
        careButtons[2].onClick.AddListener(() => Care(2));
        careButtons[3].onClick.AddListener(() => Care(3));
        if (menuButton != null) menuButton.onClick.AddListener(() => {
            if (PlaySession != null && PlaySession.IsActive) PlaySession.End();
            menuPanel.SetActive(!menuPanel.activeSelf);
        });
        if (closeMenuButton != null) closeMenuButton.onClick.AddListener(() => menuPanel.SetActive(false));
        if (saveMenuButton != null) saveMenuButton.onClick.AddListener(SaveFromMenu);
        if (catAnimation != null)
        {
            PlaySession = MeowtopiaPlaySession.Create(this, catAnimation);
            var petButton = catAnimation.GetComponent<Button>();
            if (petButton != null) petButton.onClick.AddListener(Pet);
        }
        var elapsed = Math.Min(8 * 3600, Math.Max(0, SaveSystem.Now() - Cat.LastUpdateUnix));
        Decay(elapsed); Refresh();
        feedbackText.text = "";
    }

    private void Update()
    {
        if (Cat == null) return;
        if (PlaySession != null && PlaySession.IsActive && Input.GetKeyDown(KeyCode.Escape)) PlaySession.End();
        if (menuPanel != null && menuPanel.activeSelf && Input.GetKeyDown(KeyCode.Escape)) menuPanel.SetActive(false);
        tick += Time.deltaTime; autosave += Time.deltaTime;
        if (tick >= 1) { Decay(tick); tick = 0; Refresh(); }
        if (autosave >= 30) { autosave = 0; Save(); }
    }

    private void Decay(float seconds)
    {
        Cat.Hunger = Mathf.Clamp(Cat.Hunger - seconds / 1200, 0, 100);
        Cat.Happiness = Mathf.Clamp(Cat.Happiness - seconds / 1800, 0, 100);
        Cat.Hygiene = Mathf.Clamp(Cat.Hygiene - seconds / 2400, 0, 100);
        Cat.Energy = Mathf.Clamp(Cat.Energy - seconds / 1800, 0, 100);
        Cat.LastUpdateUnix = SaveSystem.Now(); dirty = true;
    }

    public void Care(int action)
    {
        if (Cat == null) return;
        switch (action)
        {
            case 0: Cat.Hunger = Mathf.Min(100, Cat.Hunger + 20); feedbackText.text = "A little meal. A happy little cat."; break;
            case 1:
                if (PlaySession != null) { if (PlaySession.IsActive) PlaySession.End(); else PlaySession.Begin(); }
                return;
            case 2: Cat.Hygiene = Mathf.Min(100, Cat.Hygiene + 20); feedbackText.text = "Freshly brushed and looking lovely."; break;
            case 3: Cat.Energy = Mathf.Min(100, Cat.Energy + 20); feedbackText.text = "A quiet moment to recharge."; break;
            default: return;
        }
        Cat.LastUpdateUnix = SaveSystem.Now(); dirty = true; Refresh(); Save();
        if (catAnimation != null) catAnimation.React(action);
    }

    private void Pet()
    {
        if (Cat == null) return;
        if (PlaySession != null && PlaySession.IsActive) return;
        Cat.Happiness = Mathf.Min(100, Cat.Happiness + 3);
        feedbackText.text = Cat.Name + " leans into your hand. Prrr...";
        dirty = true; Refresh(); Save(); catAnimation.React(2);
    }

    public void ApplyPlayProgress(float seconds)
    {
        if (Cat == null || Cat.Energy <= 5) return;
        Cat.Happiness = Mathf.Min(100, Cat.Happiness + seconds * 2);
        Cat.Energy = Mathf.Max(5, Cat.Energy - seconds * 1.25f);
        Cat.LastUpdateUnix = SaveSystem.Now(); dirty = true; Refresh();
    }
    public void SavePlayProgress() { Save(); }

    private void SaveFromMenu()
    {
        dirty = true; Save();
        if (!dirty) feedbackText.text = "Progress saved.";
        menuPanel.SetActive(false);
    }

    private void Refresh()
    {
        catNameText.text = string.IsNullOrWhiteSpace(Cat.Name) ? "Your cat" : Cat.Name;
        var values = new[] { Cat.Hunger, Cat.Happiness, Cat.Hygiene, Cat.Energy };
        var names = new[] { "Food", "Happiness", "Cleanliness", "Energy" };
        for (var i = 0; i < 4; i++) { MeowtopiaUI.SetBar(needFills[i], values[i] / 100); needLabels[i].text = names[i] + "   " + Mathf.RoundToInt(values[i]) + "%"; }
    }

    private void Save()
    {
        if (!dirty || Cat == null) return;
        try { SaveSystem.Write(); dirty = false; }
        catch (Exception exception) { feedbackText.text = "Progress couldn't be saved. Please try again."; Debug.LogException(exception); }
    }
    private void OnApplicationPause(bool paused) { if (paused) Save(); }
    private void OnApplicationFocus(bool focused) { if (!focused) Save(); }
    private void OnDisable() { Save(); }
}
