using System;
using System.Collections;
using ReeperCommon.Extensions;
using ReeperCommon.Logging;
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
        public Texture2D HintTexture { get; set; }
        public float HintPopupDelay { get; set; }
        public Vector2 HintScale { get; set; }


        private ActiveMode _mode = ActiveMode.None;

        private Rect _rightRect = default(Rect);        // hotzone for changing width
        private Rect _bottomRect = default(Rect);       // hotzone for changing height
        private IEnumerator _dragging;
        private float _delayAccumulator = 0f;
        

        public Resizable(
            IWindowComponent baseComponent, 
            Vector2 hotzoneSize, 
            Vector2 minSize, 
            Texture2D hintTexture, 
            float hintPopupDelay,
            Vector2 hintScale) : base(baseComponent)
        {
            if (baseComponent == null) throw new ArgumentNullException("baseComponent");
            if (hintTexture == null) throw new ArgumentNullException("hintTexture");

            HotzoneSize = hotzoneSize;
            MinSize = minSize;
            HintTexture = hintTexture;
            HintPopupDelay = hintPopupDelay;
            HintScale = hintScale;
        }


        public Resizable(
            IWindowComponent baseComponent,
            Vector2 hotzoneSize,
            Vector2 minSize,
            Texture2D hintTexture,
            float hintPopupDelay = 0.25f) : this(baseComponent, hotzoneSize, minSize, hintTexture, hintPopupDelay, Vector2.one)
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
            var hzScaledWidth = HotzoneSize.x * GUI.matrix.m00;
            var hzScaledHeight = HotzoneSize.y * GUI.matrix.m11;

            _rightRect = new Rect(Dimensions.width - hzScaledWidth,
                0f,
                hzScaledWidth,
                Dimensions.height);

            _bottomRect = new Rect(0f,
                Dimensions.height - hzScaledHeight,
                Dimensions.width,
                hzScaledHeight);
        }


        private void OnMouseDown()
        {
            UpdateHotzoneRects();

            if ((_mode = GetMouseMode()) != ActiveMode.None)
                Event.current.Use();
        }


        private ActiveMode GetMouseMode()
        {
            var m = ActiveMode.None;
            var log = new DebugLog("ActiveMode");

            if (_bottomRect.Contains(Event.current.mousePosition))
            {
                m = ActiveMode.Bottom;
                log.Normal("bottom rect contains");
            }

            if (_rightRect.Contains(Event.current.mousePosition))
            {
                m |= ActiveMode.Right;
                log.Normal("right rect contains");
            }

            return m;
        }


        private void OnMouseDrag()
        {
            if (_mode == ActiveMode.None) return;

            UpdateHotzoneRects();

            _dragging = UpdateMouseDrag(GUI.matrix);
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

            var pos = GetScreenPositionOfMouse();

            var texRect = new Rect(0f, 0f, HintTexture.width, HintTexture.height);
            texRect.center = GUIUtility.ScreenToGUIPoint(pos);
            Graphics.DrawTexture(texRect, HintTexture);

            //var guiMousePos = GUIUtility.ScreenToGUIPoint(pos); // note: this does take scale into effect...
            //var scaledHintDimensions = new Vector2(HintTexture.width * GUI.matrix.m00 * HintScale.x, HintTexture.height * GUI.matrix.m11 * HintScale.y);

            //var cursorAngle = GetCursorAngle();
            //cursorAngle = 45f;
            //var originalMatrix = GUI.matrix;

            //var hintTextureRect = new Rect(guiMousePos.x - HintTexture.width * 0.5f, guiMousePos.y - HintTexture.height * 0.5f,
            //    HintTexture.width, HintTexture.height);

            //var pivot =
            //    GUIUtility.GUIToScreenPoint(new Vector2(hintTextureRect.x + HintTexture.width * 0.5f,
            //        hintTextureRect.y + HintTexture.height * 0.5f));

            // note: this is a fix for blur caused by rotation > 0
            //if (!Mathf.Approximately(0f, cursorAngle))
            //{
            //    var mat = originalMatrix;

            //    var m = mat.GetRow(0);
            //    m.w += 0.5f;
            //    mat.SetRow(0, m);

            //    m = mat.GetRow(1);
            //    m.w += 0.5f;
            //    mat.SetRow(1, m);

            //    GUI.matrix = mat;
            //}


            //var log = new DebugLog("Resizable");

            //log.Normal("Correct: " +
            //           GUIUtility.GUIToScreenPoint(new Vector2(hintTextureRect.x + scaledHintDimensions.x*0.5f,
            //               hintTextureRect.y + scaledHintDimensions.y*0.5f)));

            //log.Normal("pos: " + pos);
            //log.Normal("input: " + Input.mousePosition);
            //log.Normal("guiMouse: " + guiMousePos);

            ////GUIUtility.RotateAroundPivot(cursorAngle, pivot);
           
            //GUIUtility.RotateAroundPivot(cursorAngle,
            //    GUIUtility.GUIToScreenPoint(new Vector2(hintTextureRect.x + scaledHintDimensions.x*0.5f,
            //        hintTextureRect.y + scaledHintDimensions.y*0.5f)));
            //GUIUtility.RotateAroundPivot(cursorAngle, GUIUtility.GUIToScreenPoint(hintTextureRect.center));

            //GUIUtility.ScaleAroundPivot(HintScale, guiMousePos);
            //hintTextureRect.center = new Vector2(100f, 200f);
            //Graphics.DrawTexture(hintTextureRect, HintTexture);
           

            //GUI.matrix = originalMatrix;
        }


        private float GetCursorAngle()
        {
            var currentMode = _dragging != null ? _mode : GetMouseMode();

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
            return _dragging != null || GetMouseMode() != ActiveMode.None;
        }


        public override void OnUpdate()
        {
            base.OnUpdate();
            if (_dragging != null)
                _dragging.MoveNext();
        }


        private IEnumerator UpdateMouseDrag(Matrix4x4 guiMatrix) // note: should GUI.matrix's scaling change while dragging, drag will break. So don't do that
        {
            do
            {
                // note: the user is dragging the scaled dimensions of the rect. We'll treat the current
                // coordinates as though they're dragging that scaled version and work backwards to come up
                // with a set of dimensions that would result in that size when scaled by GUI.matrix
                var mousePos = new Vector2(Input.mousePosition.x, Screen.height - Input.mousePosition.y);
                var visibleDimensions = Dimensions.Multiply(guiMatrix);

                var newWidth = (_mode & ActiveMode.Right) != 0 ? mousePos.x - visibleDimensions.x : visibleDimensions.width;
                var newHeight = (_mode & ActiveMode.Bottom) != 0 ? mousePos.y - visibleDimensions.y : visibleDimensions.height;

                
                Dimensions = new Rect(
                    Dimensions.x,
                    Dimensions.y,
                    Mathf.Max(MinSize.x, newWidth / guiMatrix.m00),
                    Mathf.Max(MinSize.y, newHeight / guiMatrix.m11));

                yield return 0;
            } while (Input.GetMouseButton(0) && !Input.GetKeyDown(KeyCode.Escape));

            _mode = ActiveMode.None;
            _dragging = null;
        }

        
        // in screen space (inverted y)
        private static Vector2 GetScreenPositionOfMouse()
        {
            return new Vector2(Input.mousePosition.x, Screen.height - Input.mousePosition.y);
        }
    }
}
