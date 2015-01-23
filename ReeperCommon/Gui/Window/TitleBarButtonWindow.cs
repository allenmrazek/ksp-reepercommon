using System;
using System.Collections.Generic;
using System.Linq;
using ReeperCommon.Extensions;
using ReeperCommon.Gui.Window.Buttons;
using ReeperCommon.Gui.Window.Decorators;
using ReeperCommon.Gui.Window.Logic;
using ReeperCommon.Logging;
using ReeperCommon.Logging.Implementations;
using UnityEngine;

namespace ReeperCommon.Gui.Window
{
    public class TitleBarButtonWindow : BasicWindow
    {
        public delegate void ButtonCallback(string buttonName);

        private readonly List<TitleBarButton> _buttons = new List<TitleBarButton>();
        private readonly ILog _log;
        private readonly Vector2 _offset = Vector2.zero;



        public TitleBarButtonWindow(
            ILog log,
            IWindowLogic logic, 
            Rect rect,
            int winid,
            GUISkin skin,
            bool draggable = true,
            bool clamp = true,
            Vector2 offset = default(Vector2)) : base(logic, rect, winid, skin, draggable, clamp)
        {
            if (logic == null) throw new ArgumentNullException("logic");
            if (skin == null) throw new ArgumentNullException("skin");

            _log = log;
            this._offset = offset;
        }


        protected override void Draw(int winid)
        {
            base.Draw(winid);
            DrawTitleBarButtons();
        }
        

        private void DrawTitleBarButtons()
        {
            GUILayout.BeginArea(new Rect(_offset.x, _offset.y, _windowRect.width, _windowRect.height));
            GUILayout.BeginHorizontal();

            _buttons.ForEach(button =>
            {
                if (GUILayout.Button(button.Texture, button.Style, GUILayout.MaxWidth(button.Size.x), GUILayout.MinWidth(button.Size.x),
                    GUILayout.MaxHeight(button.Size.y),
                    GUILayout.MinHeight(button.Size.y)))
                    button.Callback(button.Name);
            });

            GUILayout.EndHorizontal();
            GUILayout.EndArea();

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
