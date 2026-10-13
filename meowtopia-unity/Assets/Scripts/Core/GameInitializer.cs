using UnityEngine;
using UnityEngine.SceneManagement;

public class GameInitializer : MonoBehaviour {
    [SerializeField] int starterCoins = 500;
    [SerializeField] int starterGems = 20;

    private void Awake() {
        DontDestroyOnLoad(gameObject);
        SaveSystem.LoadOrCreate(); // loads into SaveSystem.Current
        var save = SaveSystem.Current;

        // First-time seed
        if (string.IsNullOrEmpty(save.User.UserId)) {
            save.User.UserId = System.Guid.NewGuid().ToString();
            save.User.Username = "Player";
            save.User.Coins = starterCoins;
            save.User.Gems = starterGems;

            // seed inventory
            InventoryService.Add("food_tuna", 3);
            InventoryService.Add("toy_ball", 1);
            InventoryService.Add("care_brush", 1);

            SaveSystem.Write();
        }

        // Route
        if (save.Cat == null || string.IsNullOrEmpty(save.Cat.CatId))
            SceneManager.LoadSceneAsync(SceneNames.CreateCat);
        else
            SceneManager.LoadSceneAsync(SceneNames.Home);
    }
}
