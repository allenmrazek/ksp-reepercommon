using System;
using ReeperCommon.Extensions;
using ReeperCommon.Gui.Logic;
using ReeperCommon.Utility;
using UnityEngine;

namespace ReeperCommon.Gui.Window
{
    public class BasicWindow : IWindowComponent
    {
        protected Rect WindowRect = new Rect(0f, 0f, 100f, 100f);
        protected IWindowLogic WindowLogic;

        [Persistent] private int _id = 15000;
        [Persistent] private string _title = string.Empty;
        [Persistent] private bool _draggable = false;
        [Persistent] private bool _visible = true;
        [Persistent] private PersistentRect _rect = default(PersistentRect); // to avoid performance penalties of constantly implicitly casting

        public BasicWindow(
            IWindowLogic windowLogic, 
            Rect rect, 
            int winid, 
            GUISkin skin, 
            bool draggable = true)
        {
            if (windowLogic == null) throw new ArgumentNullException("windowLogic");
            if (skin == null) throw new ArgumentNullException("skin");

            Id = winid;
            WindowRect = rect;
            Skin = skin;
            Logic = windowLogic;
            Draggable = draggable;
            Visible = true;
            WindowLogic = windowLogic;
        }


        public virtual void OnWindowDraw(int winid)
        {
            if (!Skin.IsNull()) GUI.skin = Skin;

            Logic.Draw();
        }


        public virtual void OnWindowFinalize(int winid)
        {
            if (Draggable) GUI.DragWindow();
        }


        public virtual void Update()
        {
            Logic.Update();
        }


        public virtual void Save(ConfigNode node)
        {
            if (node == null) throw new ArgumentNullException("node");

            _rect = Dimensions;
            ConfigNode.CreateConfigFromObject(this, node);
        }


        public virtual void Load(ConfigNode node)
        {
            if (node == null) throw new ArgumentNullException("node");

            ConfigNode.LoadObjectFromConfig(this, node);
            Dimensions = _rect;
        }


        public Rect Dimensions
        {
            get { return WindowRect; }
            set { WindowRect = value; }
        }


        public string Title {
            get { return _title; } 
            set { _title = value; }
        }


        public GUISkin Skin { get; set; }

        
        public bool Draggable {
            get { return _draggable; }
            set { _draggable = value; }
        }

        
        public bool Visible {
            get { return _visible; }
            set { _visible = value; }
        }

        
        public int Id {
            get { return _id; }
            set { _id = value; }
        }


        public IWindowLogic Logic
        {
            get { return WindowLogic; }
            set { if (value.IsNull()) throw new ArgumentNullException("value"); WindowLogic = value; }
        }
    }
}
