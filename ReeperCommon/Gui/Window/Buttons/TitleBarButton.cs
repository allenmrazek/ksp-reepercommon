using System;
using ReeperCommon.Gui.Window.Decorators;
using UnityEngine;

namespace ReeperCommon.Gui.Window.Buttons
{
    public class TitleBarButton
    {
        public TitleBarButton(GUIStyle style, Texture texture, TitleBarButtons.ButtonCallback callback, string name)
        {
            if (style == null) throw new ArgumentNullException("style");
            if (texture == null) throw new ArgumentNullException("texture");
            if (callback == null) throw new ArgumentNullException("callback");

            Style = style;
            Texture = texture;
            Callback = callback;
            Name = name;
            Size = new Vector2(texture.width, texture.height);
        }



        public TitleBarButtons.ButtonCallback Callback { get; private set;}
        public string Name { get; private set;}
        public GUIStyle Style { get; private set; }
        public Texture Texture { get; set; }
        public Vector2 Size { get; set; }
    }
}