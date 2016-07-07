using ReeperCommon.Logging;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace ReeperCommon.UI
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
        private RectTransform _rectTransform;

        private void OnEnable()
        {
            _rectTransform = (RectTransform) transform;
        }


        public void OnPointerDown(PointerEventData eventData)
        {
            if (_resizableTarget == null) return;

            _originalSizeDelta = _resizableTarget.sizeDelta;

            RectTransformUtility.ScreenPointToLocalPointInRectangle(_resizableTarget, eventData.position,
                eventData.pressEventCamera, out _dragStart);
        }


        public void OnDrag(PointerEventData eventData)
        {
            if (_resizableTarget == null) return;

            Vector2 currentPointerLocation;

            RectTransformUtility.ScreenPointToLocalPointInRectangle(_resizableTarget, eventData.position,
                eventData.pressEventCamera, out currentPointerLocation);

            var offset = currentPointerLocation - _dragStart;

            Vector2 sizeDelta = _originalSizeDelta + new Vector2(offset.x, -offset.y);

            sizeDelta = new Vector2(
                Mathf.Clamp(sizeDelta.x, MinSize.x, MaxSize.x),
                Mathf.Clamp(sizeDelta.y, MinSize.y, MaxSize.y)
            );

            _resizableTarget.sizeDelta = sizeDelta;
        }
    }
}
