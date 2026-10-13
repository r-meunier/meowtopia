using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class MeowtopiaPlaySession : MonoBehaviour
{
    public bool IsActive { get; private set; }
    public MeowtopiaDraggableToy Toy { get; private set; }
    public RectTransform Area => (RectTransform)transform;
    private MeowtopiaHomeScreen home;
    private RectTransform cat;
    private TextMeshProUGUI playLabel;
    private Vector2 previous;
    private float sampleTime, travelled, saveTime;

    public static MeowtopiaPlaySession Create(MeowtopiaHomeScreen home, MeowtopiaAnimatedCat animatedCat)
    {
        var area = MeowtopiaUI.Rect(animatedCat.transform.parent, "Toy play area", new Vector2(.02f, .03f), new Vector2(.59f, .83f));
        var session = area.gameObject.AddComponent<MeowtopiaPlaySession>();
        session.home = home; session.cat = (RectTransform)animatedCat.transform;
        session.playLabel = home.careButtons[1].GetComponentInChildren<TextMeshProUGUI>();
        var toyRect = MeowtopiaUI.Rect(area, "Yarn ball", new Vector2(.5f, .5f), new Vector2(.5f, .5f));
        toyRect.sizeDelta = new Vector2(60, 60);
        toyRect.gameObject.AddComponent<MeowtopiaToyGraphic>();
        session.Toy = toyRect.gameObject.AddComponent<MeowtopiaDraggableToy>(); session.Toy.playArea = area;
        area.gameObject.SetActive(false);
        return session;
    }

    public void Begin()
    {
        if (SaveSystem.Current?.Cat == null) return;
        if (SaveSystem.Current.Cat.Energy <= 5) { home.feedbackText.text = "Your cat is tired. Try Rest before playing."; return; }
        IsActive = true; gameObject.SetActive(true);
        Toy.Cancel(); ((RectTransform)Toy.transform).anchoredPosition = new Vector2(-Area.rect.width * .25f, -Area.rect.height * .25f);
        previous = ((RectTransform)Toy.transform).anchoredPosition; sampleTime = travelled = saveTime = 0;
        home.catAnimation.SetLookTarget((RectTransform)Toy.transform);
        if (playLabel != null) playLabel.text = "Done playing";
        SetCareButtons(false);
        home.feedbackText.text = "Drag the yarn ball near " + SaveSystem.Current.Cat.Name + ".";
    }

    public void End(string message = "Playtime finished.")
    {
        if (!IsActive) return;
        IsActive = false; Toy.Cancel();
        home.catAnimation.SetLookTarget(null);
        if (playLabel != null) playLabel.text = "Play";
        SetCareButtons(true); home.SavePlayProgress(); home.feedbackText.text = message;
        gameObject.SetActive(false);
    }
    private void SetCareButtons(bool available)
    {
        for (int i = 0; i < home.careButtons.Length; i++) if (i != 1) home.careButtons[i].interactable = available;
    }
    private void Update() { Tick(Mathf.Min(Time.deltaTime, .1f)); }

    // Uses actual toy travel, not pointer speed or a click reward. Also used by the Editor smoke check.
    public void Tick(float seconds)
    {
        if (!IsActive || SaveSystem.Current?.Cat == null || seconds <= 0) return;
        var position = ((RectTransform)Toy.transform).anchoredPosition;
        if (Toy.IsDragging && Toy.PointerInside) travelled += Vector2.Distance(previous, position);
        previous = position; sampleTime += seconds; saveTime += seconds;
        if (sampleTime < .2f) return;
        var duration = Mathf.Min(sampleTime, .25f); sampleTime = 0;
        var catCentre = (Vector2)Area.InverseTransformPoint(cat.TransformPoint(cat.rect.center));
        bool near = Vector2.Distance(position, catCentre) <= Mathf.Min(Area.rect.width, Area.rect.height) * .48f;
        bool engaged = Toy.IsDragging && Toy.PointerInside && travelled >= 3 && near;
        travelled = 0;
        if (engaged)
        {
            home.ApplyPlayProgress(duration);
            home.feedbackText.text = SaveSystem.Current.Cat.Name + " is following the yarn ball!";
            if (SaveSystem.Current.Cat.Energy <= 5) { End("Your cat is tired. Time for a rest."); return; }
        }
        else if (Toy.IsDragging) home.feedbackText.text = near ? "Keep the yarn ball moving." : "Bring the yarn ball closer to your cat.";
        if (saveTime >= 5) { saveTime = 0; home.SavePlayProgress(); }
    }

    private void OnApplicationFocus(bool focused) { if (!focused && IsActive) End("Play paused."); }
    private void OnApplicationPause(bool paused) { if (paused && IsActive) End("Play paused."); }
    private void OnDisable()
    {
        if (!IsActive) return;
        IsActive = false;
        if (Toy != null) Toy.Cancel();
        if (home != null) home.SavePlayProgress();
        if (home != null && home.catAnimation != null) home.catAnimation.SetLookTarget(null);
    }
}
