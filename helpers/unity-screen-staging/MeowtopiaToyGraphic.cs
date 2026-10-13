using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(CanvasRenderer))]
public class MeowtopiaToyGraphic : MaskableGraphic
{
    protected override void OnPopulateMesh(VertexHelper mesh)
    {
        mesh.Clear(); var centre = rectTransform.rect.center;
        var radius = Mathf.Min(rectTransform.rect.width, rectTransform.rect.height) * .4f;
        var yarn = new Color32(222, 142, 112, 255);
        const int segments = 32;
        mesh.AddVert(centre, yarn, Vector2.zero);
        for (int i = 0; i <= segments; i++)
        {
            var angle = i * Mathf.PI * 2 / segments;
            mesh.AddVert(centre + new Vector2(Mathf.Cos(angle), Mathf.Sin(angle)) * radius, yarn, Vector2.zero);
            if (i > 0) mesh.AddTriangle(0, i, i + 1);
        }
        var thread = new Color32(255, 221, 194, 255);
        for (int row = -1; row <= 1; row++)
        {
            Vector2 previous = centre + new Vector2(-radius * .7f, row * radius * .45f);
            for (int i = 1; i <= 12; i++)
            {
                float x = Mathf.Lerp(-radius * .7f, radius * .7f, i / 12f);
                var point = centre + new Vector2(x, row * radius * .45f + Mathf.Sin(i * Mathf.PI / 12) * radius * .15f);
                Line(mesh, previous, point, 2, thread); previous = point;
            }
        }
    }
    private static void Line(VertexHelper mesh, Vector2 from, Vector2 to, float width, Color color)
    {
        var side = new Vector2(-(to - from).y, (to - from).x).normalized * width * .5f;
        int start = mesh.currentVertCount;
        mesh.AddVert(from - side, color, Vector2.zero); mesh.AddVert(from + side, color, Vector2.zero);
        mesh.AddVert(to + side, color, Vector2.zero); mesh.AddVert(to - side, color, Vector2.zero);
        mesh.AddTriangle(start, start + 1, start + 2); mesh.AddTriangle(start, start + 2, start + 3);
    }
}
