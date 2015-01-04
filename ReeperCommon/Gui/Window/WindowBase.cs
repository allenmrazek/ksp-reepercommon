using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ReeperCommon.Gui.Window.Decorators;
using UnityEngine;

namespace ReeperCommon.Gui.Window
{
    sealed class WindowBase : WindowComponent
    {
        public WindowBase(Rect rect, string title, int winid, GUISkin skin)
        {
            if (skin == null) throw new ArgumentNullException("skin");

            Dimensions = rect;
            Title = title;
            Id = winid;
            Skin = skin;
        }

        public override void OnPreGUI()
        {

        }

        public override void OnGUI()
        {
            OnPreGUI();
            Dimensions = GUILayout.Window(Id, Dimensions, Draw, Title);
            OnPostGUI();
        }

        public override void OnPostGUI()
        {

        }

        public override void Update()
        {
            
        }

        public override Rect Dimensions { get; set; }
        public override int Id { get; set; }
        public override string Title { get; set; }
        public override GUISkin Skin { get; set; }

        private void Draw(int winid)
        {
            GUILayout.BeginVertical();

            GUILayout.Label("Test window here");

            GUILayout.EndVertical();
        }

        
    }
}
