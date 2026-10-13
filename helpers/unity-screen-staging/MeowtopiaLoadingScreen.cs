using System;
using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MeowtopiaLoadingScreen : MonoBehaviour
{
    public const string HomeScene = "Meowtopia_Home";
    [SerializeField] public Image progressFill;
    [SerializeField] public TextMeshProUGUI statusText;
    [SerializeField] public Button retryButton;
    [SerializeField, Min(0)] private float minimumDisplaySeconds = .8f;
    private bool loading;

    private void Awake() { Application.runInBackground = true; }

    private void Start() { retryButton.onClick.AddListener(Begin); Begin(); }
    private void Begin() { if (!loading) StartCoroutine(Load()); }

    private IEnumerator Load()
    {
        loading = true; retryButton.gameObject.SetActive(false);
        statusText.text = "Preparing your cat's home...";
        MeowtopiaUI.SetBar(progressFill, 0);
        var started = Time.realtimeSinceStartup;
        yield return null;
        string error = null;
        try
        {
            SaveSystem.LoadOrCreate();
            if (SaveSystem.Current.User == null) SaveSystem.Current.User = new UserData();
            if (SaveSystem.Current.Cat == null)
            {
                SaveSystem.Current.Cat = new CatData {
                    CatId = Guid.NewGuid().ToString(), Name = "Mochi", Breed = "Ragdoll", Level = 1,
                    Hunger = 80, Energy = 85, Happiness = 75, Hygiene = 90, Health = 100, Comfort = 80,
                    BirthUnix = SaveSystem.Now(), LastUpdateUnix = SaveSystem.Now()
                };
                SaveSystem.Current.User.CatId = SaveSystem.Current.Cat.CatId;
                SaveSystem.Write();
            }
        }
        catch (Exception exception) { error = exception.Message; }
        if (error != null) { Fail("Couldn't read your local save. Check the Console and try again.", error); yield break; }
        if (!Application.CanStreamedLevelBeLoaded(HomeScene)) { Fail("Home scene is missing from the build. Run Meowtopia > Set up basic screens.", HomeScene); yield break; }
        var operation = SceneManager.LoadSceneAsync(HomeScene);
        operation.allowSceneActivation = false;
        while (operation.progress < .9f) { MeowtopiaUI.SetBar(progressFill, operation.progress / .9f); yield return null; }
        MeowtopiaUI.SetBar(progressFill, 1); statusText.text = "Your home is ready.";
        while (Time.realtimeSinceStartup - started < minimumDisplaySeconds) yield return null;
        operation.allowSceneActivation = true;
    }

    private void Fail(string message, string detail)
    {
        statusText.text = message; loading = false; retryButton.gameObject.SetActive(true);
        Debug.LogError("Meowtopia loading: " + detail);
    }
}
