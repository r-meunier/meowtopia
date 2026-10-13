using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(CanvasRenderer))]
public class MeowtopiaGazeEye : MaskableGraphic
{
    private Vector2 gaze;
    private Texture2D sheet;
    private Rect sourceRegion;
    private Vector2 irisCentre, irisRadius;
    public override Texture mainTexture => sheet != null ? sheet : Texture2D.whiteTexture;

    public void Configure(Texture2D texture, Rect region, Vector2 centre, Vector2 radius)
    {
        sheet = texture; sourceRegion = region; irisCentre = centre; irisRadius = radius;
        SetAllDirty();
    }
    public void SetGaze(Vector2 value)
    {
        if ((value - gaze).sqrMagnitude < .000001f) return;
        gaze = Vector2.ClampMagnitude(value, 1); SetVerticesDirty();
    }
    protected override void OnPopulateMesh(VertexHelper mesh)
    {
        mesh.Clear(); if (sheet == null) return;
        var bounds = GetPixelAdjustedRect(); const int divisions = 24;
        var shift = Vector2.Scale(gaze, new Vector2(3.5f, 2.5f));
        for (int y = 0; y <= divisions; y++) for (int x = 0; x <= divisions; x++)
        {
            float u = x / (float)divisions, v = y / (float)divisions;
            var source = new Vector2(sourceRegion.x + u * sourceRegion.width, sourceRegion.y + (1 - v) * sourceRegion.height);
            var relative = source - irisCentre;
            float distance = new Vector2(relative.x / irisRadius.x, relative.y / irisRadius.y).magnitude;
            // The original outline and all surrounding fur have zero displacement.
            float influence = 1 - Mathf.SmoothStep(0, 1, Mathf.InverseLerp(.35f, .95f, distance));
            source.x -= shift.x * influence;
            source.y += shift.y * influence;
            var uv = new Vector2(source.x / sheet.width, (sheet.height - source.y) / sheet.height);
            mesh.AddVert(new Vector3(bounds.xMin + u * bounds.width, bounds.yMin + v * bounds.height), color, uv);
        }
        for (int y = 0; y < divisions; y++) for (int x = 0; x < divisions; x++)
        {
            int index = y * (divisions + 1) + x;
            mesh.AddTriangle(index, index + divisions + 1, index + 1);
            mesh.AddTriangle(index + 1, index + divisions + 1, index + divisions + 2);
        }
    }
}
