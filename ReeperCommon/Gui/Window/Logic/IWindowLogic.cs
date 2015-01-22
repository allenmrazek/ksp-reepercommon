using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ReeperCommon.Gui.Window.Logic
{
    public interface IWindowLogic
    {
        void Draw();
        void Update();
        void OnAttached(IWindowComponent component);
        void OnDetached(IWindowComponent component);
    }
}
