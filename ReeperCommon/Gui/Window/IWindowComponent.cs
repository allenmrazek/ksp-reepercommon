using ReeperCommon.Gui.Logic;
using UnityEngine;

namespace ReeperCommon.Gui.Window
{
    public interface IWindowComponent
    {
        void OnWindowDraw(int winid);
        void Update();

        Rect Dimensions { get; set; }
        int Id { get; }
        string Title { get; set; }
        GUISkin Skin { get; set;}
        bool Draggable { get; set; }
        bool Visible { get; set; }
        IWindowLogic Logic { get; set; }
    }
}
