using System;
using ReeperCommon.Extensions;
using ReeperCommon.Gui.Logic;
using UnityEngine;

namespace ReeperCommon.Gui.Window
{
    public class BasicWindow : IWindowComponent
    {
        protected Rect WindowRect = new Rect(0f, 0f, 100f, 100f);
        private readonly int _depth;
        protected IWindowLogic WindowLogic;

        public BasicWindow(
            IWindowLogic windowLogic, 
            Rect rect, 
            int winid, 
            GUISkin skin, 
            bool draggable = true,
            int depth = 1)
        {
            if (windowLogic == null) throw new ArgumentNullException("windowLogic");
            if (skin == null) throw new ArgumentNullException("skin");

            Id = winid;
            WindowRect = rect;
            _depth = depth;
            Skin = skin;
            Logic = windowLogic;
            Draggable = draggable;
            Visible = true;
            WindowLogic = windowLogic;
        }



        public virtual void OnWindowDraw(int winid)
        {
            if (!Skin.IsNull()) GUI.skin = Skin;

            //GUI.depth = _depth;
            Logic.Draw();

            //if (Draggable) GUI.DragWindow();
        }


        public virtual void OnWindowFinalize(int winid)
        {
            if (Draggable) GUI.DragWindow();
        }


  


        public virtual void Update()
        {
            Logic.Update();
        }


        public Rect Dimensions
        {
            get { return WindowRect; }
            set { WindowRect = value; }
        }

        public string Title { get; set; }
        public GUISkin Skin { get; set; }
        public bool Draggable { get; set; }
        public bool Visible { get; set; }
        public int Id { get; set; }

        public IWindowLogic Logic
        {
            get { return WindowLogic; }
            set { if (value.IsNull()) throw new ArgumentNullException("value"); WindowLogic = value; }
        }
    }
}
