// Assets/Scripts/UI/PantryPanel.cs
using UnityEngine;
using TMPro;

public class PantryPanel : MonoBehaviour {
    [SerializeField] Transform listRoot;
    [SerializeField] GameObject rowPrefab; // a simple row with TMP for name/qty and a Use button

    void OnEnable() { Rebuild(); }

    public void Rebuild() {
        foreach (Transform t in listRoot) Destroy(t.gameObject);
        var inv = SaveSystem.Current.User.Inventory;
        foreach (var s in inv) {
            var row = Instantiate(rowPrefab, listRoot);
            var labels = row.GetComponentsInChildren<TextMeshProUGUI>();
            if (labels.Length > 0) labels[0].text = $"{s.ItemId}";
            if (labels.Length > 1) labels[1].text = $"x{s.Quantity}";

            var use = row.GetComponentInChildren<UnityEngine.UI.Button>();
            if (use != null) {
                string id = s.ItemId;
                use.onClick.AddListener(() => {
                    if (InteractionService.UseItemOnCat(id)) {
                        Rebuild(); // refresh qty
                    } else {
                        UIManager.Instance?.ShowNotification("Can't use item.");
                    }
                });
            }
        }
    }
}
