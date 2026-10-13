using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class HomeCanvasGenerator : MonoBehaviour {
    // Colors based on reference image
    private Color darkBgColor = new Color(0.05f, 0.1f, 0.05f); // Very dark green/black
    private Color neonGreen = new Color(0.2f, 1.0f, 0.2f);
    private Color darkBarBg = new Color(0.1f, 0.2f, 0.1f);
    private Color panelColor = new Color(0, 0, 0, 0.5f);

    // Assets
    private Sprite circleSprite;
    private Sprite roundedRectSprite;
    private TMP_FontAsset fontAsset;

    void Start() {
        // Create procedural sprites
        circleSprite = CreateCircleSprite();
        roundedRectSprite = CreateRoundedRectSprite();

        // Try to find font
        fontAsset = Resources.Load<TMP_FontAsset>("LiberationSans SDF"); // Default usually
        if (fontAsset == null) {
            // Fallback search
            var allFonts = Resources.FindObjectsOfTypeAll<TMP_FontAsset>();
            if (allFonts.Length > 0) fontAsset = allFonts[0];
        }

        BuildUI();
    }

    void BuildUI() {
        // 1. Find Canvas
        Canvas canvas = FindObjectOfType<Canvas>();
        if (canvas == null) {
            GameObject go = new GameObject("Canvas");
            canvas = go.AddComponent<Canvas>();
            canvas.renderMode = RenderMode.ScreenSpaceOverlay;
            go.AddComponent<CanvasScaler>();
            go.AddComponent<GraphicRaycaster>();
        }

        // 2. Clear existing UI (Safely)
        // We iterate backwards to destroy
        for (int i = canvas.transform.childCount - 1; i >= 0; i--) {
            Transform child = canvas.transform.GetChild(i);
            // Don't destroy the Camera or Gameplay object if they are somehow under Canvas (unlikely but safe check)
            if (child.GetComponent<NeedsTicker>() == null) {
                Destroy(child.gameObject);
            }
        }

        // 3. Set Background
        GameObject bg = CreateImage(canvas.transform, "Background", darkBgColor, Vector2.zero, Vector2.zero, Vector2.one, Vector2.one);
        bg.transform.SetAsFirstSibling();

        // 4. Create Structure
        HomeUIController controller = canvas.gameObject.AddComponent<HomeUIController>();

        // --- Top Bar ---
        CreateTopBar(canvas.transform, controller);

        // --- Stats Bars (Top Left/Center) ---
        CreateStatsBars(canvas.transform, controller);

        // --- Cat Center ---
        CreateCatArea(canvas.transform);

        // --- Bottom Navigation ---
        CreateBottomNav(canvas.transform, controller);
    }

    void CreateTopBar(Transform parent, HomeUIController controller) {
        GameObject panel = CreatePanel(parent, "TopBar", new Vector2(0, 1), new Vector2(1, 1), new Vector2(0, -150));

        // Coin Container (Top Leftish)
        GameObject coinBg = CreateImage(panel.transform, "CoinBadge", new Color(0.1f, 0.1f, 0.1f), new Vector2(0, 1), new Vector2(0, 1), new Vector2(200, 60));
        RectTransform rt = coinBg.GetComponent<RectTransform>();
        rt.anchoredPosition = new Vector2(120, -60);
        // Rounded corners
        coinBg.GetComponent<Image>().sprite = roundedRectSprite;

        // Coin Icon (Yellow Circle)
        GameObject icon = CreateImage(coinBg.transform, "Icon", Color.yellow, new Vector2(0, 0.5f), new Vector2(0, 0.5f), new Vector2(40, 40));
        icon.GetComponent<RectTransform>().anchoredPosition = new Vector2(30, 0);
        icon.GetComponent<Image>().sprite = circleSprite;
        // Text inside icon "$"
        CreateText(icon.transform, "$", 24, Color.black).GetComponent<RectTransform>().anchoredPosition = Vector2.zero;

        // Coin Text
        GameObject textObj = CreateText(coinBg.transform, "0", 28, Color.white);
        controller.coinsText = textObj.GetComponent<TextMeshProUGUI>();
        RectTransform textRT = textObj.GetComponent<RectTransform>();
        textRT.anchorMin = new Vector2(0, 0);
        textRT.anchorMax = new Vector2(1, 1);
        textRT.offsetMin = new Vector2(60, 0);
        textRT.offsetMax = new Vector2(0, 0);
        controller.coinsText.alignment = TextAlignmentOptions.MidlineLeft;

        // Settings Button (Top Right)
        GameObject settingsBtn = CreateImage(panel.transform, "Settings", new Color(0.2f, 0.2f, 0.2f), new Vector2(1, 1), new Vector2(1, 1), new Vector2(60, 60));
        settingsBtn.GetComponent<RectTransform>().anchoredPosition = new Vector2(-60, -60);
        settingsBtn.GetComponent<Image>().sprite = circleSprite;
        // Gear icon placeholder (Text)
        CreateText(settingsBtn.transform, "*", 40, Color.white);
    }

    void CreateStatsBars(Transform parent, HomeUIController controller) {
        GameObject panel = CreatePanel(parent, "StatsPanel", new Vector2(0, 1), new Vector2(1, 1), new Vector2(0, -200));
        RectTransform rt = panel.GetComponent<RectTransform>();
        rt.anchoredPosition = new Vector2(0, -140); // Below top bar
        rt.sizeDelta = new Vector2(0, 200);

        float startY = -30;
        float gap = 60;

        controller.hungerSlider = CreateStatRow(panel.transform, "Hunger", neonGreen, new Vector2(50, startY));
        controller.energySlider = CreateStatRow(panel.transform, "Energy", neonGreen, new Vector2(50, startY - gap)); // Energy is shorter in ref image but we'll make standard
        // Modify Energy bar length if we want to match reference exactly (shorter bar)
        // Reference: Hunger (Long), Energy (Short), Happiness (Medium/Long)

        controller.happinessSlider = CreateStatRow(panel.transform, "Happiness", neonGreen, new Vector2(50, startY - gap * 2));
    }

    Slider CreateStatRow(Transform parent, string name, Color color, Vector2 pos) {
        // Icon
        GameObject icon = CreateImage(parent, name + "Icon", color, new Vector2(0, 1), new Vector2(0, 1), new Vector2(30, 30));
        RectTransform iconRT = icon.GetComponent<RectTransform>();
        iconRT.anchoredPosition = new Vector2(pos.x, pos.y);

        // Bar Background
        GameObject barBg = CreateImage(parent, name + "BarBg", darkBarBg, new Vector2(0, 1), new Vector2(0, 1), new Vector2(300, 20)); // Width 300
        RectTransform bgRT = barBg.GetComponent<RectTransform>();
        bgRT.anchoredPosition = new Vector2(pos.x + 180, pos.y); // Offset x
        barBg.GetComponent<Image>().sprite = roundedRectSprite;

        // Fill Area
        GameObject fillArea = new GameObject("Fill Area");
        fillArea.transform.SetParent(barBg.transform, false);
        RectTransform fillAreaRT = fillArea.AddComponent<RectTransform>();
        fillAreaRT.anchorMin = Vector2.zero;
        fillAreaRT.anchorMax = Vector2.one;
        fillAreaRT.sizeDelta = Vector2.zero;

        // Fill
        GameObject fill = CreateImage(fillArea.transform, "Fill", color, Vector2.zero, Vector2.zero, Vector2.zero);
        RectTransform fillRT = fill.GetComponent<RectTransform>();
        fillRT.anchorMin = Vector2.zero;
        fillRT.anchorMax = Vector2.one;
        fill.GetComponent<Image>().sprite = roundedRectSprite;

        // Slider Component
        Slider slider = barBg.AddComponent<Slider>();
        slider.fillRect = fillRT;
        slider.targetGraphic = fill.GetComponent<Image>();
        slider.direction = Slider.Direction.LeftToRight;
        slider.minValue = 0;
        slider.maxValue = 1;

        return slider;
    }

    void CreateCatArea(Transform parent) {
        GameObject panel = CreatePanel(parent, "CatArea", new Vector2(0.5f, 0.5f), new Vector2(0.5f, 0.5f), new Vector2(600, 600));

        // Try to load existing cat sprite
        // Guid found: eaa41b6eb1d10604e9755ca0bf5ec8a5 -> Assets/Art/Pets/PetsPack/PNG/Cat/Orange-Cat.png
        // We can't use AssetDatabase in runtime/build, but we can try Resources if it's there.
        // It's likely not in Resources. We will fallback to a placeholder or try to find the Sprite in the scene before we destroyed it?
        // Too late, we destroyed children.
        // Strategy: Just create a nice big circle/square placeholder with "CAT" text if we can't load.

        GameObject catObj = CreateImage(panel.transform, "CatImage", Color.white, new Vector2(0.5f, 0.5f), new Vector2(0.5f, 0.5f), new Vector2(400, 400));

        // Attempt to load from Resources (Unlikely to work unless moved)
        // So we use a placeholder color (Light Grey/Blue like reference)
        catObj.GetComponent<Image>().color = new Color(0.6f, 0.6f, 0.7f);
        catObj.GetComponent<Image>().sprite = roundedRectSprite;

        // "Meow!" Bubble
        GameObject bubble = CreateImage(panel.transform, "Bubble", new Color(0.1f, 0.1f, 0.1f, 0.8f), new Vector2(0.5f, 1), new Vector2(0.5f, 1), new Vector2(150, 50));
        bubble.GetComponent<RectTransform>().anchoredPosition = new Vector2(0, -50); // Above cat
        bubble.GetComponent<Image>().sprite = roundedRectSprite;
        // Outline for bubble
        bubble.GetComponent<Image>().color = darkBgColor;
        // Green border
        GameObject bubbleBorder = CreateImage(bubble.transform, "Border", Color.clear, new Vector2(0,0), new Vector2(1,1), Vector2.zero);
        bubbleBorder.GetComponent<RectTransform>().offsetMin = new Vector2(-2,-2);
        bubbleBorder.GetComponent<RectTransform>().offsetMax = new Vector2(2,2);
        bubbleBorder.transform.SetAsFirstSibling(); // Behind
        // Actually borders are hard with simple Images. Let's just use Text.

        CreateText(bubble.transform, "Meow!", 24, Color.white).GetComponent<RectTransform>().anchoredPosition = Vector2.zero;
    }

    void CreateBottomNav(Transform parent, HomeUIController controller) {
        GameObject panel = CreatePanel(parent, "NavBar", new Vector2(0, 0), new Vector2(1, 0), new Vector2(0, 250)); // Height 250 for gradient look

        // Gradient BG (simulated with dark panel)
        GameObject bg = CreateImage(panel.transform, "Gradient", new Color(0, 0, 0, 0.8f), new Vector2(0, 0), new Vector2(1, 1), Vector2.zero);

        // Buttons: Play, Feed, Home(Center Big), Shop, Care
        float btnWidth = 100;
        float spacing = 50;

        // We arrange them: -2, -1, 0, 1, 2
        CreateNavButton(panel.transform, "Play", -2, false, null);
        CreateNavButton(panel.transform, "Feed", -1, false, null); // Controller handles wiring

        // Home Button (Big Green)
        CreateNavButton(panel.transform, "Home", 0, true, null);

        CreateNavButton(panel.transform, "Shop", 1, false, null);
        CreateNavButton(panel.transform, "Care", 2, false, null);

        // Find the Feed button we just made and assign it to controller
        // The CreateNavButton for "Feed" doesn't return the button but we can find it.
        Transform feedTrans = panel.transform.Find("Feed");
        if (feedTrans) controller.feedButton = feedTrans.GetComponent<Button>();
    }

    void CreateNavButton(Transform parent, string label, int index, bool isCenter, UnityEngine.Events.UnityAction action) {
        GameObject btnObj = new GameObject(label);
        btnObj.transform.SetParent(parent, false);

        RectTransform rt = btnObj.AddComponent<RectTransform>();

        float xOffset = index * 150; // Spacing
        float yOffset = 50;

        if (isCenter) {
            rt.sizeDelta = new Vector2(120, 120);
            rt.anchoredPosition = new Vector2(0, 80); // Popping up

            // Circle Green
            Image img = btnObj.AddComponent<Image>();
            img.color = neonGreen;
            img.sprite = circleSprite;

            // Home Icon inside (Black house)
            GameObject icon = CreateText(btnObj.transform, "H", 60, Color.black);
        } else {
            rt.sizeDelta = new Vector2(80, 80);
            rt.anchoredPosition = new Vector2(xOffset, 50);

            Image img = btnObj.AddComponent<Image>();
            img.color = Color.clear; // Invisible hit box mostly

            // Icon (Text for now)
            GameObject iconObj = CreateText(btnObj.transform, label.Substring(0,1), 40, Color.gray);
            // Label
            GameObject labelObj = CreateText(btnObj.transform, label, 20, Color.gray);
            labelObj.GetComponent<RectTransform>().anchoredPosition = new Vector2(0, -30);
        }

        Button btn = btnObj.AddComponent<Button>();
        if (action != null) {
            btn.onClick.AddListener(action);
        }
    }

    // --- Helpers ---

    GameObject CreatePanel(Transform parent, string name, Vector2 anchorMin, Vector2 anchorMax, Vector2 sizeDelta) {
        GameObject go = new GameObject(name);
        go.transform.SetParent(parent, false);
        RectTransform rt = go.AddComponent<RectTransform>();
        rt.anchorMin = anchorMin;
        rt.anchorMax = anchorMax;
        rt.anchoredPosition = Vector2.zero;
        if (sizeDelta != Vector2.zero) rt.sizeDelta = sizeDelta;
        else {
             rt.offsetMin = Vector2.zero;
             rt.offsetMax = Vector2.zero;
        }
        return go;
    }

    GameObject CreateImage(Transform parent, string name, Color color, Vector2 anchorMin, Vector2 anchorMax, Vector2 sizeDelta) {
        GameObject go = new GameObject(name);
        go.transform.SetParent(parent, false);
        RectTransform rt = go.AddComponent<RectTransform>();
        rt.anchorMin = anchorMin;
        rt.anchorMax = anchorMax;
        rt.anchoredPosition = Vector2.zero;
        if (sizeDelta != Vector2.zero) rt.sizeDelta = sizeDelta;
        else {
             rt.offsetMin = Vector2.zero;
             rt.offsetMax = Vector2.zero;
        }
        Image img = go.AddComponent<Image>();
        img.color = color;
        return go;
    }

    GameObject CreateText(Transform parent, string content, float size, Color color) {
        GameObject go = new GameObject("Text");
        go.transform.SetParent(parent, false);
        RectTransform rt = go.AddComponent<RectTransform>();
        rt.anchorMin = Vector2.zero;
        rt.anchorMax = Vector2.one;
        rt.offsetMin = Vector2.zero;
        rt.offsetMax = Vector2.zero;

        TextMeshProUGUI tmp = go.AddComponent<TextMeshProUGUI>();
        tmp.text = content;
        tmp.fontSize = size;
        tmp.color = color;
        tmp.alignment = TextAlignmentOptions.Center;
        if (fontAsset != null) tmp.font = fontAsset;

        return go;
    }

    Sprite CreateCircleSprite() {
        Texture2D tex = new Texture2D(64, 64);
        Vector2 center = new Vector2(32, 32);
        for(int y=0; y<64; y++) {
            for(int x=0; x<64; x++) {
                float dist = Vector2.Distance(new Vector2(x,y), center);
                if(dist < 30) tex.SetPixel(x,y, Color.white);
                else tex.SetPixel(x,y, Color.clear);
            }
        }
        tex.Apply();
        return Sprite.Create(tex, new Rect(0,0,64,64), new Vector2(0.5f, 0.5f));
    }

    Sprite CreateRoundedRectSprite() {
        Texture2D tex = new Texture2D(64, 64);
        Color[] colors = new Color[64*64];
        for(int i=0; i<colors.Length; i++) colors[i] = Color.white;
        tex.SetPixels(colors);
        tex.Apply();
        return Sprite.Create(tex, new Rect(0,0,64,64), new Vector2(0.5f, 0.5f));
    }
}
