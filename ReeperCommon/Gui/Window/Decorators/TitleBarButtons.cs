using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ReeperCommon.Extensions;
using ReeperCommon.Logging;
using UnityEngine;

namespace ReeperCommon.Gui.Window.Decorators
{
    public class TitleBarButtons : WindowDecorator
    {
        public delegate void ButtonCallback();

        private Rect _barRect = new Rect();
        private readonly List<TitleBarButton> _buttons = new List<TitleBarButton>();
        private readonly Vector2 _offset = Vector2.zero;

        public TitleBarButtons(IWindowComponent baseComponent, Vector2 offset = default(Vector2)) : base(baseComponent)
        {
            if (baseComponent == null) throw new ArgumentNullException("baseComponent");

            this._offset = offset;
        }

        ~TitleBarButtons()
        {

        }


        public override void OnPostWindowDraw()
        {
            _barRect.height = 20f;
            _barRect.x = Dimensions.x + _offset.x;
            _barRect.y = Dimensions.y + _offset.y;
            _barRect.width = Dimensions.width;

            GUILayout.BeginArea(_barRect);
            GUILayout.BeginHorizontal();

            _buttons.ForEach(button =>
            {
                GUILayout.Button("", button.Style, GUILayout.MaxWidth(16f), GUILayout.MinWidth(16f), GUILayout.MaxHeight(16f),
                    GUILayout.MinHeight(16f));
            });

            GUILayout.EndHorizontal();
            GUILayout.EndArea();

            base.OnPostWindowDraw();
        }



        public void AddButton(GUIStyle style, ButtonCallback callback, string name = "")
        {
            if (style == null) throw new ArgumentNullException("style");
            if (callback == null) throw new ArgumentNullException("callback");

            _buttons.Add(new TitleBarButton(style, callback, name));
        }


        public void RemoveButton(string name)
        {
            if (string.IsNullOrEmpty(name)) throw new InvalidOperationException("invalid name");

            var button = _buttons.FirstOrDefault(b => b.Name == name);
            if (!button.IsNull())
                _buttons.Remove(button);
            else throw new InvalidOperationException(name + " not found");
        }
    }
}
