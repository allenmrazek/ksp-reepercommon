using ReeperCommon.Serialization;
using UnityEngine;

namespace ReeperCommon.Gui.Window.Decorators
{
// ReSharper disable once UnusedMember.Global
    public class Scaling : WindowDecorator
    {
// ReSharper disable once MemberCanBePrivate.Global
        [ReeperPersistent] private Vector3 _scale = Vector3.one;

        public Scaling(IWindowComponent baseComponent, Vector2 initialScale) : base(baseComponent)
        {
            Scale = initialScale;
        }

        public override void OnWindowPreDraw()
        {
            base.OnWindowPreDraw();
            GUI.matrix = Matrix4x4.TRS(Vector3.zero, Quaternion.identity, _scale);
        }


        public Vector2 Scale
        {
            get { return _scale; }
            set { _scale = new Vector3(value.x, value.y, 1f); }
        }
    }
}
