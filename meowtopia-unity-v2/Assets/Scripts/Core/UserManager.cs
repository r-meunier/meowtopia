using System.Collections.Generic;
using UnityEngine;

public class UserManager : MonoBehaviour {
    public static UserManager Instance { get; private set; }
    private UserData Data => SaveSystem.Current.User;

    private void Awake() {
        if (Instance != null && Instance != this) { Destroy(gameObject); return; }
        Instance = this;
        DontDestroyOnLoad(gameObject);
    }

    public string CurrentUserId => Data?.UserId;
    public (int coins, int gems) GetWallet() => (Data.Coins, Data.Gems);

    public void SetUsername(string name) {
        if (string.IsNullOrWhiteSpace(name)) return;
        Data.Username = name;
        SaveSystem.MarkDirtyAndMaybeAutosave();
    }

    // Economy helpers
    public void AddCoins(int amount) => EconomyService.EarnCoins(amount);
    public bool SpendCoins(int amount) => EconomyService.SpendCoins(amount);
    public void AddGems(int amount) => EconomyService.EarnGems(amount);
    public bool SpendGems(int amount) => EconomyService.SpendGems(amount);

    // Cat (single)
    public void SetUserCatId(string catId) {
        Data.CatId = catId;
        SaveSystem.MarkDirtyAndMaybeAutosave();
    }

    public string GetUserCatId() => Data.CatId;

    // Inventory
    public List<InventoryStack> GetInventory() => Data.Inventory;
    public bool HasItem(string itemId, int qty = 1) => InventoryService.Has(itemId, qty);
}
