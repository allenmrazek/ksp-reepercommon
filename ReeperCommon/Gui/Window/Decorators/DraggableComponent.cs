using UnityEngine;

namespace ReeperCommon.Gui.Window.Decorators
{
    class DraggableComponent : WindowDecorator
    {
        public DraggableComponent(WindowComponent _base, bool initialDraggable = true) : base(_base)
        {
            Draggable = initialDraggable;
        }

        public override void OnPreGUI()
        {
            base.OnPreGUI();
            Dimensions = KSPUtil.ClampRectToScreen(Dimensions);
        }

        public override void OnPostGUI()
        {
            base.OnPostGUI();
            GUI.DragWindow();
        }

        public bool Draggable { get; set; }
    }
}
