using UnityEngine;
using UnityEngine.UI;

// Feathered closed-eye patches animate independently of the stable body pose.
[RequireComponent(typeof(CanvasRenderer))]
public class MeowtopiaEyeOverlay : MaskableGraphic
{
    public Texture2D sheet;
    public Rect region;
    public override Texture mainTexture => sheet != null ? sheet : Texture2D.whiteTexture;

    protected override void OnPopulateMesh(VertexHelper mesh)
    {
        mesh.Clear(); if (sheet == null) return;
        var bounds = GetPixelAdjustedRect(); const int divisions = 8;
        var cellWidth = sheet.width / 4f;
        for (int y = 0; y <= divisions; y++) for (int x = 0; x <= divisions; x++)
        {
            float u = x / (float)divisions, v = y / (float)divisions;
            var tint = color;
            tint.a *= Mathf.Clamp01(Mathf.Min(Mathf.Min(u, 1 - u), Mathf.Min(v, 1 - v)) / .125f);
            var uv = new Vector2((3 * cellWidth + region.x + u * region.width) / sheet.width,
                (sheet.height - region.y - region.height + v * region.height) / sheet.height);
            mesh.AddVert(new Vector3(bounds.xMin + u * bounds.width, bounds.yMin + v * bounds.height), tint, uv);
        }
        for (int y = 0; y < divisions; y++) for (int x = 0; x < divisions; x++)
        {
            int index = y * (divisions + 1) + x;
            mesh.AddTriangle(index, index + divisions + 1, index + 1);
            mesh.AddTriangle(index + 1, index + divisions + 1, index + divisions + 2);
        }
    }
}
