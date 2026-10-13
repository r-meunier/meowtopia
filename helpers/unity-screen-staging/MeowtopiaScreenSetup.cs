#if UNITY_EDITOR
using System.Collections.Generic;
using System.IO;
using TMPro;
using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public static class MeowtopiaScreenSetup
{
    private const string LoadingPath = "Assets/Scenes/Meowtopia_Loading.unity";
    private const string HomePath = "Assets/Scenes/Meowtopia_Home.unity";
    private const string CatPath = "Assets/Art/Pets/PetsPack/PNG/Cat/Orange-Cat.png";
    private const string RagdollSheetPath = "Assets/Art/Pets/Ragdoll/RagdollIdle.png";

    [MenuItem("Meowtopia/Check toy eye tracking")]
    public static void CheckToyEyeTracking()
    {
        var cat = Object.FindFirstObjectByType<MeowtopiaAnimatedCat>();
        if (cat == null || !EditorApplication.isPlaying) { Debug.LogError("Run Home first."); return; }
        var previousTarget = cat.LookTarget;
        var target = new GameObject("Gaze check target", typeof(RectTransform)).GetComponent<RectTransform>();
        var rect = cat.GetComponent<RectTransform>();
        target.position = rect.TransformPoint(rect.rect.center + new Vector2(-rect.rect.width, 0));
        cat.SetLookTarget(target);
        var started = EditorApplication.timeSinceStartup; int phase = 0;
        EditorApplication.CallbackFunction check = null;
        check = () => {
            if (cat == null || !EditorApplication.isPlaying) { EditorApplication.update -= check; if (target != null) Object.Destroy(target.gameObject); return; }
            double elapsed = EditorApplication.timeSinceStartup - started;
            if (elapsed < 1) return;
            bool valid = phase == 0 ? cat.Gaze.x < -.2f : phase == 1 ? cat.Gaze.x > .2f : cat.Gaze.magnitude < .05f;
            if (!valid) { EditorApplication.update -= check; cat.SetLookTarget(previousTarget); Object.Destroy(target.gameObject); Debug.LogError("Eye tracking direction or return-to-centre failed."); return; }
            phase++; started = EditorApplication.timeSinceStartup;
            if (phase == 1) target.position = rect.TransformPoint(rect.rect.center + new Vector2(rect.rect.width, 0));
            else if (phase == 2) cat.SetLookTarget(null);
            else { EditorApplication.update -= check; cat.SetLookTarget(previousTarget); Object.Destroy(target.gameObject); Debug.Log("Eye tracking passed: left, right, and smooth return to centre."); }
        };
        EditorApplication.update += check;
    }

    [MenuItem("Meowtopia/Preview toy play")]
    public static void PreviewToyPlay()
    {
        var home = Object.FindFirstObjectByType<MeowtopiaHomeScreen>();
        if (home != null && EditorApplication.isPlaying) home.Care(1);
    }

    [MenuItem("Meowtopia/Check interactive play")]
    public static void CheckInteractivePlay()
    {
        var home = Object.FindFirstObjectByType<MeowtopiaHomeScreen>();
        if (home == null || home.PlaySession == null || !EditorApplication.isPlaying) { Debug.LogError("Run Home before checking interactive play."); return; }
        var session = home.PlaySession;
        if (session.IsActive) session.End();
        var original = JsonUtility.ToJson(SaveSystem.Current.Cat);
        try
        {
            var cat = SaveSystem.Current.Cat; cat.Happiness = cat.Energy = 40;
            home.careButtons[1].onClick.Invoke();
            if (!session.IsActive || cat.Happiness != 40 || cat.Energy != 40) throw new System.Exception("Play must start a session without an instant reward.");
            if (home.catAnimation.LookTarget != (RectTransform)session.Toy.transform) throw new System.Exception("The eyes are not tracking the active toy.");
            var pointer = new PointerEventData(EventSystem.current) { pointerId = -1 };
            var catRect = home.catAnimation.GetComponent<RectTransform>();
            var centre = (Vector2)session.Area.InverseTransformPoint(catRect.TransformPoint(catRect.rect.center));
            session.Toy.OnPointerDown(pointer);
            for (int i = 0; i < 20; i++)
            {
                var point = centre + new Vector2(i % 2 == 0 ? -20 : 20, 0);
                pointer.position = RectTransformUtility.WorldToScreenPoint(null, session.Area.TransformPoint(point));
                session.Toy.OnDrag(pointer); session.Tick(.1f);
            }
            if (cat.Happiness <= 40 || cat.Happiness >= 45 || cat.Energy >= 40 || cat.Energy <= 37)
                throw new System.Exception("Dragging near the cat should gradually increase happiness and spend energy.");
            session.Toy.OnPointerUp(pointer);
            float happiness = cat.Happiness, energy = cat.Energy;
            session.Toy.OnPointerDown(pointer);
            for (int i = 0; i < 10; i++) session.Tick(.1f);
            if (cat.Happiness != happiness || cat.Energy != energy) throw new System.Exception("A stationary toy should give no reward.");
            for (int i = 0; i < 10; i++)
            {
                var far = session.Area.rect.min + new Vector2(40 + (i % 2) * 10, 40);
                pointer.position = RectTransformUtility.WorldToScreenPoint(null, session.Area.TransformPoint(far));
                session.Toy.OnDrag(pointer); session.Tick(.1f);
            }
            if (cat.Happiness != happiness || cat.Energy != energy) throw new System.Exception("A distant toy should give no reward.");
            for (int i = 0; i < 10; i++)
            {
                pointer.position = RectTransformUtility.WorldToScreenPoint(null, session.Area.TransformPoint(new Vector2(session.Area.rect.width * 2, i * 10)));
                session.Toy.OnDrag(pointer); session.Tick(.1f);
            }
            if (cat.Happiness != happiness || cat.Energy != energy) throw new System.Exception("Dragging outside the play area should give no reward.");
            home.careButtons[1].onClick.Invoke();
            if (session.IsActive || session.Toy.IsDragging || !home.careButtons[0].interactable) throw new System.Exception("Ending play must release the toy and restore care buttons.");
            if (home.catAnimation.LookTarget != null) throw new System.Exception("Eye tracking target was not released.");
            SaveSystem.LoadOrCreate();
            if (Mathf.Abs(SaveSystem.Current.Cat.Happiness - happiness) > .001f) throw new System.Exception("Play progress was not saved.");
            SaveSystem.Current.Cat.Energy = 5; session.Begin();
            if (session.IsActive) throw new System.Exception("An exhausted cat should not begin play.");
            Debug.Log("Interactive play passed: pointer dragging, gradual rewards, stationary/outside rejection, session cleanup, save, and low-energy guard.");
        }
        finally
        {
            if (session.IsActive) session.End();
            SaveSystem.Current.Cat = JsonUtility.FromJson<CatData>(original); SaveSystem.Write();
            home.feedbackText.text = "";
        }
    }

    [MenuItem("Meowtopia/Preview closed eyelids")]
    public static void PreviewEyelids()
    {
        var cat = Object.FindFirstObjectByType<MeowtopiaAnimatedCat>();
        if (cat != null && EditorApplication.isPlaying) cat.previewClosedEyes = !cat.previewClosedEyes;
    }

    [MenuItem("Meowtopia/Check animation stability")]
    public static void CheckAnimationStability()
    {
        var cat = Object.FindFirstObjectByType<MeowtopiaAnimatedCat>();
        if (cat == null || !EditorApplication.isPlaying) { Debug.LogError("Run the cat preview first."); return; }
        var image = cat.GetComponent<Image>(); var rect = cat.GetComponent<RectTransform>();
        var sprite = image.sprite; var position = rect.anchoredPosition; var rotation = rect.localRotation;
        var started = EditorApplication.timeSinceStartup;
        EditorApplication.CallbackFunction check = null;
        check = () => {
            if (cat == null || !EditorApplication.isPlaying) { EditorApplication.update -= check; return; }
            if (image.sprite != sprite || Vector2.Distance(position, rect.anchoredPosition) > .01f || Quaternion.Angle(rotation, rect.localRotation) > .01f)
            { EditorApplication.update -= check; Debug.LogError("Cat pose or position changed unexpectedly."); return; }
            if (EditorApplication.timeSinceStartup - started >= 5)
            { EditorApplication.update -= check; Debug.Log("Animation stability passed: fixed body sprite, position, and rotation across a breathing/blink cycle."); }
        };
        EditorApplication.update += check;
    }

    [MenuItem("Meowtopia/Apply Ragdoll and compact layout")]
    public static void ApplyRagdoll()
    {
        if (EditorApplication.isPlaying) { Debug.LogError("Stop Play mode before applying scene changes."); return; }
        if (!EditorSceneManager.SaveCurrentModifiedScenesIfUserWantsTo()) return;
        var texture = AssetDatabase.LoadAssetAtPath<Texture2D>(RagdollSheetPath);
        if (texture == null) { Debug.LogError("RagdollIdle.png has not been imported."); return; }
        var importer = (TextureImporter)AssetImporter.GetAtPath(RagdollSheetPath);
        importer.alphaIsTransparency = true; importer.mipmapEnabled = false; importer.textureCompression = TextureImporterCompression.Uncompressed;
        importer.maxTextureSize = 4096; importer.SaveAndReimport();
        texture = AssetDatabase.LoadAssetAtPath<Texture2D>(RagdollSheetPath);
        var previewPath = "Assets/Art/Pets/Ragdoll/RagdollIdlePreview.asset";
        if (AssetDatabase.LoadAssetAtPath<Sprite>(previewPath) == null)
        {
            var preview = Sprite.Create(texture, new Rect(0, texture.height / 2, texture.width / 4, texture.height / 2), new Vector2(.5f, .5f), 100, 0, SpriteMeshType.FullRect);
            AssetDatabase.CreateAsset(preview, previewPath);
        }
        // Only rebuild the two prototype screens requested in this iteration.
        BuildLoading(); BuildHome();
        EditorSceneManager.OpenScene(LoadingPath);
        Debug.Log("Ragdoll animation and compact layout applied.");
    }

    [MenuItem("Meowtopia/Check care buttons in Play mode")]
    public static void CheckCareButtons()
    {
        var screen = Object.FindFirstObjectByType<MeowtopiaHomeScreen>();
        if (!EditorApplication.isPlaying || screen == null) { Debug.LogError("Run the Home scene before checking buttons."); return; }
        var original = JsonUtility.ToJson(SaveSystem.Current.Cat);
        try
        {
            var cat = SaveSystem.Current.Cat;
            cat.Hunger = cat.Happiness = cat.Hygiene = cat.Energy = 40;
            screen.careButtons[0].onClick.Invoke();
            screen.careButtons[2].onClick.Invoke();
            screen.careButtons[3].onClick.Invoke();
            screen.careButtons[1].onClick.Invoke(); screen.careButtons[1].onClick.Invoke();
            if (cat.Hunger != 60 || cat.Happiness != 40 || cat.Hygiene != 60 || cat.Energy != 60)
                throw new System.Exception("Care button effects did not match expected needs.");
            if (!screen.needLabels[0].text.Contains("60%")) throw new System.Exception("Need labels did not refresh.");
            SaveSystem.LoadOrCreate();
            if (SaveSystem.Current.Cat.Hunger != 60) throw new System.Exception("Care change was not persisted.");
            if (screen.menuButton != null)
            {
                screen.menuButton.onClick.Invoke();
                if (!screen.menuPanel.activeSelf) throw new System.Exception("Menu did not open.");
                screen.closeMenuButton.onClick.Invoke();
                if (screen.menuPanel.activeSelf) throw new System.Exception("Menu did not close.");
                if (screen.catAnimation == null || screen.catAnimation.idleFrames.Length != 8)
                    throw new System.Exception("The Ragdoll's eight animation frames were not loaded.");
                if (GameObject.Find("Needs heading") != null || GameObject.Find("Footer") != null)
                    throw new System.Exception("Removed headings are still present.");
            }
            Debug.Log("Meowtopia verification passed: care buttons, displayed needs, save, menu, and animation frames.");
        }
        finally
        {
            SaveSystem.Current.Cat = JsonUtility.FromJson<CatData>(original);
            SaveSystem.Write();
        }
    }

    [MenuItem("Meowtopia/Set up basic screens")]
    public static void Setup()
    {
        if (EditorApplication.isPlaying) { Debug.LogError("Stop Play mode before setting up screens."); return; }
        if (File.Exists(LoadingPath) || File.Exists(HomePath)) { Debug.LogWarning("Basic scenes already exist; keeping your edits."); return; }
        if (!EditorSceneManager.SaveCurrentModifiedScenesIfUserWantsTo()) return;
        Directory.CreateDirectory("Assets/Scenes");
        BuildLoading(); BuildHome();
        var scenes = new List<EditorBuildSettingsScene> {
            new EditorBuildSettingsScene(LoadingPath, true), new EditorBuildSettingsScene(HomePath, true)
        };
        foreach (var scene in EditorBuildSettings.scenes)
            if (scene.path != LoadingPath && scene.path != HomePath) scenes.Add(scene);
        EditorBuildSettings.scenes = scenes.ToArray();
        EditorSceneManager.OpenScene(LoadingPath);
        Debug.Log("Meowtopia basic screens are ready. Press Play from Meowtopia_Loading.");
    }

    private static Canvas NewScene()
    {
        EditorSceneManager.NewScene(NewSceneSetup.EmptyScene, NewSceneMode.Single);
        var camera = new GameObject("Main Camera", typeof(Camera), typeof(AudioListener));
        camera.tag = "MainCamera";
        var view = camera.GetComponent<Camera>(); view.clearFlags = CameraClearFlags.SolidColor; view.backgroundColor = MeowtopiaUI.Cream;
        new GameObject("EventSystem", typeof(EventSystem), typeof(StandaloneInputModule));
        return MeowtopiaUI.Canvas();
    }

    private static void CatImage(Transform parent, Vector2 min, Vector2 max)
    {
        var image = MeowtopiaUI.Panel(parent, "Cat illustration", min, max, Color.white);
        image.sprite = AssetDatabase.LoadAssetAtPath<Sprite>("Assets/Art/Pets/Ragdoll/RagdollIdlePreview.asset") ?? AssetDatabase.LoadAssetAtPath<Sprite>(CatPath); image.preserveAspect = true;
        var sheet = AssetDatabase.LoadAssetAtPath<Texture2D>(RagdollSheetPath);
        if (sheet != null) image.gameObject.AddComponent<MeowtopiaAnimatedCat>().idleSheet = sheet;
        if (image.sprite == null) Debug.LogWarning("Assign your cat sprite to Cat illustration.");
    }

    private static void BuildLoading()
    {
        var canvas = NewScene(); var root = canvas.transform;
        var screen = new GameObject("Loading Screen Controller").AddComponent<MeowtopiaLoadingScreen>();
        MeowtopiaUI.Text(root, "Brand", "MEOWTOPIA", new Vector2(.1f, .74f), new Vector2(.9f, .85f), 44);
        MeowtopiaUI.Text(root, "Tagline", "A little home. A little friend.", new Vector2(.1f, .67f), new Vector2(.9f, .74f), 22);
        CatImage(root, new Vector2(.35f, .35f), new Vector2(.65f, .65f));
        screen.progressFill = MeowtopiaUI.Bar(root, "Loading progress", new Vector2(.28f, .27f), new Vector2(.72f, .29f));
        screen.statusText = MeowtopiaUI.Text(root, "Loading status", "Preparing your cat's home...", new Vector2(.15f, .17f), new Vector2(.85f, .25f), 20);
        screen.retryButton = MeowtopiaUI.Button(root, "Try again", new Vector2(.4f, .07f), new Vector2(.6f, .14f));
        screen.retryButton.gameObject.SetActive(false);
        EditorSceneManager.SaveScene(EditorSceneManager.GetActiveScene(), LoadingPath);
    }

    private static void BuildHome()
    {
        var canvas = NewScene(); var root = canvas.transform;
        var screen = new GameObject("Home Screen Controller").AddComponent<MeowtopiaHomeScreen>();
        MeowtopiaUI.Text(root, "Brand", "MEOWTOPIA", new Vector2(.06f, .88f), new Vector2(.48f, .97f), 32).alignment = TextAlignmentOptions.MidlineLeft;
        var menuImage = MeowtopiaUI.Panel(root, "Menu button", new Vector2(.89f, .905f), new Vector2(.95f, .975f), MeowtopiaUI.Green);
        menuImage.raycastTarget = true;
        screen.menuButton = menuImage.gameObject.AddComponent<Button>(); screen.menuButton.targetGraphic = menuImage;
        for (int i = 0; i < 3; i++)
            MeowtopiaUI.Panel(menuImage.transform, "Menu line " + i, new Vector2(.25f, .26f + i * .2f), new Vector2(.75f, .32f + i * .2f), Color.white);
        var card = MeowtopiaUI.Panel(root, "Pet care panel", new Vector2(.04f, .20f), new Vector2(.96f, .87f), Color.white);
        screen.catNameText = MeowtopiaUI.Text(card.transform, "Cat name", "Mochi", new Vector2(.04f, .84f), new Vector2(.55f, .97f), 30);
        CatImage(card.transform, new Vector2(.03f, .035f), new Vector2(.58f, .85f));
        screen.catAnimation = card.transform.Find("Cat illustration").GetComponent<MeowtopiaAnimatedCat>();
        if (screen.catAnimation != null)
        {
            var catImage = screen.catAnimation.GetComponent<Image>(); catImage.raycastTarget = true;
            var pet = catImage.gameObject.AddComponent<Button>(); pet.targetGraphic = catImage;
            pet.transition = Selectable.Transition.None;
        }
        screen.needFills = new Image[4]; screen.needLabels = new TextMeshProUGUI[4];
        var names = new[] { "Food", "Happiness", "Cleanliness", "Energy" };
        for (int i = 0; i < 4; i++)
        {
            var top = .87f - i * .22f;
            screen.needLabels[i] = MeowtopiaUI.Text(card.transform, names[i] + " label", names[i], new Vector2(.62f, top - .075f), new Vector2(.95f, top), 20);
            screen.needLabels[i].alignment = TextAlignmentOptions.MidlineLeft;
            screen.needFills[i] = MeowtopiaUI.Bar(card.transform, names[i] + " bar", new Vector2(.62f, top - .13f), new Vector2(.95f, top - .10f));
        }
        screen.feedbackText = MeowtopiaUI.Text(root, "Care feedback", "", new Vector2(.06f, .14f), new Vector2(.94f, .20f), 18);
        screen.careButtons = new Button[4];
        var actions = new[] { "Feed", "Play", "Brush", "Rest" };
        for (int i = 0; i < 4; i++)
        {
            var left = .06f + .225f * i;
            screen.careButtons[i] = MeowtopiaUI.Button(root, actions[i], new Vector2(left, .035f), new Vector2(left + .205f, .13f));
        }
        var overlay = MeowtopiaUI.Panel(root, "Menu overlay", Vector2.zero, Vector2.one, new Color(0, 0, 0, .35f));
        overlay.raycastTarget = true; screen.menuPanel = overlay.gameObject;
        var menu = MeowtopiaUI.Panel(overlay.transform, "Menu panel", new Vector2(.32f, .27f), new Vector2(.68f, .73f), MeowtopiaUI.Cream);
        MeowtopiaUI.Text(menu.transform, "Title", "Menu", new Vector2(.1f, .7f), new Vector2(.9f, .93f), 30);
        screen.saveMenuButton = MeowtopiaUI.Button(menu.transform, "Save progress", new Vector2(.1f, .41f), new Vector2(.9f, .61f));
        screen.closeMenuButton = MeowtopiaUI.Button(menu.transform, "Back to cat", new Vector2(.1f, .13f), new Vector2(.9f, .33f));
        overlay.gameObject.SetActive(false);
        EditorSceneManager.SaveScene(EditorSceneManager.GetActiveScene(), HomePath);
    }
}
#endif
