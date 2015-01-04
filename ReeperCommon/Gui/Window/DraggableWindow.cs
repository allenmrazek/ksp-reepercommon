using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace ReeperCommon.Gui.Window
{
    // note: this is an extension of WindowBase instead of a decorator because we need access
    // to the actual window drawing method for GUI.DragWindow to work
    public class DraggableWindow : WindowBase
    {
        public DraggableWindow(
            Rect rect,
            int winid,
            GUISkin skin,
            bool draggable = true,
            bool clampScreen = true) : base(rect, winid, skin)
        {
            Draggable = draggable;
            ClampToScreen = clampScreen;
        }

        protected override void Draw(int winid)
        {
            base.Draw(winid);

            if (Draggable) GUI.DragWindow();
        }

        public override void OnPostWindowDraw()
        {
            base.OnPostWindowDraw();

            if (ClampToScreen) Dimensions = KSPUtil.ClampRectToScreen(Dimensions);
        }

        public bool Draggable { get; set; }
        public bool ClampToScreen { get; set; }
    }
}
