using System;

public static class EventBus {
    // Wallet
    public static Action<int, int> OnWalletChanged; // coins, gems

    // Inventory
    public static Action<string, int> OnInventoryChanged; // itemId, qty

    // Cat stats HUD refresh
    public static Action OnCatStatsChanged;
}
