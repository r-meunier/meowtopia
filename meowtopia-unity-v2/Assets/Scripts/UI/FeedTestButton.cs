// Assets/Scripts/UI/FeedTestButton.cs
using UnityEngine;

public class FeedTestButton : MonoBehaviour {
    public string itemId = "food_tuna";
    public void Feed() {
        if (InteractionService.UseItemOnCat(itemId))
            UIManager.Instance?.ShowNotification("Fed tuna! +20 Hunger");
        else
            UIManager.Instance?.ShowNotification("No tuna left!");
    }
}
