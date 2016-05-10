using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using UnityEngine.EventSystems;

namespace ReeperCommon.GUIComponents
{
    [DisallowMultipleComponent, RequireComponent(typeof(RectTransform))]
    class RectTransformResizerHandle : MonoBehaviour, IPointerDownHandler, IDragHandler
    {
        public Vector2 MinSize = new Vector2(100f, 100f);
        public Vector2 MaxSize = new Vector2(400f, 400f);

#pragma warning disable 649
        [SerializeField] private  RectTransform _resizableTarget;
#pragma warning restore 649

        private Vector2 _dragStart;
        private Vector2 _originalSizeDelta;

        private void Awake()
        {
            print("RectTransformResizerHandle.Awake");
        }
        public void OnPointerDown(PointerEventData eventData)
        {
            print("OnPointerDown");
            if (_resizableTarget == null) return;

            _originalSizeDelta = _resizableTarget.sizeDelta;
            print("Original size delta: " + _originalSizeDelta);
            print("Width: " + _resizableTarget.rect.width);
            print("Height: " + _resizableTarget.rect.height);

            RectTransformUtility.ScreenPointToLocalPointInRectangle(_resizableTarget, eventData.position,
                eventData.pressEventCamera, out _dragStart);

            print("drag start: " + _dragStart);
        }


        public void OnDrag(PointerEventData eventData)
        {
            print("OnDrag");
            if (_resizableTarget == null) return;

            Vector2 currentPointerLocation;

            RectTransformUtility.ScreenPointToLocalPointInRectangle(_resizableTarget, eventData.position,
                eventData.pressEventCamera, out currentPointerLocation);

            print("Current mouse: " + currentPointerLocation);
            print("Drag start: " + _dragStart);

            var offset = currentPointerLocation - _dragStart;

            Vector2 sizeDelta = _originalSizeDelta + new Vector2(offset.x, -offset.y);
            sizeDelta = new Vector2(
                Mathf.Clamp(sizeDelta.x, MinSize.x, MaxSize.x),
                Mathf.Clamp(sizeDelta.y, MinSize.y, MaxSize.y)
            );

            _resizableTarget.sizeDelta = sizeDelta;
            print("Current SizeDelta: " + _originalSizeDelta);
            print("New SizeDelta: " + sizeDelta);

            //print("SizeDelta: " + _resizableTarget.sizeDelta);
        }
    }
}
