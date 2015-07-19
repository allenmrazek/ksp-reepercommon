using ReeperCommon.Extensions;
using UnityEngine;

namespace ReeperCommon.Gui.Window.Decorators
{
    public class ClampToScreen : WindowDecorator
    {
        public ClampToScreen(IWindowComponent baseComponent) : base(baseComponent)
        {
        }


        public override void OnWindowPreDraw()
        {
            base.OnWindowPreDraw();

            if (Event.current.type != EventType.Repaint) return;

            Dimensions = KSPUtil.ClampRectToScreen(Dimensions.Multiply(GUI.matrix)).Multiply(GUI.matrix.inverse);
        }
    }
}
