using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class PassiveScrollRect : ScrollRect
{
    public enum DragStates
    {
        None,
        Dragging,
        End
    }

    private DragStates state = DragStates.None;
    private DirectionalScrollRouter parent;
    public bool inControl = false;


    public override void OnScroll(PointerEventData data)
    {
        if (vertical && !horizontal)
        {
            base.OnScroll(data);
        }
        else
        {
            transform.parent?.GetComponentInParent<ScrollRect>()?.OnScroll(data);
        }
    }

    public override void OnBeginDrag(PointerEventData eventData)
    {
        GetParent()?.OnBeginDrag(eventData);
    }

    public void ProcessOnBeginDrag(PointerEventData eventData)
    {
        if (state == DragStates.None)
        {
            state = DragStates.Dragging;
            base.OnBeginDrag(eventData);
        }
    }

    public override void OnDrag(PointerEventData eventData)
    {
        GetParent()?.OnDrag(eventData);
    }

    public void ProcessOnDrag(PointerEventData eventData)
    {
        if (state == DragStates.None)
        {
            GetParent()?.OnDrag(eventData);
        }

        if (state == DragStates.Dragging)
        {
            base.OnDrag(eventData);
        }
    }

    public override void OnEndDrag(PointerEventData eventData)
    {
        GetParent()?.OnEndDrag(eventData);
    }

    public void ProcessOnEndDrag(PointerEventData eventData, bool force = false)
    {
        if (state == DragStates.Dragging)
        {
            state = DragStates.End;
        }

        if (state == DragStates.End || force)
        {
            state = DragStates.None;
            base.OnEndDrag(eventData);
        }
    }

    public DirectionalScrollRouter GetParent()
    {
        if (parent == null)
        {
            parent = GetComponentInParent<DirectionalScrollRouter>();
        }

        return parent;
    }
}
