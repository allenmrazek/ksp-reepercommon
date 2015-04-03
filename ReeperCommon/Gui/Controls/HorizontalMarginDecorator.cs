using UnityEngine;

namespace ReeperCommon.Gui.Controls
{
    public class HorizontalMarginDecorator : CustomControlDecorator
    {
        private readonly float _leftMargin;
        private readonly float _rightMargin;

        public HorizontalMarginDecorator(float leftMargin, float rightMargin, ICustomControl decorated) : base(decorated)
        {
            _leftMargin = leftMargin;
            _rightMargin = rightMargin;
        }

        protected override void PreDecorate(params GUILayoutOption[] options)
        {
            GUILayout.BeginHorizontal(GUILayout.ExpandWidth(true), GUILayout.ExpandHeight(true));
            GUILayout.Space(_leftMargin);
        }

        protected override void PostDecorate(params GUILayoutOption[] options)
        {
            GUILayout.Space(_rightMargin);
            GUILayout.EndHorizontal();
        }
    }
}
