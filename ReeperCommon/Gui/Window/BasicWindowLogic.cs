using System;
using ReeperCommon.Extensions;
using ReeperCommon.Serialization;
using UnityEngine;

namespace ReeperCommon.Gui.Window
{
    public class BasicWindowLogic : IWindowComponent
    {
        [ReeperPersistent] private Rect _windowRect = new Rect(0f, 0f, 100f, 100f);
        [ReeperPersistent] private WindowID _id = new WindowID();
        [ReeperPersistent] private string _title = string.Empty;
        [ReeperPersistent] private bool _draggable = false;
        [ReeperPersistent] private bool _visible = true;

        public BasicWindowLogic(
            Rect rect, 
            WindowID winid, 
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


        public virtual void OnWindowPreDraw()
        {
            
        }


        public virtual void OnWindowDraw(int winid)
        {
            if (!Skin.IsNull()) GUI.skin = Skin;

        }


        public virtual void OnWindowFinalize(int winid)
        {
            if (Draggable) GUI.DragWindow();
        }


        public virtual void OnUpdate()
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

        
        public WindowID Id {
            get { return _id; }
            set { _id = value; }
        }
    }
}
