using System;
using ReeperCommon.Extensions;
using ReeperCommon.Serialization;
using UnityEngine;

namespace ReeperCommon.Gui.Window
{
    public abstract class BasicWindowLogic : IWindowComponent
    {
        [ReeperPersistent]
        private Rect _windowRect = new Rect(0f, 0f, 100f, 100f);
        [ReeperPersistent]
        private int _id = 15000;
        [ReeperPersistent]
        private string _title = string.Empty;
        [ReeperPersistent]
        private bool _draggable = false;
        [ReeperPersistent]
        private bool _visible = true;

        protected BasicWindowLogic(
            Rect rect, 
            int winid, 
            GUISkin skin, 
            bool draggable = true)
        {
            if (skin == null) throw new ArgumentNullException("skin");

            Id = winid;
            _windowRect = rect;
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


        public virtual void Serialize(IConfigNodeSerializer formatter, ConfigNode node)
        {
            if (node == null) throw new ArgumentNullException("node");

            formatter.Serialize(this, node);
        }


        public virtual void Deserialize(IConfigNodeSerializer formatter, ConfigNode node)
        {
            if (node == null) throw new ArgumentNullException("node");

            formatter.Deserialize(this, node);
        }


        public Rect Dimensions
        {
            get { return _windowRect; }
            set { _windowRect = value; }
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
