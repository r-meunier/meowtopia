using UnityEngine;
using UnityEngine.EventSystems;

public class MeowtopiaDraggableToy : MonoBehaviour, IPointerDownHandler, IPointerUpHandler, IBeginDragHandler, IDragHandler, IEndDragHandler
{
    public RectTransform playArea;
    public bool IsDragging { get; private set; }
    public bool PointerInside { get; private set; }
    private int pointerId;
    private RectTransform Rect => (RectTransform)transform;

    public void OnPointerDown(PointerEventData data)
    {
        if (IsDragging) return;
        pointerId = data.pointerId; IsDragging = true; PointerInside = true;
    }
    public void OnBeginDrag(PointerEventData data) { OnPointerDown(data); }
    public void OnDrag(PointerEventData data)
    {
        if (!IsDragging || data.pointerId != pointerId || playArea == null) return;
        if (!RectTransformUtility.ScreenPointToLocalPointInRectangle(playArea, data.position, data.pressEventCamera, out var point)) return;
        var bounds = playArea.rect; var margin = Rect.sizeDelta * .5f;
        PointerInside = bounds.Contains(point);
        point.x = Mathf.Clamp(point.x, bounds.xMin + margin.x, bounds.xMax - margin.x);
        point.y = Mathf.Clamp(point.y, bounds.yMin + margin.y, bounds.yMax - margin.y);
        Rect.anchoredPosition = point;
    }
    public void OnPointerUp(PointerEventData data) { if (data.pointerId == pointerId) Cancel(); }
    public void OnEndDrag(PointerEventData data) { OnPointerUp(data); }
    public void Cancel() { IsDragging = false; PointerInside = false; }
    private void OnDisable() { Cancel(); }
}
