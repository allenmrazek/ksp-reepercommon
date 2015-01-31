using System;
using ReeperCommon.Gui.Window.Decorators;
using UnityEngine;

namespace ReeperCommon.Gui.Window.Buttons
{
    public delegate void ButtonCallback(string buttonName);

    public class TitleBarButton
    {
        public TitleBarButton(GUIStyle style, Texture texture, ButtonCallback callback, string name):this(texture, callback, name)
        {
            if (style == null) throw new ArgumentNullException("style");

            Style = style;
        }


        public TitleBarButton(Texture texture, ButtonCallback callback, string name)
        {
            if (texture == null) throw new ArgumentNullException("texture");
            if (callback == null) throw new ArgumentNullException("callback");
            if (name == null) throw new ArgumentNullException("name");

            Texture = texture;
            Callback = callback;
            Name = name;
            Size = new Vector2(texture.width, texture.height);
        }


        public ButtonCallback Callback { get; private set;}
        public string Name { get; private set;}
        public GUIStyle Style { get; private set; }
        public Texture Texture { get; set; }
        public Vector2 Size { get; set; }
    }
}