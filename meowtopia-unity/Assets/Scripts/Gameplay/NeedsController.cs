using System;
using System.Collections.Generic;
using UnityEngine;

[Serializable] public class NeedState { public string id; public float value; }

public class NeedsController : MonoBehaviour {
    [SerializeField] private PetDefinition pet;
    [SerializeField] private float autosaveInterval = 60f;

    private readonly Dictionary<string, NeedState> _state = new();
    private ITime _time;
    private float _t;

    private string SavePath => System.IO.Path.Combine(Application.persistentDataPath, "pet.json");

    void Awake() {
        _time = new UnityTime();
        foreach (var n in pet.needs)
            _state[n.needId] = new NeedState { id = n.needId, value = Mathf.Clamp(n.value, 0, 100) };
        Load();
    }

    void Update() {
        var dt = _time.DeltaTime;
        foreach (var n in pet.needs) {
            var s = _state[n.needId];
            s.value = Mathf.Clamp(s.value - n.decayPerSecond * dt, 0, 100);
        }
        _t += dt;
        if (_t >= autosaveInterval) { _t = 0; Save(); }
    }

    public float GetNeed(string id) => _state[id].value;

    public void ApplyItem(string id, float amount) {
        if (_state.TryGetValue(id, out var s))
            s.value = Mathf.Clamp(s.value + amount, 0, 100);
    }

    [Serializable] class SaveBlob { public List<NeedState> needs = new(); }

    void Save() {
        var blob = new SaveBlob { needs = new List<NeedState>(_state.Values) };
        System.IO.File.WriteAllText(SavePath, JsonUtility.ToJson(blob));
    }

    void Load() {
        if (!System.IO.File.Exists(SavePath)) return;
        var blob = JsonUtility.FromJson<SaveBlob>(System.IO.File.ReadAllText(SavePath));
        if (blob?.needs == null) return;
        foreach (var s in blob.needs)
            if (_state.ContainsKey(s.id)) _state[s.id].value = Mathf.Clamp(s.value, 0, 100);
    }
}
