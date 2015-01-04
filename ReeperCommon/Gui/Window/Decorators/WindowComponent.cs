using UnityEngine;

namespace ReeperCommon.Gui.Window.Decorators
{
    public abstract class WindowComponent
    {
        public abstract void OnPreGUI();
        public abstract void OnGUI();
        public abstract void OnPostGUI();


        public abstract void Update();

        public abstract Rect Dimensions { get; set; }
        public abstract int Id { get; set; }
        public abstract string Title { get; set; }
        public abstract GUISkin Skin { get; set; }
    }
}
