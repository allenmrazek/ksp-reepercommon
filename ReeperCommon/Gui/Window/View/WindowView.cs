using System;
using ReeperCommon.Extensions;
using UnityEngine;

namespace ReeperCommon.Gui.Window.View
{
    // note: do not hide this behind an interface; it will mess with Unity's operator overloads of == and Equals
// ReSharper disable once ClassNeverInstantiated.Global
    public class WindowView : MonoBehaviour
    {
        private IWindowComponent Implementation { get; set; }



// ReSharper disable once UnusedMember.Global
        public void OnDestroy()
        {
            print("WindowView is destructing...");
        }



// ReSharper disable once InconsistentNaming
// ReSharper disable once UnusedMember.Global
        public void OnGUI()
        {
            if (Implementation.IsNull() || !Implementation.Visible) return;

            if (!Implementation.Skin.IsNull())
                GUI.skin = Implementation.Skin;

            Implementation.Dimensions = GUILayout.Window(Implementation.Id, Implementation.Dimensions, Implementation.OnWindowDraw,
                Implementation.Title);
        }



// ReSharper disable once UnusedMember.Global
        public void Update()
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
