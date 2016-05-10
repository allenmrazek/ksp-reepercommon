using UnityEngine;
using UnityEngine.EventSystems;

namespace ReeperCommon.GUIComponents
{
    [DisallowMultipleComponent, RequireComponent(typeof(RectTransform))]
    public class RectTransformDraggableHandle : MonoBehaviour, IPointerDownHandler, IDragHandler
    {
        // ReSharper disable once ConvertToConstant.Local
        // ReSharper disable once FieldCanBeMadeReadOnly.Local
        [SerializeField] private bool _clampToContainer = true;

        private RectTransform _resizedTarget;
        private RectTransform _container;
        private Vector2 _dragBegin;
        private Vector3 _originalLocalPosition;

        private void Awake()
        {
            _resizedTarget = GetComponent<RectTransform>();
        }

        public void OnPointerDown(PointerEventData eventData)
        {
            if (_resizedTarget == null) return;

            _container = _resizedTarget.transform.parent as RectTransform;
            _originalLocalPosition = _resizedTarget.localPosition;

            RectTransformUtility.ScreenPointToLocalPointInRectangle(_resizedTarget, eventData.position,
                eventData.pressEventCamera, out _dragBegin);

            print("Draggable.OnPointerDown");
        }


        public void OnDrag(PointerEventData eventData)
        {
            print("Draggable.OnDrag");

            Vector2 currentMousePosition;

            if (!RectTransformUtility.ScreenPointToLocalPointInRectangle(_resizedTarget, eventData.position,
                eventData.pressEventCamera, out currentMousePosition)) return;

            Vector3 offset = currentMousePosition - _dragBegin;

            print("Offset: " + offset);

            _resizedTarget.localPosition = _clampToContainer && _container != null
                ? ClampToContainer(_originalLocalPosition + offset)
                : (_originalLocalPosition + offset);
        }


        private Vector3 ClampToContainer(Vector3 pos)
        {
            print("Container.rect.min: " + _container.rect.max);
            print("Container.rect.max: " + _container.rect.max);
            print("Our min: " + _resizedTarget.rect.min);
            print("Our max: " + _resizedTarget.rect.max);

            var minPosition = _container.rect.min - _resizedTarget.rect.min;
            var maxPosition = _container.rect.max - _resizedTarget.rect.max;

            pos.x = Mathf.Clamp(pos.x, minPosition.x, maxPosition.x);
            pos.y = Mathf.Clamp(pos.y, maxPosition.y, maxPosition.y);

            print("Clamped pos: " + pos);

            return pos;
        }
    }
}
