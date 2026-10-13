// Assets/Scripts/Gameplay/InteractionService.cs
using UnityEngine;

public static class InteractionService {
    // Hook this up from your Pantry UI “Use” button
    public static bool UseItemOnCat(string itemId) {
        // TODO: look up item definition by id (SO catalog). For now, hardcode demo effects.
        if (!InventoryService.Consume(itemId, 1)) return false;

        var cat = SaveSystem.Current.Cat;
        if (cat == null) return false;

        // Example effects – replace with data-driven SO
        switch (itemId) {
            case "food_tuna":
                cat.Hunger = Mathf.Clamp(cat.Hunger + 25f, 0, 100);
                cat.Happiness = Mathf.Clamp(cat.Happiness + 5f, 0, 100);
                break;
            case "toy_ball":
                cat.Happiness = Mathf.Clamp(cat.Happiness + 20f, 0, 100);
                cat.Energy = Mathf.Clamp(cat.Energy - 5f, 0, 100);
                break;
            case "care_brush":
                cat.Hygiene = Mathf.Clamp(cat.Hygiene + 20f, 0, 100);
                cat.Comfort = Mathf.Clamp(cat.Comfort + 5f, 0, 100); // if you add Comfort later
                break;
        }

        SaveSystem.MarkDirtyAndMaybeAutosave();
        // Trigger animation/sfx via an event or a CatController
        return true;
    }
}
