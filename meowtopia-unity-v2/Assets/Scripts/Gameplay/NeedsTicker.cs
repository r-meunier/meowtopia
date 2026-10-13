// Assets/Scripts/Gameplay/NeedsTicker.cs
using UnityEngine;
using UnityEngine.UI;

public class NeedsTicker : MonoBehaviour {
    [Header("UI")]
    [SerializeField] Slider hunger;
    [SerializeField] Slider energy;
    [SerializeField] Slider happiness;
    [SerializeField] Slider hygiene;
    [SerializeField] Slider health;

    [Header("Decay/sec")]
    [SerializeField] float hungerDec = 0.15f;
    [SerializeField] float energyDec = 0.10f;
    [SerializeField] float happyDec = 0.08f;
    [SerializeField] float hygieneDec = 0.05f;
    [SerializeField] float healthDec = 0.03f;

    float accum;

    void Start() {
        // Redesign Injection
        if (gameObject.GetComponent<HomeCanvasGenerator>() == null) {
            gameObject.AddComponent<HomeCanvasGenerator>();
        }

        if (SaveSystem.Current == null) SaveSystem.LoadOrCreate();
        ApplyOfflineCatchup();
        RefreshUI();
    }

    void Update() {
        var cat = SaveSystem.Current?.Cat; if (cat == null) return;
        accum += Time.deltaTime;
        if (accum < 1f) return; // tick every 1s
        var s = accum; accum = 0f;

        cat.Hunger = Mathf.Clamp(cat.Hunger - hungerDec * s, 0, 100);
        cat.Energy = Mathf.Clamp(cat.Energy - energyDec * s, 0, 100);
        cat.Happiness = Mathf.Clamp(cat.Happiness - happyDec * s, 0, 100);
        cat.Hygiene = Mathf.Clamp(cat.Hygiene - hygieneDec * s, 0, 100);

        float hpPenalty = 0f;
        if (cat.Hunger < 20) hpPenalty += 0.06f;
        if (cat.Hygiene < 20) hpPenalty += 0.04f;
        if (cat.Happiness < 20) hpPenalty += 0.04f;
        cat.Health = Mathf.Clamp(cat.Health - (healthDec + hpPenalty) * s, 0, 100);

        cat.LastUpdateUnix = SaveSystem.Now();
        SaveSystem.MarkDirtyAndMaybeAutosave();
        RefreshUI();
    }

    void ApplyOfflineCatchup() {
        var cat = SaveSystem.Current?.Cat; if (cat == null) return;
        var delta = Mathf.Max(0, (int)(SaveSystem.Now() - cat.LastUpdateUnix));
        var capped = Mathf.Min(delta, 8 * 3600);
        if (capped <= 0) return;

        cat.Hunger = Mathf.Clamp(cat.Hunger - hungerDec * capped, 0, 100);
        cat.Energy = Mathf.Clamp(cat.Energy - energyDec * capped, 0, 100);
        cat.Happiness = Mathf.Clamp(cat.Happiness - happyDec * capped, 0, 100);
        cat.Hygiene = Mathf.Clamp(cat.Hygiene - hygieneDec * capped, 0, 100);
        cat.Health = Mathf.Clamp(cat.Health - healthDec * capped, 0, 100);

        cat.LastUpdateUnix = SaveSystem.Now();
        SaveSystem.MarkDirtyAndMaybeAutosave();
    }

    void RefreshUI() {
        var cat = SaveSystem.Current?.Cat; if (cat == null) return;
        if (hunger) hunger.value = cat.Hunger / 100f;
        if (energy) energy.value = cat.Energy / 100f;
        if (happiness) happiness.value = cat.Happiness / 100f;
        if (hygiene) hygiene.value = cat.Hygiene / 100f;
        if (health) health.value = cat.Health / 100f;
    }
}
