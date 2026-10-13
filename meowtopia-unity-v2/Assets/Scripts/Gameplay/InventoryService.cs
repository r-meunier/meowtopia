// Assets/Scripts/Gameplay/InventoryService.cs
using System.Linq;
using System.Collections.Generic;

public static class InventoryService {
    public static bool Has(string itemId, int qty = 1) {
        var inv = SaveSystem.Current.User.Inventory;
        var stack = inv.FirstOrDefault(s => s.ItemId == itemId);
        return stack != null && stack.Quantity >= qty;
    }

    public static bool Consume(string itemId, int qty = 1) {
        var inv = SaveSystem.Current.User.Inventory;
        var stack = inv.FirstOrDefault(s => s.ItemId == itemId);
        if (stack == null || stack.Quantity < qty) return false;
        stack.Quantity -= qty;
        if (stack.Quantity <= 0) inv.Remove(stack);
        SaveSystem.MarkDirtyAndMaybeAutosave();
        return true;
    }

    public static void Add(string itemId, int qty) {
        var inv = SaveSystem.Current.User.Inventory;
        var stack = inv.FirstOrDefault(s => s.ItemId == itemId);
        if (stack == null) inv.Add(new InventoryStack(itemId, qty));
        else stack.Quantity += qty;
        SaveSystem.MarkDirtyAndMaybeAutosave();
    }
}
