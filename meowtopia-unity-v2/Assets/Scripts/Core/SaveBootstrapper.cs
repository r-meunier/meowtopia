using UnityEngine;

public class SaveBootstrapper : MonoBehaviour {
    void Awake() {
        if (SaveSystem.Current == null)
            SaveSystem.LoadOrCreate();
    }
}
