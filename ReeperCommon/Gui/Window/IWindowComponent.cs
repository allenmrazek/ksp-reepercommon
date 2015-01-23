using ReeperCommon.Gui.Window.Logic;
using UnityEngine;

namespace ReeperCommon.Gui.Window
{
    public interface IWindowComponent
    {
        void OnWindowDraw();
        void Update();

        Rect Dimensions { get; set; }
        int Id { get; }
        string Title { get; set; }
        GUISkin Skin { get; set;}
        bool Draggable { get; set; }
        bool ClampToScreen { get; set; }
        bool Visible { get; set; }
        IWindowLogic Logic { get; set; }
    }
}
