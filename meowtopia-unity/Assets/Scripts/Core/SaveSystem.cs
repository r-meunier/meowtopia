using System.IO;
using UnityEngine;
using Newtonsoft.Json;
using System;

public static class SaveSystem {
    private static readonly string Path = System.IO.Path.Combine(Application.persistentDataPath, "save.json");
    private static readonly JsonSerializerSettings Settings = new JsonSerializerSettings {
        Formatting = Formatting.None,
        NullValueHandling = NullValueHandling.Ignore
    };

    public static GameSaveData Current { get; private set; }

    public static void New(GameSaveData fresh) {
        Current = fresh;
        Current.LastSaveUnix = Now();
        Write();
    }

    public static void LoadOrCreate() {
        if (File.Exists(Path)) {
            var json = File.ReadAllText(Path);
            Current = JsonConvert.DeserializeObject<GameSaveData>(json, Settings) ?? new GameSaveData();
        } else {
            Current = new GameSaveData();
        }
        if (Current.SaveVersion <= 0) Current.SaveVersion = 1;
    }

    public static void Write() {
        Current.LastSaveUnix = Now();
        var json = JsonConvert.SerializeObject(Current, Settings);
        File.WriteAllText(Path, json);
    }

    public static void MarkDirtyAndMaybeAutosave() {
        // Call this after key interactions; you can throttle autosaves if needed
        Write();
    }

    public static long Now() => DateTimeOffset.UtcNow.ToUnixTimeSeconds();
}
