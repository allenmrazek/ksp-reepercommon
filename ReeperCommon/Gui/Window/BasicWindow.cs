using System;
using ReeperCommon.Extensions;
using ReeperCommon.Utility;
using UnityEngine;

namespace ReeperCommon.Gui.Window
{
    public abstract class BasicWindow : IWindowComponent
    {
        private Rect WindowRect = new Rect(0f, 0f, 100f, 100f);

        [Persistent] private int _id = 15000;
        [Persistent] private string _title = string.Empty;
        [Persistent] private bool _draggable = false;
        [Persistent] private bool _visible = true;
        [Persistent] private PersistentRect _rect = default(PersistentRect); // to avoid performance penalties of constantly implicitly casting

        protected BasicWindow(
            Rect rect, 
            int winid, 
            GUISkin skin, 
            bool draggable = true)
        {
            if (skin == null) throw new ArgumentNullException("skin");

            Id = winid;
            WindowRect = rect;
            Skin = skin;
            Draggable = draggable;
            Visible = true;
        }


        public virtual void OnWindowDraw(int winid)
        {
            if (!Skin.IsNull()) GUI.skin = Skin;

        }


        public virtual void OnWindowFinalize(int winid)
        {
            if (Draggable) GUI.DragWindow();
        }


        public virtual void Update()
        {

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
    }
}
