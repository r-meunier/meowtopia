using System;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class UserData {
    public string UserId;
    public string Username;
    public string Email;

    // Two-currency wallet
    public int Coins;
    public int Gems;

    // MVP = single cat. Keep CatId nullable for future multi-cat.
    public string CatId;

    // Achievements and social reserved for later
    public List<string> Achievements = new();

    // Inventory (stack-based)
    public List<InventoryStack> Inventory = new();

    // Client-side like dedupe (optional)
    public List<string> LikedUserIds = new();
}

[Serializable]
public class InventoryStack {
    public string ItemId;
    public int Quantity;

    public InventoryStack() { }
    public InventoryStack(string itemId, int qty) { ItemId = itemId; Quantity = qty; }
}

[Serializable]
public class CatData {
    public string CatId;
    public string Name;
    public string Breed;

    // Use Unix seconds to avoid DateTime serialization issues
    public long BirthUnix;

    // Progression
    public int Experience;
    public int Level;

    // Appearance
    public string BaseColor;
    public string PatternType;
    public string PatternColor;
    public string EyeColor;
    public string AccessoryId;

    // Needs / stats (0..100)
    public float Hunger;
    public float Energy;
    public float Happiness;
    public float Hygiene;
    public float Health;
    public float Comfort;

    // Traits (0..10) – reserved for later AI/personality
    public int Playfulness;
    public int Independence;
    public int Curiosity;
    public int Affection;

    // Time bookkeeping for offline decay
    public long LastUpdateUnix;
}

[Serializable]
public class GameSettings {
    public bool SoundEnabled = true;
    public bool MusicEnabled = true;
    public bool NotificationsEnabled = true;
    public float MasterVolume = 1.0f;
    public string Language = "English";
}

[Serializable]
public class GameSaveData {
    public UserData User = new UserData();
    public CatData Cat;              // MVP single cat
    public GameSettings Settings = new GameSettings();
    public long LastSaveUnix;        // Unix seconds
    public int SaveVersion = 1;      // bump when schema changes
}
