using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ReeperCommon.Extensions;
using ReeperCommon.Gui.Window.Decorators;
using ReeperCommon.Gui.Window.Logic;
using UnityEngine;

namespace ReeperCommon.Gui.Window
{
    public class BasicWindow : IWindowComponent
    {
        private readonly IWindowLogic _windowLogic;
        private Rect _windowRect = new Rect(0f, 0f, 0f, 0f);

        public BasicWindow(
            IWindowLogic windowLogic, 
            Rect rect, 
            int winid, 
            GUISkin skin, 
            bool draggable = true, 
            bool clamp = false,
            bool shrinkVertically = false)
        {
            if (windowLogic == null) throw new ArgumentNullException("windowLogic");
            if (skin == null) throw new ArgumentNullException("skin");

            _windowLogic = windowLogic;
            Id = winid;
            _windowRect = rect;
            Draggable = draggable;
            ClampToScreen = clamp;
            ShrinkVertically = shrinkVertically;
        }

        public virtual void OnPreWindowDraw()
        {

        }

        public virtual void OnWindowDraw()
        {
            if (ShrinkVertically) _windowRect.height = 1f;

            _windowRect = GUILayout.Window(Id, Dimensions, Draw, Title);

            if (ClampToScreen) _windowRect = KSPUtil.ClampRectToScreen(_windowRect);

        }

        public virtual void OnPostWindowDraw()
        {

        }

        public virtual void Update()
        {
            _windowLogic.Update();
        }

        public Rect Dimensions
        {
            get { return _windowRect; }
            set { _windowRect = value; }
        }

        public string Title { get; set; }
        public GUISkin Skin { get; set; }
        public bool Draggable { get; set; }
        public bool ClampToScreen { get; set; }
        public bool ShrinkVertically { get; set; }
        public bool Visible { get; set; }
        public int Id { get; set; }

        protected virtual void Draw(int winid)
        {
            if (!Skin.IsNull()) GUI.skin = Skin;

            _windowLogic.Draw();

            if (Draggable) GUI.DragWindow();
        }

        
    }
}
