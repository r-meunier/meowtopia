using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Image))]
public class MeowtopiaAnimatedCat : MonoBehaviour
{
    public Sprite[] idleFrames;
    public Texture2D idleSheet;
    public bool previewClosedEyes;
    [SerializeField, Range(0, .02f)] private float breathingAmount = .006f;
    private Image image;
    private RectTransform rect;
    private Vector3 baseScale;
    private Quaternion baseRotation;
    private MeowtopiaEyeOverlay[] eyes;
    private MeowtopiaGazeEye[] gazeEyes;
    private RectTransform lookTarget;
    private Vector2 currentGaze;
    public Vector2 Gaze => currentGaze;
    public RectTransform LookTarget => lookTarget;
    private int reaction = -1;
    private float elapsed, nextBlink = 3, blinkTime = -1, reactionTime;
    private bool ownsSprites;

    private void Awake()
    {
        image = GetComponent<Image>(); rect = GetComponent<RectTransform>();
        // Preserve the rectangle while moving the breathing pivot toward the paws.
        var min = rect.offsetMin; var max = rect.offsetMax;
        rect.pivot = new Vector2(.5f, .05f); rect.offsetMin = min; rect.offsetMax = max;
        baseScale = rect.localScale; baseRotation = rect.localRotation;
        if (idleSheet != null && (idleFrames == null || idleFrames.Length == 0))
        {
            ownsSprites = true; idleFrames = new Sprite[8];
            var width = idleSheet.width / 4; var height = idleSheet.height / 2;
            for (int i = 0; i < 8; i++)
                idleFrames[i] = Sprite.Create(idleSheet, new Rect((i % 4) * width, (1 - i / 4) * height, width, height), new Vector2(.5f, .5f), 100, 0, SpriteMeshType.FullRect);
        }
        if (idleFrames != null && idleFrames.Length > 0) image.sprite = idleFrames[0];
        if (idleSheet != null)
        {
            gazeEyes = new[] {
                GazeEye("Left tracking eye", new Rect(187, 161, 64, 64), new Vector2(219, 194), new Vector2(23, 25)),
                GazeEye("Right tracking eye", new Rect(266, 165, 62, 62), new Vector2(297, 196), new Vector2(22, 23))
            };
            eyes = new[] { Eye("Left eyelid", new Rect(187, 161, 64, 64)), Eye("Right eyelid", new Rect(266, 165, 62, 62)) };
            UpdateEyePositions();
        }
    }

    private MeowtopiaGazeEye GazeEye(string name, Rect region, Vector2 centre, Vector2 radius)
    {
        var go = new GameObject(name, typeof(RectTransform)); go.transform.SetParent(transform, false);
        var eye = go.AddComponent<MeowtopiaGazeEye>(); eye.raycastTarget = false;
        eye.Configure(idleSheet, region, centre, radius); return eye;
    }

    private MeowtopiaEyeOverlay Eye(string name, Rect region)
    {
        var go = new GameObject(name, typeof(RectTransform)); go.transform.SetParent(transform, false);
        var eye = go.AddComponent<MeowtopiaEyeOverlay>();
        eye.sheet = idleSheet; eye.region = region; eye.raycastTarget = false;
        eye.color = new Color(1, 1, 1, 0); return eye;
    }

    private void UpdateEyePositions()
    {
        if (eyes == null || image.sprite == null) return;
        var bounds = rect.rect; var size = image.sprite.rect.size;
        var scale = Mathf.Min(bounds.width / size.x, bounds.height / size.y);
        var left = bounds.xMin + (bounds.width - size.x * scale) * rect.pivot.x;
        var bottom = bounds.yMin + (bounds.height - size.y * scale) * rect.pivot.y;
        for (int i = 0; i < eyes.Length; i++)
        {
            var eye = eyes[i];
            var r = eye.region; var eyeRect = eye.rectTransform;
            eyeRect.anchorMin = eyeRect.anchorMax = rect.pivot;
            eyeRect.sizeDelta = r.size * scale;
            eyeRect.anchoredPosition = new Vector2(left + r.center.x * scale, bottom + (size.y - r.center.y) * scale);
            if (gazeEyes != null)
            {
                var gazeRect = gazeEyes[i].rectTransform;
                gazeRect.anchorMin = gazeRect.anchorMax = eyeRect.anchorMin;
                gazeRect.sizeDelta = eyeRect.sizeDelta; gazeRect.anchoredPosition = eyeRect.anchoredPosition;
            }
        }
    }

    private void Update()
    {
        // Keep one stable body pose. The generated cells have inconsistent baselines.
        elapsed += Time.deltaTime;
        var breath = (Mathf.Sin(elapsed * Mathf.PI * .5f) + 1) * .5f;
        var response = reactionTime > 0 ? Mathf.Sin((1 - Mathf.Min(reactionTime, 1)) * Mathf.PI) * .008f : 0;
        rect.localScale = Vector3.Scale(baseScale, new Vector3(1 + breath * breathingAmount * .2f + response, 1 + breath * breathingAmount + response, 1));
        rect.localRotation = baseRotation;
        reactionTime = Mathf.Max(0, reactionTime - Time.deltaTime);
        nextBlink -= Time.deltaTime;
        if (nextBlink <= 0 && blinkTime < 0) { blinkTime = 0; nextBlink = Random.Range(3, 5); }
        float closed = 0;
        if (blinkTime >= 0)
        {
            blinkTime += Time.deltaTime;
            closed = blinkTime < .08f ? blinkTime / .08f : blinkTime < .16f ? 1 : 1 - (blinkTime - .16f) / .10f;
            if (blinkTime >= .26f) blinkTime = -1;
        }
        if (reaction == 3 && reactionTime > 0) closed = Mathf.Min(1, reactionTime / .15f);
        if (previewClosedEyes) closed = 1;
        if (eyes != null) foreach (var eye in eyes) eye.color = new Color(1, 1, 1, Mathf.Clamp01(closed));
        UpdateEyePositions();
        var desiredGaze = Vector2.zero;
        if (lookTarget != null && gazeEyes != null)
        {
            var eyeCentre = (gazeEyes[0].rectTransform.anchoredPosition + gazeEyes[1].rectTransform.anchoredPosition) * .5f;
            var target = (Vector2)rect.InverseTransformPoint(lookTarget.position);
            var delta = target - eyeCentre;
            desiredGaze = Vector2.ClampMagnitude(new Vector2(delta.x / Mathf.Max(1, rect.rect.width * .3f), delta.y / Mathf.Max(1, rect.rect.height * .3f)), 1);
        }
        currentGaze = Vector2.Lerp(currentGaze, desiredGaze, 1 - Mathf.Exp(-12 * Time.deltaTime));
        if (gazeEyes != null) foreach (var eye in gazeEyes) { eye.SetGaze(currentGaze); eye.color = new Color(1, 1, 1, 1 - Mathf.Clamp01(closed)); }
    }

    public void React(int action) { reaction = action; reactionTime = action == 3 ? 2 : 1; }
    public void SetLookTarget(RectTransform target) { lookTarget = target; }
    private void OnDestroy()
    {
        if (ownsSprites && idleFrames != null) foreach (var sprite in idleFrames) if (sprite != null) Destroy(sprite);
    }
}
