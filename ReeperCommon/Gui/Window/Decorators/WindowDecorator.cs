using System;
using UnityEngine;

namespace ReeperCommon.Gui.Window.Decorators
{
    class WindowDecorator : WindowComponent
    {
        private readonly WindowComponent _base;

        protected WindowDecorator(WindowComponent baseComponent)
        {
            if (baseComponent == null) throw new ArgumentNullException("baseComponent");

            _base = baseComponent;
        }

        public override void OnPreWindowDraw()
        {
            _base.OnPreWindowDraw();
        }

        public override void OnWindowDraw()
        {
            _base.OnWindowDraw();
        }

        public override void OnPostWindowDraw()
        {
            _base.OnPostWindowDraw();
        }

        public override void Update()
        {
            _base.Update();
        }

        public override Rect Dimensions
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


        public override string Title
        {
            get { return _base.Title; }
            set { _base.Title = value; }
        }

        public override GUISkin Skin
        {
            get { return _base.Skin; }
            set { _base.Skin = value; }
        }
    }
}
