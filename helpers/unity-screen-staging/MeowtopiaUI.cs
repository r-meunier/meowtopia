using TMPro;
using UnityEngine;
using UnityEngine.UI;

public static class MeowtopiaUI
{
    public static readonly Color Ink = new Color32(57, 65, 57, 255);
    public static readonly Color Cream = new Color32(247, 244, 234, 255);
    public static readonly Color Green = new Color32(106, 143, 112, 255);

    public static RectTransform Rect(Transform parent, string name, Vector2 min, Vector2 max)
    {
        var go = new GameObject(name, typeof(RectTransform));
        go.transform.SetParent(parent, false);
        var rect = go.GetComponent<RectTransform>();
        rect.anchorMin = min; rect.anchorMax = max;
        rect.offsetMin = rect.offsetMax = Vector2.zero;
        return rect;
    }

    public static Image Panel(Transform parent, string name, Vector2 min, Vector2 max, Color color)
    {
        var image = Rect(parent, name, min, max).gameObject.AddComponent<Image>();
        image.color = color; image.raycastTarget = false;
        return image;
    }

    public static TextMeshProUGUI Text(Transform parent, string name, string text, Vector2 min, Vector2 max, float size)
    {
        var label = Rect(parent, name, min, max).gameObject.AddComponent<TextMeshProUGUI>();
        label.font = Resources.Load<TMP_FontAsset>("Fonts & Materials/LiberationSans SDF") ?? TMP_Settings.defaultFontAsset;
        label.text = text; label.fontSize = size; label.color = Ink;
        label.alignment = TextAlignmentOptions.Center;
        label.enableAutoSizing = true; label.fontSizeMin = size * .65f; label.fontSizeMax = size;
        label.raycastTarget = false;
        return label;
    }

    public static Button Button(Transform parent, string name, Vector2 min, Vector2 max)
    {
        var image = Panel(parent, name, min, max, Green);
        image.raycastTarget = true;
        var button = image.gameObject.AddComponent<Button>(); button.targetGraphic = image;
        var colors = button.colors;
        colors.highlightedColor = new Color(.88f, 1f, .88f);
        colors.pressedColor = new Color(.7f, .85f, .7f);
        button.colors = colors;
        Text(image.transform, "Label", name, new Vector2(.05f, .1f), new Vector2(.95f, .9f), 24).color = Color.white;
        return button;
    }

    public static Image Bar(Transform parent, string name, Vector2 min, Vector2 max)
    {
        var background = Panel(parent, name, min, max, new Color32(225, 231, 220, 255));
        return Panel(background.transform, "Fill", Vector2.zero, Vector2.one, Green);
    }

    public static void SetBar(Image fill, float value)
    {
        fill.rectTransform.anchorMax = new Vector2(Mathf.Clamp01(value), 1);
    }

    public static Canvas Canvas()
    {
        var go = new GameObject("Screen Canvas", typeof(RectTransform), typeof(Canvas), typeof(CanvasScaler), typeof(GraphicRaycaster));
        var canvas = go.GetComponent<Canvas>(); canvas.renderMode = RenderMode.ScreenSpaceOverlay;
        var scaler = go.GetComponent<CanvasScaler>(); scaler.uiScaleMode = CanvasScaler.ScaleMode.ScaleWithScreenSize;
        scaler.referenceResolution = new Vector2(960, 720); scaler.matchWidthOrHeight = .5f;
        Panel(go.transform, "Background", Vector2.zero, Vector2.one, Cream);
        return canvas;
    }
}
