using UnityEngine;
using UnityEngine.EventSystems;

namespace ReeperCommon.UI
{
    [DisallowMultipleComponent, RequireComponent(typeof(RectTransform))]
    public class RectTransformDraggable : MonoBehaviour, IPointerDownHandler, IDragHandler
    {
        private RectTransform _resizedTarget;
        private RectTransform _parent;
        private Vector2 _dragBegin;
        private Vector3 _originalAnchoredPosition;

        private void Awake()
        {
            _resizedTarget = GetComponent<RectTransform>();
        }

        public void OnPointerDown(PointerEventData eventData)
        {
            if (_resizedTarget == null) return;

            _parent = _resizedTarget.transform.parent as RectTransform;
            if (_parent == null) return;

            _originalAnchoredPosition = _resizedTarget.anchoredPosition;

            RectTransformUtility.ScreenPointToLocalPointInRectangle(_parent, eventData.position,
                eventData.pressEventCamera, out _dragBegin);
        }


        public void OnDrag(PointerEventData eventData)
        {
            if (_resizedTarget == null) return;
            if (_parent == null) return;

            Vector2 currentMousePosition;

            if (!RectTransformUtility.ScreenPointToLocalPointInRectangle(_parent, eventData.position,
                eventData.pressEventCamera, out currentMousePosition)) return;

            Vector3 offset = currentMousePosition - _dragBegin;

            _resizedTarget.anchoredPosition = _originalAnchoredPosition + offset;
        }
    }
}
