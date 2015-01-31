using System;
using System.Collections.Generic;
using ReeperCommon.Extensions;
using ReeperCommon.Extensions.Object;
using ReeperCommon.Gui.Window.Buttons;
using UnityEngine;

namespace ReeperCommon.Gui.Window.Decorators
{
    public class TitleBarButtons : WindowDecorator
    {
        private readonly List<TitleBarButton> _buttons = new List<TitleBarButton>();
        private readonly ButtonAlignment _alignment;
        private readonly Vector2 _offset = Vector2.zero;

        public enum ButtonAlignment
        {
            Center,
            Left,
            Right
        }



        public TitleBarButtons(
            IWindowComponent window,
            ButtonAlignment alignment = ButtonAlignment.Right,
            Vector2 offset = default(Vector2)) : base(window)
        {
            if (window == null) throw new ArgumentNullException("window");

            _alignment = alignment;
            this._offset = offset;
        }



        public override void OnWindowDraw(int winid)
        {
            base.OnWindowDraw(winid);
            DrawTitleBarButtons();
        }



        private void DrawTitleBarButtons()
        {
            GUILayout.BeginArea(new Rect(_offset.x, _offset.y, Dimensions.width - _offset.x * 2f, Dimensions.height));

                GUILayout.BeginHorizontal();
                {

                    if (_alignment != ButtonAlignment.Left)
                        GUILayout.FlexibleSpace();

                    _buttons.ForEach(DrawButton);

                    if (_alignment != ButtonAlignment.Right)
                        GUILayout.FlexibleSpace();

                }
                GUILayout.EndHorizontal();
            GUILayout.EndArea();

        }



        private void DrawButton(TitleBarButton button)
        {    
            //if (GUILayout.Button(button.Texture, button.Style.IsNull() ? GUI.skin.button : button.Style, GUILayout.MaxWidth(button.Size.x),
            //    GUILayout.MinWidth(button.Size.x),
            //    GUILayout.MaxHeight(button.Size.y),
            //    GUILayout.MinHeight(button.Size.y),
            //    GUILayout.ExpandWidth(false),
            //    GUILayout.ExpandHeight(false)))
            if (GUILayout.Button(button.Texture, button.Style.IsNull() ? GUI.skin.button : button.Style,
                GUILayout.ExpandWidth(false),
                GUILayout.ExpandHeight(false)))
                    button.Callback(button.Name);
        }



        public void AddButton(TitleBarButton button)
        {
            if (button == null) throw new ArgumentNullException("button");

            if (_buttons.Contains(button))
                throw new InvalidOperationException("TitleBar already contains " + button.Name);

            _buttons.Add(button);
        }

    }
}
