using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ReeperCommon.Gui.Window.Decorators;
using ReeperCommon.Gui.Window.Logic;
using ReeperCommon.Gui.Window.View;
using UnityEngine;

namespace ReeperCommon.Gui.Window.Factory
{
    public class WindowFactory
    {
        private readonly GUISkin _skin;

        [Flags]
        public enum WindowDecorators
        {
            None      =      0,
            Draggable = 1 << 0,
        }

        public WindowFactory(GUISkin skin)
        {
            if (skin == null) throw new ArgumentNullException("skin");

            _skin = skin;
        }

        public WindowView Create(IWindowLogic windowLogic, Rect rect, string title, int id, WindowDecorators style = WindowDecorators.None)
        {
            if (windowLogic == null) throw new ArgumentNullException("windowLogic");

            var view = new GameObject("ReeperWindow").AddComponent<WindowView>();

            var wbase = new BasicWindow(windowLogic, rect, id, _skin, IsFlagSet(style, WindowDecorators.Draggable),
                true);


             

            var impl = wbase as IWindowComponent;



            view.Implementation = impl;

            return view;
        }

        private bool IsFlagSet(WindowDecorators toCheck, WindowDecorators set)
        {
            return (toCheck & set) != 0;
        }
    }
}
