using System;
using UnityEngine;

namespace ReeperCommon.Gui.Window.Decorators
{
    class TitleBarButton
    {
        public TitleBarButton(GUIStyle style, TitleBarButtons.ButtonCallback callback, string name)
        {
            if (style == null) throw new ArgumentNullException("style");
            if (callback == null) throw new ArgumentNullException("callback");

            Style = style;
            Callback = callback;
            Name = name;
        }



        public TitleBarButtons.ButtonCallback Callback { get; private set;}
        public string Name { get; private set;}
        public GUIStyle Style { get; private set; }
    }
}