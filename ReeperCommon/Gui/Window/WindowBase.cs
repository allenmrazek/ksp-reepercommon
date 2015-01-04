using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ReeperCommon.Extensions;
using ReeperCommon.Gui.Window.Decorators;
using UnityEngine;

namespace ReeperCommon.Gui.Window
{
    public class WindowBase : WindowComponent
    {
        public WindowBase(Rect rect, int winid, GUISkin skin)
        {
            if (skin == null) throw new ArgumentNullException("skin");

            Id = winid;
            Dimensions = rect;
        }

        public override void OnPreWindowDraw()
        {

        }

        public override void OnWindowDraw()
        {
            
            Dimensions = GUILayout.Window(Id, Dimensions, Draw, Title);
            
        }

        public override void OnPostWindowDraw()
        {

        }

        public override void Update()
        {
            
        }

        public override Rect Dimensions { get; set; }
        public override string Title { get; set; }
        public override GUISkin Skin { get; set; }

        protected virtual void Draw(int winid)
        {
            if (!Skin.IsNull()) GUI.skin = Skin;

            GUILayout.BeginVertical();

            GUILayout.Label("Test window here");

            GUILayout.EndVertical();
        }

        
    }
}
