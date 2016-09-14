using UnityEngine;
using UnityEngine.EventSystems;
// ReSharper disable ConvertToConstant.Global
// ReSharper disable FieldCanBeMadeReadOnly.Global

namespace ReeperCommon.UI
{
    [DisallowMultipleComponent, RequireComponent(typeof(RectTransform))]
    // ReSharper disable once UnusedMember.Global
    public class RectTransformDraggable : MonoBehaviour, IPointerDownHandler, IDragHandler, IBeginDragHandler, IEndDragHandler
    {
        private RectTransform _resizedTarget;
        private RectTransform _parent;
        private Vector2 _dragBegin;
        private Vector3 _originalAnchoredPosition;

        public bool PropogateEvents = false;
        

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

            if (PropogateEvents)
                ExecuteEvents.ExecuteHierarchy(_parent.gameObject, eventData, ExecuteEvents.pointerDownHandler);
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

            if (PropogateEvents)
                ExecuteEvents.ExecuteHierarchy(_parent.gameObject, eventData, ExecuteEvents.dragHandler);
        }

        public void OnBeginDrag(PointerEventData eventData)
        {
            if (PropogateEvents && _parent)
                ExecuteEvents.ExecuteHierarchy(_parent.gameObject, eventData, ExecuteEvents.beginDragHandler);
        }

        public void OnEndDrag(PointerEventData eventData)
        {
            if (PropogateEvents && _parent)
                ExecuteEvents.ExecuteHierarchy(_parent.gameObject, eventData, ExecuteEvents.endDragHandler);
        }
    }
}
