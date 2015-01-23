using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ReeperCommon.Extensions;
using ReeperCommon.Gui.Window.Decorators;
using UnityEngine;

namespace ReeperCommon.Gui.Window.View
{
    public class WindowView : MonoBehaviour
    {
        public IWindowComponent Implementation { get; set; }



        private void OnDestroy()
        {
            print("WindowView is destructing...");
        }



        private void OnGUI()
        {
            if (Implementation.IsNull() || !Implementation.Visible) return;
           
            Implementation.OnWindowDraw();
        }



        private void Update()
        {
            if (Implementation.IsNull()) return;

            Implementation.Update();
        }



        public static WindowView Create(IWindowComponent window, string goName = "WindowView")
        {
            if (window == null) throw new ArgumentNullException("window");

            var view = new GameObject(goName).AddComponent<WindowView>();
            view.Implementation = window;

            return view;
        }
    }
}
