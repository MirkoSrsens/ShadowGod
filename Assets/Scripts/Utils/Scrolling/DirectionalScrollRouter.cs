using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class DirectionalScrollRouter : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler
{
    public PassiveScrollRect verticalScroll;

    private PassiveScrollRect activeHorizontalScroll;
    private Vector2 dragStart;
    private bool isHorizontal;

    public void OnBeginDrag(PointerEventData eventData)
    {
        dragStart = eventData.position;
        isHorizontal = false;

        // Raycast to find the horizontal ScrollRect under the pointer
        activeHorizontalScroll = GetHorizontalUnderPointer(eventData);
    }

    public void OnDrag(PointerEventData eventData)
    {
        Vector2 delta = eventData.delta; // eventData.position - dragStart;

        if (delta.x == 0 && delta.y == 0)
        {
            return;
        }

        isHorizontal = Mathf.Abs(delta.x) > Mathf.Abs(delta.y);

        if (isHorizontal && activeHorizontalScroll != null)
        {
            activeHorizontalScroll.ProcessOnBeginDrag(eventData);
            activeHorizontalScroll.ProcessOnDrag(eventData);
            verticalScroll.ProcessOnEndDrag(eventData, true);
        }
        else
        {
            verticalScroll.ProcessOnBeginDrag(eventData);
            verticalScroll.ProcessOnDrag(eventData);
            if (activeHorizontalScroll != null)
            {
                activeHorizontalScroll.ProcessOnEndDrag(eventData, true);
            }
        }
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        if (activeHorizontalScroll != null)
        {
            activeHorizontalScroll.ProcessOnEndDrag(eventData);
        }
        verticalScroll.ProcessOnEndDrag(eventData);
    }

    private PassiveScrollRect GetHorizontalUnderPointer(PointerEventData eventData)
    {
        var results = new List<RaycastResult>();
        EventSystem.current.RaycastAll(eventData, results);

        foreach (var result in results)
        {
            var scroll = result.gameObject.GetComponentInParent<PassiveScrollRect>();
            if (scroll != null && scroll.horizontal && !scroll.vertical)
                return scroll;
        }
        return null;
    }
}
