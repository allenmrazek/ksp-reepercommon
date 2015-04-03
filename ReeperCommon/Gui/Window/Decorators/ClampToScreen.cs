using UnityEngine;

namespace ReeperCommon.Gui.Window.Decorators
{
    public class ClampToScreen : WindowDecorator
    {
        public ClampToScreen(IWindowComponent baseComponent) : base(baseComponent)
        {
        }


        public override void OnWindowDraw(int winid)
        {
            base.OnWindowDraw(winid);

            if (Event.current.type != EventType.Repaint)
                Dimensions = KSPUtil.ClampRectToScreen(Dimensions);
        }
    }
}
