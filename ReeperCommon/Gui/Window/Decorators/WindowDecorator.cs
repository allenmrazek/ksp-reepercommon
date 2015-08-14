﻿using System;
using ReeperCommon.Logging;
using ReeperCommon.Serialization;
using UnityEngine;

namespace ReeperCommon.Gui.Window.Decorators
{
    public abstract class WindowDecorator : IWindowComponent
    {
        private readonly IWindowComponent _base;

        public WindowDecorator(IWindowComponent baseComponent)
        {
            if (baseComponent == null) throw new ArgumentNullException("baseComponent");

            _base = baseComponent;
        }


        public virtual void OnWindowPreDraw()
        {
            _base.OnWindowPreDraw();
        }


        public virtual void OnWindowDraw(int winid)
        {
            _base.OnWindowDraw(winid);
        }


        public virtual void OnWindowFinalize(int winid)
        {
            _base.OnWindowFinalize(winid);
        }


        public virtual void OnUpdate()
        {
            _base.OnUpdate();
        }


        public virtual void Serialize(IConfigNodeSerializer formatter, ConfigNode node)
        {
            _base.Serialize(formatter, node);
        }


        public virtual void Deserialize(IConfigNodeSerializer formatter, ConfigNode node)
        {
            _base.Deserialize(formatter, node);
        }


        public Rect Dimensions
        {
            get
            {
                return _base.Dimensions;
            }
            set
            {
                _base.Dimensions = value;
            }
        }


        public string Title
        {
            get { return _base.Title; }
            set { _base.Title = value; }
        }


        public GUISkin Skin
        {
            get { return _base.Skin; }
            set { _base.Skin = value; }
        }


        public bool Draggable { 
            get { return _base.Draggable; }
            set { _base.Draggable = value; }
        }


        public virtual bool Visible
        {
            get { return _base.Visible; }
            set { _base.Visible = value; }
        }


        public WindowID Id { get { return _base.Id; } }


        public float Width
        {
            get { return _base.Width; }
            set { _base.Width = value; }
        }

        public float Height
        {
            get { return _base.Height; }
            set { _base.Height = value; }
        }
    }
}
