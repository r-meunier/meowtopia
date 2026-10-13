using UnityEngine;

public class AddressableLoader : MonoBehaviour {
    [SerializeField] bool warmOnStart = false;
    private void Start() {
        if (warmOnStart) {
            // TODO: load labels/groups when you define them
            // Addressables.LoadResourceLocationsAsync("UI");
        }
    }
}
