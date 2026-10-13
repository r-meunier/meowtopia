using UnityEngine;

public static class EconomyService {
    static bool EnsureSave() {
        if (SaveSystem.Current == null) SaveSystem.LoadOrCreate();
        return SaveSystem.Current != null && SaveSystem.Current.User != null;
    }

    public static void EarnCoins(int amount) {
        if (amount <= 0) return;
        if (!EnsureSave()) { Debug.LogWarning("EarnCoins: no save/user"); return; }

        var user = SaveSystem.Current.User;
        user.Coins += amount;

        UIManager.Instance?.UpdateWallet(user.Coins, user.Gems);
        SaveSystem.MarkDirtyAndMaybeAutosave();
    }

    public static bool SpendCoins(int amount) {
        if (amount <= 0) return true;
        if (!EnsureSave()) { Debug.LogWarning("SpendCoins: no save/user"); return false; }

        var user = SaveSystem.Current.User;
        if (user.Coins < amount) return false;

        user.Coins -= amount;
        UIManager.Instance?.UpdateWallet(user.Coins, user.Gems);
        SaveSystem.MarkDirtyAndMaybeAutosave();
        return true;
    }

    public static void EarnGems(int amount) {
        if (amount <= 0) return;
        if (!EnsureSave()) { Debug.LogWarning("EarnGems: no save/user"); return; }

        var user = SaveSystem.Current.User;
        user.Gems += amount;

        UIManager.Instance?.UpdateWallet(user.Coins, user.Gems);
        SaveSystem.MarkDirtyAndMaybeAutosave();
    }

    public static bool SpendGems(int amount) {
        if (amount <= 0) return true;
        if (!EnsureSave()) { Debug.LogWarning("SpendGems: no save/user"); return false; }

        var user = SaveSystem.Current.User;
        if (user.Gems < amount) return false;

        user.Gems -= amount;
        UIManager.Instance?.UpdateWallet(user.Coins, user.Gems);
        SaveSystem.MarkDirtyAndMaybeAutosave();
        return true;
    }
}
