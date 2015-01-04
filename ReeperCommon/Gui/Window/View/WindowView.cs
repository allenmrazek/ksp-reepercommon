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
        //private WindowLogic _logic = new DefaultLogic();

        //public WindowLogic Logic
        //{
        //    get
        //    {
        //        return _logic;
        //    }
        //    set { _logic = value ?? new DefaultLogic(); }
        //}

        public WindowComponent Implementation { get; set; }

        private void OnGUI()
        {
            if (Implementation.IsNull()) return;

            print("Drawing window of size " + Implementation.Dimensions.ToString());

            Implementation.OnPreWindowDraw();
            Implementation.OnWindowDraw();
            Implementation.OnPostWindowDraw();
        }
    }
}
