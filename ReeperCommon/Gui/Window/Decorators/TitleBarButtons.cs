using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ReeperCommon.Extensions;
using ReeperCommon.Gui.Window.Decorators.Buttons;
using ReeperCommon.Logging;
using UnityEngine;

namespace ReeperCommon.Gui.Window.Decorators
{
    public class TitleBarButtons : WindowDecorator
    {
        public delegate void ButtonCallback(string buttonName);

        private Rect _barRect = new Rect();
        private readonly List<TitleBarButton> _buttons = new List<TitleBarButton>();
        private readonly Vector2 _offset = Vector2.zero;



        public TitleBarButtons(IWindowComponent baseComponent, Vector2 offset = default(Vector2)) : base(baseComponent)
        {
            if (baseComponent == null) throw new ArgumentNullException("baseComponent");

            this._offset = offset;
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
                if (GUILayout.Button(button.Texture, GUILayout.MaxWidth(16f), GUILayout.MinWidth(16f),
                    GUILayout.MaxHeight(16f),
                    GUILayout.MinHeight(16f)))
                    button.Callback(button.Name);

                ////if (GUILayout.Button(button.Texture, button.Style, GUILayout.MaxWidth(16f), GUILayout.MinWidth(16f),
                ////    GUILayout.MaxHeight(16f),
                ////    GUILayout.MinHeight(16f)))
                ////    button.Callback(button.Name);
            });

            GUILayout.EndHorizontal();
            GUILayout.EndArea();

            base.OnPostWindowDraw();
        }



        public void AddButton(TitleBarButton button)
        {
            if (button == null) throw new ArgumentNullException("button");

            if (_buttons.Contains(button))
                throw new InvalidOperationException("TitleBar already contains " + button.Name);

            _buttons.Add(button);
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
