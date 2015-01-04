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

        public override void OnPreGUI()
        {
            _base.OnPreGUI();
        }

        public override void OnGUI()
        {
            _base.OnGUI();
        }

        public override void OnPostGUI()
        {
            _base.OnPostGUI();
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

        public override int Id
        {
            get { return _base.Id; }
            set { _base.Id = value; }
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
