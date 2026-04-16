using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;

[ExecuteAlways]
[RequireComponent(typeof(CanvasRenderer))]
public class UILineRenderer : Graphic
{
    [SerializeField] private List<Vector2> points = new();
    [SerializeField] private float thickness = 2f;

    public List<Vector2> Points
    {
        get => points;
        set { points = value; SetVerticesDirty(); }
    }

    public float Thickness
    {
        get => thickness;
        set { thickness = value; SetVerticesDirty(); }
    }

    protected override void OnPopulateMesh(VertexHelper vh)
    {
        vh.Clear();
        if (isActiveAndEnabled == false) return;
        if (points.Count < 2) return;

        for (int i = 0; i < points.Count - 1; i++)
            DrawSegment(vh, points[i], points[i + 1], i);
    }

    private void DrawSegment(VertexHelper vh, Vector2 start, Vector2 end, int index)
    {
        Vector2 dir = (end - start).normalized;
        Vector2 normal = new Vector2(-dir.y, dir.x) * (thickness * 0.5f);

        int vertIndex = index * 4;

        vh.AddVert(start - normal, color, Vector2.zero);
        vh.AddVert(start + normal, color, Vector2.zero);
        vh.AddVert(end + normal, color, Vector2.zero);
        vh.AddVert(end - normal, color, Vector2.zero);

        vh.AddTriangle(vertIndex, vertIndex + 1, vertIndex + 2);
        vh.AddTriangle(vertIndex + 2, vertIndex + 3, vertIndex);
    }
}