using System;
using UnityEngine;

namespace ReeperCommon.Gui.Window.Decorators
{
    public abstract class WindowDecorator : IWindowComponent
    {
        private readonly IWindowComponent _base;

        protected WindowDecorator(IWindowComponent baseComponent)
        {
            if (baseComponent == null) throw new ArgumentNullException("baseComponent");

            _base = baseComponent;
        }

        public virtual void OnPreWindowDraw()
        {
            _base.OnPreWindowDraw();
        }

        public virtual void OnWindowDraw()
        {
            _base.OnWindowDraw();
        }

        public virtual void OnPostWindowDraw()
        {
            _base.OnPostWindowDraw();
        }

        public virtual void Update()
        {
            _base.Update();
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

        public bool ClampToScreen
        {
            get { return _base.ClampToScreen; }
            set { _base.ClampToScreen = value; }
        }

        public bool ShrinkVertically
        {
            get { return _base.ShrinkVertically; }
            set { _base.ShrinkVertically = value; }
        }

        public bool Visible
        {
            get { return _base.Visible; }
            set { _base.Visible = value; }
        }

        public int Id { get { return _base.Id; } }
    }
}
