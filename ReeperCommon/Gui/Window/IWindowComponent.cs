using UnityEngine;

namespace ReeperCommon.Gui.Window
{
    public interface IWindowComponent
    {
        void OnPreWindowDraw();
        void OnWindowDraw();
        void OnPostWindowDraw();
        void Update();

        Rect Dimensions { get; set; }
        int Id { get; }
        string Title { get; set; }
        GUISkin Skin { get; set;}
        bool Draggable { get; set; }
        bool ClampToScreen { get; set; }
        bool ShrinkVertically { get; set; }
        bool Visible { get; set; }
    }
}
