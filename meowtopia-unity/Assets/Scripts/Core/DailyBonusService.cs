// Assets/Scripts/Core/DailyBonusService.cs
using UnityEngine;

public class DailyBonusService : MonoBehaviour {
    [SerializeField] int coinsPerDay = 100;

    void Start() {
        if (SaveSystem.Current == null) SaveSystem.LoadOrCreate();
        var user = SaveSystem.Current.User;
        if (user == null) return;

        var last = SaveSystem.Current.LastSaveUnix;
        var today = new System.DateTimeOffset(System.DateTime.UtcNow.Date).ToUnixTimeSeconds();
        var lastDay = new System.DateTimeOffset(
                          System.DateTimeOffset.FromUnixTimeSeconds(last).UtcDateTime.Date
                      ).ToUnixTimeSeconds();

        if (today > lastDay) {
            EconomyService.EarnCoins(coinsPerDay);
            UIManager.Instance?.ShowNotification($"+{coinsPerDay} daily bonus!");
        }
    }
}
