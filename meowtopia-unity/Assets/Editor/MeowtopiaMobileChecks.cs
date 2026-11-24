#if UNITY_EDITOR
using System;
using System.IO;
using System.Reflection;
using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;

// Exercises the real scenes at phone, tablet and desktop resolutions, including safe areas.
[InitializeOnLoad]
public static class MeowtopiaMobileChecks
{
    private const string Key = "Meowtopia.MobileChecks";
    private const string Home = "Assets/Scenes/Meowtopia_Home.unity";
    private static readonly Vector2Int[] Sizes = {
        new Vector2Int(320, 568), new Vector2Int(390, 844), new Vector2Int(844, 390),
        new Vector2Int(768, 1024), new Vector2Int(1280, 720)
    };
    private static double waitUntil;

    static MeowtopiaMobileChecks() { EditorApplication.update += Tick; }

    [MenuItem("Meowtopia/Check mobile layouts")]
    public static void Run()
    {
        if (EditorApplication.isPlaying) throw new InvalidOperationException("Stop Play mode before running mobile checks.");
        if (!Application.isBatchMode && !EditorSceneManager.SaveCurrentModifiedScenesIfUserWantsTo()) return;
        Directory.CreateDirectory(Output);
        string save = Path.Combine(Application.persistentDataPath, "save.json");
        SessionState.SetBool(Key + ".hadSave", File.Exists(save));
        if (File.Exists(save)) File.Copy(save, Path.Combine(Output, "save-before-checks.json"), true);
        SessionState.SetInt(Key + ".phase", 1);
        SessionState.SetInt(Key + ".size", 0);
        SessionState.SetBool(Key + ".failed", false);
        SessionState.SetBool(Key + ".batch", Application.isBatchMode);
        SessionState.SetFloat(Key + ".started", (float)EditorApplication.timeSinceStartup);
        SessionState.SetFloat(Key + ".resized", (float)EditorApplication.timeSinceStartup);
        SetGameView(Sizes[0]);
        EditorSceneManager.OpenScene("Assets/Scenes/Meowtopia_Loading.unity");
        EditorApplication.isPlaying = true;
        waitUntil = EditorApplication.timeSinceStartup + 3;
    }

    public static void RunBatch()
    {
        Run();
        SessionState.SetBool(Key + ".batch", true);
    }

    [MenuItem("Meowtopia/Preview phone (390 x 844)")]
    public static void PreviewPhone()
    {
        SetGameView(new Vector2Int(390, 844));
        if (!EditorApplication.isPlaying && EditorSceneManager.SaveCurrentModifiedScenesIfUserWantsTo())
            EditorSceneManager.OpenScene(Home);
    }

    private static void Tick()
    {
        int phase = SessionState.GetInt(Key + ".phase", 0);
        if (phase == 0 || EditorApplication.isCompiling || EditorApplication.isUpdating ||
            EditorApplication.timeSinceStartup < waitUntil) return;
        try
        {
            if (phase == 1 && EditorApplication.isPlaying)
            {
                var home = UnityEngine.Object.FindAnyObjectByType<MeowtopiaHomeScreen>();
                if (home == null)
                {
                    Require(EditorApplication.timeSinceStartup - SessionState.GetFloat(Key + ".started", 0) < 45,
                        "Loading did not reach Home within 45 seconds.");
                    return;
                }
                SessionState.SetInt(Key + ".phase", 2);
                waitUntil = EditorApplication.timeSinceStartup + 2;
            }
            else if (phase == 2 && EditorApplication.isPlaying)
            {
                int index = SessionState.GetInt(Key + ".size", 0);
                var size = Sizes[index];
                if (Screen.width != size.x || Screen.height != size.y)
                {
                    Require(EditorApplication.timeSinceStartup - SessionState.GetFloat(Key + ".resized", 0) < 15,
                        "Game view resolution did not update: " + Screen.width + "x" + Screen.height + "; expected " + size);
                    EditorApplication.QueuePlayerLoopUpdate();
                    return;
                }
                var home = UnityEngine.Object.FindAnyObjectByType<MeowtopiaHomeScreen>();
                var layout = home.catNameText.canvas.GetComponent<MeowtopiaMobileLayout>();
                Require(layout != null, "Responsive layout is missing.");
                layout.RefreshLayout();
                ValidateHome(home, layout);
                // Simulate top and bottom cutouts in both orientations.
                layout.ApplyLayout(size, new Rect(12, 34, size.x - 24, size.y - 90));
                ValidateHome(home, layout);
                layout.RefreshLayout();
                MeowtopiaScreenSetup.CheckInteractivePlay();
                CheckTouch(home);
                if (index == 0) CheckBackground(home);
                MeowtopiaScreenSetup.CheckCareButtons();
                home.feedbackText.text = "";
                ScreenCapture.CaptureScreenshot(Path.Combine(Output, "home-" + size.x + "x" + size.y + ".png"));
                Debug.Log("[Mobile checks] Passed " + size + ": bounds, touch targets, safe area, care and toy interaction.");
                SessionState.SetInt(Key + ".phase", 3);
                waitUntil = EditorApplication.timeSinceStartup + 2;
            }
            else if (phase == 3)
            {
                int next = SessionState.GetInt(Key + ".size", 0) + 1;
                if (next < Sizes.Length)
                {
                    SessionState.SetInt(Key + ".size", next);
                    SetGameView(Sizes[next]);
                    SessionState.SetFloat(Key + ".resized", (float)EditorApplication.timeSinceStartup);
                    SessionState.SetInt(Key + ".phase", 2);
                    waitUntil = EditorApplication.timeSinceStartup + 2;
                }
                else
                {
                    SessionState.SetInt(Key + ".phase", 4);
                    EditorApplication.isPlaying = false;
                }
            }
            else if (phase == 4 && !EditorApplication.isPlayingOrWillChangePlaymode)
            {
                string save = Path.Combine(Application.persistentDataPath, "save.json");
                if (SessionState.GetBool(Key + ".hadSave", false)) File.Copy(Path.Combine(Output, "save-before-checks.json"), save, true);
                else if (File.Exists(save)) File.Delete(save);
                bool failed = SessionState.GetBool(Key + ".failed", false);
                SessionState.SetInt(Key + ".phase", 0);
                Debug.Log("[Mobile checks] " + (failed ? "FAILED" : "ALL PASSED") + "; original save restored.");
                if (SessionState.GetBool(Key + ".batch", false)) EditorApplication.Exit(failed ? 1 : 0);
            }
        }
        catch (Exception error)
        {
            Debug.LogException(error);
            SessionState.SetBool(Key + ".failed", true);
            SessionState.SetInt(Key + ".phase", 4);
            EditorApplication.isPlaying = false;
        }
    }

    private static void ValidateHome(MeowtopiaHomeScreen home, MeowtopiaMobileLayout layout)
    {
        Require(SaveSystem.Current != null && SaveSystem.Current.Cat != null, "Loading did not initialize the cat save.");
        Require(home.PlaySession != null && home.catAnimation.idleFrames.Length == 8, "Cat animation or play session did not initialize.");
        var outer = Bounds(layout.SafeRoot);
        Require(home.menuPanel.transform.GetSiblingIndex() == layout.SafeRoot.childCount - 1, "Menu overlay must draw over the controls.");
        foreach (var button in home.careButtons)
        {
            var rect = Bounds((RectTransform)button.transform);
            Require(rect.width >= 44 && rect.height >= 44, "Care button touch target is too small.");
            Require(Contains(outer, rect), "Care button is outside the safe area.");
            Require(!rect.Overlaps(Bounds(home.feedbackText.rectTransform)), "Care button overlaps feedback.");
        }
        Require(Contains(outer, Bounds(home.menuButton.GetComponent<RectTransform>())), "Menu is outside the safe area.");
        var cat = Bounds(home.catAnimation.GetComponent<RectTransform>());
        Require(cat.height > 60 && cat.width > 80, "Cat area collapsed.");
        foreach (var label in home.needLabels)
        {
            Require(Contains(outer, Bounds(label.rectTransform)), "Need label is outside the safe area.");
            Require(!cat.Overlaps(Bounds(label.rectTransform)), "Cat overlaps need labels.");
        }
    }

    private static void CheckTouch(MeowtopiaHomeScreen home)
    {
        var session = home.PlaySession;
        SaveSystem.Current.Cat.Energy = 50;
        session.Begin();
        var toy = (RectTransform)session.Toy.transform;
        var before = toy.anchoredPosition;
        var pointer = new PointerEventData(EventSystem.current) { pointerId = 0,
            position = RectTransformUtility.WorldToScreenPoint(null, toy.position) + new Vector2(8, 8) };
        session.Toy.OnPointerDown(pointer);
        session.Toy.OnDrag(pointer);
        Require(Vector2.Distance(before, toy.anchoredPosition) < .1f, "Toy jumps on touch pickup.");
        var second = new PointerEventData(EventSystem.current) { pointerId = 1, position = Vector2.zero };
        session.Toy.OnDrag(second); session.Toy.OnPointerUp(second);
        Require(session.Toy.IsDragging && Vector2.Distance(before, toy.anchoredPosition) < .1f, "Second finger hijacked the toy.");
        session.Toy.OnPointerUp(pointer);
        Require(!session.Toy.IsDragging, "Touch release did not release toy.");
        // A layout change must clean up active drag and gaze state.
        home.catNameText.canvas.GetComponent<MeowtopiaMobileLayout>().RefreshLayout();
        Require(!session.IsActive && home.catAnimation.LookTarget == null, "Resize left an active drag or gaze target.");
    }

    private static Rect Bounds(RectTransform rect)
    {
        var corners = new Vector3[4]; rect.GetWorldCorners(corners);
        return Rect.MinMaxRect(corners[0].x, corners[0].y, corners[2].x, corners[2].y);
    }
    private static bool Contains(Rect outer, Rect inner) => inner.xMin >= outer.xMin - 1 && inner.yMin >= outer.yMin - 1 && inner.xMax <= outer.xMax + 1 && inner.yMax <= outer.yMax + 1;
    private static void Require(bool valid, string message) { if (!valid) throw new InvalidOperationException(message); }
    private static string Output => Path.GetFullPath(Path.Combine(Application.dataPath, "../../.local-recovery/mobile-checks"));

    private static void CheckBackground(MeowtopiaHomeScreen home)
    {
        var original = JsonUtility.ToJson(SaveSystem.Current.Cat);
        try
        {
            home.SendMessage("OnApplicationPause", false);
            SaveSystem.Current.Cat.Energy = 50;
            home.PlaySession.Begin();
            home.ApplyPlayProgress(.1f);
            float energy = SaveSystem.Current.Cat.Energy;
            SaveSystem.Current.Cat.LastUpdateUnix = SaveSystem.Now() - 120;
            home.SendMessage("OnApplicationPause", true);
            Require(!home.PlaySession.IsActive && home.catAnimation.LookTarget == null, "Suspending left play or gaze active.");
            SaveSystem.LoadOrCreate();
            Require(Mathf.Abs(SaveSystem.Current.Cat.Energy - energy) < .001f, "Suspending did not save play progress.");
            home.SendMessage("OnApplicationPause", false);
            float resumed = SaveSystem.Current.Cat.Energy;
            Require(Mathf.Abs(energy - resumed - 120f / 1800f) < .01f, "Resuming did not account for elapsed care decay.");
            home.SendMessage("OnApplicationFocus", true);
            Require(Mathf.Abs(SaveSystem.Current.Cat.Energy - resumed) < .001f, "Focus and resume applied decay twice.");
            Debug.Log("[Mobile checks] Suspend/resume passed: play cleanup, saved progress, elapsed decay, no double decay.");
        }
        finally
        {
            if (home.PlaySession.IsActive) home.PlaySession.End();
            home.SendMessage("OnApplicationPause", false);
            SaveSystem.Current.Cat = JsonUtility.FromJson<CatData>(original);
            SaveSystem.Write();
        }
    }

    private static void SetGameView(Vector2Int size)
    {
        var assembly = typeof(Editor).Assembly;
        var sizesType = assembly.GetType("UnityEditor.GameViewSizes");
        var singleton = typeof(ScriptableSingleton<>).MakeGenericType(sizesType);
        var sizes = singleton.GetProperty("instance", BindingFlags.Public | BindingFlags.Static | BindingFlags.FlattenHierarchy).GetValue(null);
        var groupType = assembly.GetType("UnityEditor.GameViewSizeGroupType");
        var group = sizesType.GetMethod("GetGroup").Invoke(sizes, new[] { Enum.Parse(groupType, "Standalone") });
        int count = (int)group.GetType().GetMethod("GetBuiltinCount").Invoke(group, null) + (int)group.GetType().GetMethod("GetCustomCount").Invoke(group, null);
        int selected = -1;
        for (int i = 0; i < count; i++)
        {
            var entry = group.GetType().GetMethod("GetGameViewSize").Invoke(group, new object[] { i });
            if ((int)entry.GetType().GetProperty("width").GetValue(entry) == size.x && (int)entry.GetType().GetProperty("height").GetValue(entry) == size.y &&
                entry.GetType().GetProperty("sizeType").GetValue(entry).ToString() == "FixedResolution") { selected = i; break; }
        }
        if (selected < 0)
        {
            var entryType = assembly.GetType("UnityEditor.GameViewSize");
            var kindType = assembly.GetType("UnityEditor.GameViewSizeType");
            var entry = Activator.CreateInstance(entryType, new object[] { Enum.Parse(kindType, "FixedResolution"), size.x, size.y, "Meowtopia " + size });
            group.GetType().GetMethod("AddCustomSize").Invoke(group, new[] { entry });
            selected = count;
        }
        var view = EditorWindow.GetWindow(assembly.GetType("UnityEditor.GameView"));
        view.GetType().GetProperty("selectedSizeIndex", BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance).SetValue(view, selected);
        view.Focus();
        view.Repaint();
        EditorApplication.QueuePlayerLoopUpdate();
        Debug.Log("[Mobile checks] Selected fixed Game view " + size + ", preset " + selected + ".");
    }
}
#endif
