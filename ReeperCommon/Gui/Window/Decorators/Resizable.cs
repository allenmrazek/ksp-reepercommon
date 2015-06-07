using System;
using System.Collections;
using UnityEngine;

namespace ReeperCommon.Gui.Window.Decorators
{
    public class Resizable : WindowDecorator
    {
        [Flags]
        private enum ActiveMode
        {
            None = 0,
            Right = 1 << 0,
            Bottom = 1 << 1,
            Both = Right | Bottom
        }

        private readonly Vector2 _hotzoneSize = default(Vector2);
        private readonly Vector2 _minSize;
        private readonly Texture2D _hint;
        private readonly float _hintPopupDelay;
        private ActiveMode _mode = ActiveMode.None;

        private Rect _rightRect = default(Rect);        // hotzone for changing width
        private Rect _bottomRect = default(Rect);       // hotzone for changing height
        private IEnumerator _dragging;
        private float _delayAccumulator = 0f;


        public Resizable(IWindowComponent baseComponent, Vector2 hotzoneSize, Vector2 minSize, Texture2D hint, float hintPopupDelay = 0.25f) : base(baseComponent)
        {
            if (baseComponent == null) throw new ArgumentNullException("baseComponent");
            if (hint == null) throw new ArgumentNullException("hint");

            _hotzoneSize = hotzoneSize;
            _minSize = minSize;
            _hint = hint;
            _hintPopupDelay = hintPopupDelay;
        }


        public override void OnWindowDraw(int winid)
        {
            base.OnWindowDraw(winid);

            switch (Event.current.type)
            {
                case EventType.Repaint:
                    OnRepaint();
                    break;

                case EventType.MouseDown:
                    OnMouseDown();
                    break;

                case EventType.MouseDrag:
                    OnMouseDrag();
                    break;
            }

        }


        private void UpdateHotzoneRects()
        {
            _rightRect = new Rect(Dimensions.width - _hotzoneSize.x, 
                0f,
                _hotzoneSize.x, 
                Dimensions.height);

            _bottomRect = new Rect(0f, 
                Dimensions.height - _hotzoneSize.y, 
                Dimensions.width,
                _hotzoneSize.y);

        }


        private void OnMouseDown()
        {
            UpdateHotzoneRects();

            if ((_mode = GetMouseMode(Event.current.mousePosition)) != ActiveMode.None)
                Event.current.Use();
        }


        private ActiveMode GetMouseMode(Vector2 guiPos)
        {
            var m = ActiveMode.None;

            if (_bottomRect.Contains(guiPos))
                m = ActiveMode.Bottom;

            if (_rightRect.Contains(guiPos))
                m |= ActiveMode.Right;

            return m;
        }


        private void OnMouseDrag()
        {
            if (_mode == ActiveMode.None) return;

            _dragging = UpdateMouseDrag();
            Event.current.Use();
        }


        private void OnRepaint()
        {
            UpdateHotzoneRects();

            if (!ShouldShowCursor())
            {
                _delayAccumulator = 0f;
                return;
            }

            _delayAccumulator += Time.deltaTime;
            if (_delayAccumulator < _hintPopupDelay)
                return;

            var mouseLocal =
                GUIUtility.ScreenToGUIPoint(new Vector2(Input.mousePosition.x, Screen.height - Input.mousePosition.y));

            var pos = new Vector2(Input.mousePosition.x, Screen.height - Input.mousePosition.y);
            var rect = GUIUtility.ScreenToGUIRect(new Rect(pos.x - _hint.width * 0.5f, pos.y - _hint.height * 0.5f, _hint.width, _hint.height));
            var savedMatrix = GUI.matrix;

            GUIUtility.RotateAroundPivot(GetCursorAngle(mouseLocal), new Vector2(rect.x + _hint.width * 0.5f, rect.y + _hint.height * 0.5f));
            Graphics.DrawTexture(rect, _hint);
            GUI.matrix = savedMatrix;
        }


        private float GetCursorAngle(Vector2 guiPos)
        {
            var currentMode = _dragging != null ? _mode : GetMouseMode(guiPos);

            switch (currentMode)
            {
                case ActiveMode.Bottom:
                    return 90f;

                case ActiveMode.Right:
                    return 0f;

                case ActiveMode.Both:
                    return 45f;

                default:
                    return 0f;
            }
        }


        private bool ShouldShowCursor()
        {
            return _dragging != null || GetMouseMode(GUIUtility.ScreenToGUIPoint(new Vector2(Input.mousePosition.x, Screen.height - Input.mousePosition.y))) != ActiveMode.None;
        }


        public override void Update()
        {
            base.Update();
            if (_dragging != null)
                _dragging.MoveNext();
        }


        private IEnumerator UpdateMouseDrag()
        {
            do
            {
                var newWidth = (_mode & ActiveMode.Right) != 0 ? Input.mousePosition.x - Dimensions.x : Dimensions.width;
                var newHeight = (_mode & ActiveMode.Bottom) != 0 ? (Screen.height - Input.mousePosition.y) - Dimensions.y : Dimensions.height;

                Dimensions = new Rect(
                    Dimensions.x, 
                    Dimensions.y, 
                    Mathf.Max(_minSize.x, newWidth),
                    Mathf.Max(_minSize.y, newHeight));

                yield return 0;
            } while (Input.GetMouseButton(0) && !Input.GetKeyDown(KeyCode.Escape));

            _mode = ActiveMode.None;
            _dragging = null;
        }
    }
}
