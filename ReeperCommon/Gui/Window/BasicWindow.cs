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
        protected IWindowLogic WindowLogic;
        protected Rect WindowRect = new Rect(0f, 0f, 0f, 0f);

        public BasicWindow(
            IWindowLogic windowLogic, 
            Rect rect, 
            int winid, 
            GUISkin skin, 
            bool draggable = true, 
            bool clamp = false)
        {
            if (windowLogic == null) throw new ArgumentNullException("windowLogic");
            if (skin == null) throw new ArgumentNullException("skin");

            Id = winid;
            WindowRect = rect;
            WindowLogic = windowLogic;
            Draggable = draggable;
            ClampToScreen = clamp;
            Visible = true;

            WindowLogic.OnAttached(this);
        }


        ~BasicWindow()
        {
            WindowLogic.OnDetached(this);
        }



        public virtual void OnWindowDraw(int winid)
        {
            if (!Skin.IsNull()) GUI.skin = Skin;

            WindowLogic.Draw();

            if (Draggable) GUI.DragWindow();
        }



        public virtual void Update()
        {
            WindowLogic.Update();
        }

        public Rect Dimensions
        {
            get { return WindowRect; }
            set { WindowRect = value; }
        }

        public string Title { get; set; }
        public GUISkin Skin { get; set; }
        public bool Draggable { get; set; }
        public bool ClampToScreen { get; set; }
        public bool Visible { get; set; }
        public int Id { get; set; }

        public IWindowLogic Logic
        {
            get { return WindowLogic; }
            set
            {
                if (!WindowLogic.IsNull() && !ReferenceEquals(WindowLogic, value))
                    WindowLogic.OnDetached(this);

                WindowLogic = value;
                WindowLogic.OnAttached(this);
            }
        }

        //protected virtual void Draw(int winid)
        //{
        //    if (!Skin.IsNull()) GUI.skin = Skin;

        //    _windowLogic.Draw();

        //    if (Draggable) GUI.DragWindow();
        //}
    }
}
