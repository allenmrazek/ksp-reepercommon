using System;
using ReeperCommon.Extensions;
using UnityEngine;

namespace ReeperCommon.Gui.Window.Decorators
{
// ReSharper disable once UnusedMember.Global
    public class AdjustableScale : Resizable
    {
        private readonly WindowScale _scale;
        private readonly float _minScalar;
        private readonly float _maxScalar;

        public AdjustableScale(
            WindowScale decoratedComponent,
            Vector2 hotzoneSize, 
            Vector2 minSize,
            float minScalar,
            float maxScalar,
            Texture2D hintTexture, 
            float hintPopupDelay, 
            Vector2 hintScale,
            Func<bool> allowScaling) : base(decoratedComponent, hotzoneSize, minSize, hintTexture, hintPopupDelay, hintScale, allowScaling)
        {
            _scale = decoratedComponent;
            _minScalar = minScalar;
            _maxScalar = maxScalar;

        }


        protected override void OnDragUpdate(Matrix4x4 guiMatrix)
        {
            // this should be rather straightforward: figure out where the mouse is
            // and what size the current dimensions are, then figure out scale to apply
            // to the original dimensions to make the window as large as it appears

            var mousePos = GetScreenPositionOfMouse();

            var offset = new Vector2(Dimensions.x, Dimensions.y);
            mousePos -= offset;                                  // scaling gets weird when the origin isn't at zero,
                                                                 // so let's pretend the window is at the top left

            var desired = new Rect(
                0f,
                0f,
                mousePos.x,
                mousePos.y);

            // what scalar to use to get current Dimensions to match desired dimensions?
            float scalar;

            if (Mathf.Abs(mousePos.y - Dimensions.y) > Mathf.Abs(mousePos.x - Dimensions.x))
            {
                // match height
                scalar = desired.height / Dimensions.height;
            }
            else // match width
            {
                scalar = desired.width / Dimensions.width;
            }

            scalar = Mathf.Clamp(scalar, _minScalar, _maxScalar);
            var vScalar = new Vector2(scalar, scalar);

            var m = Matrix4x4.Scale(vScalar);
            var invDim = Dimensions.Invert(m);
            
            Dimensions = new Rect(Dimensions.x - invDim.x, Dimensions.y - invDim.y, Dimensions.width, Dimensions.height);
            _scale.Scale = vScalar;
        }
    }
}
