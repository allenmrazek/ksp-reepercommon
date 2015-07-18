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


        public Vector2 HotzoneSize { get; set; }
        public Vector2 MinSize { get; set; }

        public Texture2D Hint { get; set; }
        public float HintPopupDelay { get; set; }
        public Vector2 Scale { get; set; }


        private ActiveMode _mode = ActiveMode.None;

        private Rect _rightRect = default(Rect);        // hotzone for changing width
        private Rect _bottomRect = default(Rect);       // hotzone for changing height
        private IEnumerator _dragging;
        private float _delayAccumulator = 0f;
        

        public Resizable(
            IWindowComponent baseComponent, 
            Vector2 hotzoneSize, 
            Vector2 minSize, 
            Texture2D hint, 
            float hintPopupDelay,
            Vector2 scale) : base(baseComponent)
        {
            if (baseComponent == null) throw new ArgumentNullException("baseComponent");
            if (hint == null) throw new ArgumentNullException("hint");

            HotzoneSize = hotzoneSize;
            MinSize = minSize;
            Hint = hint;
            HintPopupDelay = hintPopupDelay;
            Scale = scale;
        }


        public Resizable(
            IWindowComponent baseComponent,
            Vector2 hotzoneSize,
            Vector2 minSize,
            Texture2D hint,
            float hintPopupDelay = 0.25f) : this(baseComponent, hotzoneSize, minSize, hint, hintPopupDelay, Vector2.one)
        {
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
            _rightRect = new Rect(Dimensions.width - HotzoneSize.x, 
                0f,
                HotzoneSize.x, 
                Dimensions.height);

            _bottomRect = new Rect(0f, 
                Dimensions.height - HotzoneSize.y, 
                Dimensions.width,
                HotzoneSize.y);

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

            UpdateHotzoneRects();

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
            if (_delayAccumulator < HintPopupDelay)
                return;

            var mouseLocal =
                GUIUtility.ScreenToGUIPoint(new Vector2(Input.mousePosition.x, Screen.height - Input.mousePosition.y));

            var pos = new Vector2(Input.mousePosition.x, Screen.height - Input.mousePosition.y);
            var rect = GUIUtility.ScreenToGUIRect(new Rect(pos.x - Hint.width * 0.5f, pos.y - Hint.height * 0.5f, Hint.width, Hint.height));
            var originalMatrix = GUI.matrix;
            var cursorAngle = GetCursorAngle(mouseLocal);
            var pivot = new Vector2(rect.x + Hint.width*0.5f, rect.y + Hint.height*0.5f);

            // note: this is a fix for blur caused by rotation > 0
            if (!Mathf.Approximately(0f, cursorAngle))
            {
                var mat = originalMatrix;

                var m = mat.GetRow(0);
                m.w += 0.5f;
                mat.SetRow(0, m);

                m = mat.GetRow(1);
                m.w += 0.5f;
                mat.SetRow(1, m);

                GUI.matrix = mat;
            }

            GUIUtility.RotateAroundPivot(cursorAngle, pivot);
            GUIUtility.ScaleAroundPivot(Scale, pivot);

            Graphics.DrawTexture(rect, Hint);
            GUI.matrix = originalMatrix;
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


        public override void OnUpdate()
        {
            base.OnUpdate();
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
                    Mathf.Max(MinSize.x, newWidth),
                    Mathf.Max(MinSize.y, newHeight));

                yield return 0;
            } while (Input.GetMouseButton(0) && !Input.GetKeyDown(KeyCode.Escape));

            _mode = ActiveMode.None;
            _dragging = null;
        }
    }
}
