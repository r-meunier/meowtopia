using TMPro;
using UnityEngine;
using UnityEngine.UI;

// Reflows the existing screens; gameplay and serialized scene references stay intact.
[DisallowMultipleComponent]
public sealed class MeowtopiaMobileLayout : MonoBehaviour
{
    public RectTransform SafeRoot { get; private set; }
    private Canvas canvas;
    private CanvasScaler scaler;
    private MeowtopiaHomeScreen home;
    private MeowtopiaLoadingScreen loading;
    private RectTransform brand, card, menu, tagline, cat, progress;
    private RectTransform lastPlayArea;
    private Vector2 lastSize;
    private Vector2 lastRootSize;
    private Rect lastSafeArea;

    public static void Attach(MeowtopiaHomeScreen screen)
    {
        var layout = Attach(screen.catNameText.canvas);
        layout.home = screen;
        layout.Cache();
        layout.RefreshLayout();
    }

    public static void Attach(MeowtopiaLoadingScreen screen)
    {
        var layout = Attach(screen.statusText.canvas);
        layout.loading = screen;
        layout.Cache();
        layout.RefreshLayout();
    }

    private static MeowtopiaMobileLayout Attach(Canvas canvas)
    {
        var layout = canvas.GetComponent<MeowtopiaMobileLayout>() ?? canvas.gameObject.AddComponent<MeowtopiaMobileLayout>();
        layout.canvas = canvas;
        layout.scaler = canvas.GetComponent<CanvasScaler>();
        if (layout.SafeRoot == null)
        {
            layout.SafeRoot = MeowtopiaUI.Rect(canvas.transform, "Safe area", Vector2.zero, Vector2.one);
            // Leave the background edge-to-edge, with interactive content inside the safe area.
            for (int i = canvas.transform.childCount - 1; i >= 0; i--)
            {
                var child = canvas.transform.GetChild(i);
                if (child != layout.SafeRoot && child.name != "Background")
                {
                    child.SetParent(layout.SafeRoot, false);
                    child.SetAsFirstSibling();
                }
            }
        }
        if (Application.isMobilePlatform) Application.targetFrameRate = 60;
        return layout;
    }

    private void Cache()
    {
        brand = (RectTransform)SafeRoot.Find("Brand");
        if (home != null)
        {
            card = (RectTransform)home.catNameText.transform.parent;
            cat = (RectTransform)home.catAnimation.transform;
            menu = (RectTransform)home.menuPanel.transform.Find("Menu panel");
        }
        else
        {
            tagline = (RectTransform)SafeRoot.Find("Tagline");
            cat = (RectTransform)SafeRoot.Find("Cat illustration");
            progress = (RectTransform)loading.progressFill.transform.parent;
        }
    }

    private void LateUpdate()
    {
        var size = new Vector2(Screen.width, Screen.height);
        var area = home != null && home.PlaySession != null ? home.PlaySession.Area : null;
        if (size != lastSize || Screen.safeArea != lastSafeArea || area != lastPlayArea || SafeRoot.rect.size != lastRootSize) RefreshLayout();
    }

    public void RefreshLayout()
    {
        if (canvas == null || Screen.width <= 0 || Screen.height <= 0) return;
        lastSize = new Vector2(Screen.width, Screen.height);
        lastSafeArea = Screen.safeArea;
        ApplyLayout(lastSize, lastSafeArea);
    }

    // Explicit viewport inputs also allow Editor checks of notches and rotation.
    public void ApplyLayout(Vector2 pixels, Rect safeArea)
    {
        if (pixels.x <= 0 || pixels.y <= 0) return;
        bool portrait = safeArea.height >= safeArea.width * 1.4f;
        scaler.uiScaleMode = CanvasScaler.ScaleMode.ScaleWithScreenSize;
        scaler.referenceResolution = portrait ? new Vector2(390, 844) : new Vector2(Mathf.Min(960, pixels.x), 540);
        scaler.matchWidthOrHeight = 0;
        canvas.scaleFactor = pixels.x / scaler.referenceResolution.x;
        SafeRoot.anchorMin = new Vector2(safeArea.xMin / pixels.x, safeArea.yMin / pixels.y);
        SafeRoot.anchorMax = new Vector2(safeArea.xMax / pixels.x, safeArea.yMax / pixels.y);
        SafeRoot.offsetMin = SafeRoot.offsetMax = Vector2.zero;
        Canvas.ForceUpdateCanvases();
        float w = SafeRoot.rect.width, h = SafeRoot.rect.height;
        lastRootSize = SafeRoot.rect.size;
        Frame(brand, 16, h - 62, w - (home != null ? 90 : 32), 46);
        Font(brand, portrait ? 23 : 28);
        if (home != null) LayoutHome(w, h, portrait);
        else LayoutLoading(w, h);
    }

    private void LayoutHome(float w, float h, bool portrait)
    {
        bool compact = !portrait && h < 440;
        float buttonHeight = compact ? 48 : 56;
        float padding = compact ? 8 : 16;
        Frame(home.menuButton, w - (compact ? 60 : 64), h - (compact ? 52 : 62), compact ? 44 : 48, compact ? 44 : 48);
        if (compact) { Frame(brand, 16, h - 50, w - 90, 38); Font(brand, 22); }
        float trayHeight = portrait ? 122 : buttonHeight;
        for (int i = 0; i < 4; i++)
        {
            float buttonWidth = portrait ? (w - 42) / 2 : (w - 68) / 4;
            float x = 16 + (portrait ? i % 2 : i) * (buttonWidth + (portrait ? 10 : 12));
            float y = padding + (portrait && i < 2 ? 66 : 0);
            Frame(home.careButtons[i], x, y, buttonWidth, buttonHeight);
            Font(home.careButtons[i].GetComponentInChildren<TextMeshProUGUI>().rectTransform, 18);
        }
        float feedbackHeight = compact ? 28 : 40;
        Frame(home.feedbackText, 16, padding + trayHeight + 4, w - 32, feedbackHeight);
        Font(home.feedbackText.rectTransform, 14);
        float bottom = padding + trayHeight + 4 + feedbackHeight + 8;
        float height = h - bottom - (compact ? 60 : 76);
        Frame(card, 16, bottom, w - 32, height);
        float cw = w - 32;
        if (portrait)
        {
            Frame(home.catNameText, 12, height - 45, cw - 24, 36);
            Font(home.catNameText.rectTransform, 25);
            Frame(cat, 8, 128, cw - 16, height - 178);
            for (int i = 0; i < 4; i++)
            {
                float x = 16 + (i % 2) * (cw / 2);
                float y = i < 2 ? 72 : 18;
                Frame(home.needLabels[i], x, y + 12, cw / 2 - 28, 27);
                Font(home.needLabels[i].rectTransform, 14);
                Frame((RectTransform)home.needFills[i].transform.parent, x, y, cw / 2 - 28, 8);
            }
        }
        else
        {
            Frame(home.catNameText, 12, height - (compact ? 36 : 44), cw * .55f - 24, compact ? 28 : 36);
            Font(home.catNameText.rectTransform, compact ? 21 : 25);
            Frame(cat, 8, 8, cw * .55f - 16, height - (compact ? 44 : 54));
            for (int i = 0; i < 4; i++)
            {
                float y = height - 16 - (i + 1) * ((height - 24) / 4);
                Frame(home.needLabels[i], cw * .59f, y + (compact ? 10 : 13), cw * .37f, compact ? 20 : 27);
                Font(home.needLabels[i].rectTransform, compact ? 14 : 17);
                Frame((RectTransform)home.needFills[i].transform.parent, cw * .59f, y, cw * .37f, compact ? 6 : 8);
            }
        }
        var play = home.PlaySession;
        lastPlayArea = play != null ? play.Area : null;
        if (play != null)
        {
            if (play.IsActive) play.End("Screen resized. Tap Play to continue.");
            Frame(play.Area, cat.anchoredPosition.x - cat.rect.width * cat.pivot.x,
                cat.anchoredPosition.y - cat.rect.height * cat.pivot.y, cat.rect.width, cat.rect.height);
            ((RectTransform)play.Toy.transform).sizeDelta = new Vector2(64, 64);
        }
        float menuHeight = Mathf.Min(280, h - 16);
        Frame(menu, (w - Mathf.Min(340, w - 32)) / 2, (h - menuHeight) / 2, Mathf.Min(340, w - 32), menuHeight);
        float mw = menu.rect.width;
        float menuButtonHeight = menuHeight < 260 ? 48 : 56;
        Frame(home.saveMenuButton, 20, 36 + menuButtonHeight, mw - 40, menuButtonHeight);
        Frame(home.closeMenuButton, 20, 20, mw - 40, menuButtonHeight);
        Frame(menu.Find("Title"), 20, menuHeight - 60, mw - 40, 36);
    }

    private void LayoutLoading(float w, float h)
    {
        Frame(brand, 16, h * .79f, w - 32, 48);
        Frame(tagline, 16, h * .73f, w - 32, 36);
        Font(tagline, 17);
        Frame(cat, 32, h * .32f, w - 64, h * .36f);
        Frame(progress, 32, h * .27f, w - 64, 10);
        Frame(loading.statusText, 24, h * .16f, w - 48, 60);
        Font(loading.statusText.rectTransform, 16);
        Frame(loading.retryButton, 32, 24, w - 64, 56);
    }

    private static void Font(RectTransform rect, float size)
    {
        var text = rect.GetComponent<TextMeshProUGUI>();
        text.fontSizeMin = size * .9f;
        text.fontSizeMax = text.fontSize = size;
    }

    private static void Frame(Component component, float x, float y, float width, float height)
    {
        var rect = (RectTransform)component.transform;
        rect.anchorMin = rect.anchorMax = Vector2.zero;
        rect.sizeDelta = new Vector2(Mathf.Max(0, width), Mathf.Max(0, height));
        rect.anchoredPosition = new Vector2(x + width * rect.pivot.x, y + height * rect.pivot.y);
    }
}
