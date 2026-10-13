using UnityEngine;

public class TimeService : MonoBehaviour {
    public static long NowUnix => SaveSystem.Now();

    private void OnApplicationPause(bool paused) {
        if (!paused) {
            // app resumed → bump cat stats via your NeedsTicker offline catch-up
            EventBus.OnCatStatsChanged?.Invoke();
        }
    }
}
