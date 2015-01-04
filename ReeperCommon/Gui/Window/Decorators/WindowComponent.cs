using UnityEngine;

namespace ReeperCommon.Gui.Window.Decorators
{
    public abstract class WindowComponent
    {
        public abstract void OnPreWindowDraw();
        public abstract void OnWindowDraw();
        public abstract void OnPostWindowDraw();
        public abstract void Update();

        public abstract Rect Dimensions { get; set; }
        public int Id { get; set; }
        public abstract string Title { get; set; }
        public abstract GUISkin Skin { get; set; }
    }
}
