using System;
using UnityEngine;

namespace ReeperCommon.Gui.Controls
{
    [Serializable]
    public class ExpandablePanel : IExpandablePanel, ICustomControl
    {
        private readonly GUIStyle _toggleStyle;
        private readonly string _text;
        private readonly Action _drawMethod;

        [Persistent]
        private bool _expanded;



        public ExpandablePanel(GUIStyle toggleStyle, string text, Action drawMethod, bool expandedIntitially = false)
        {
            if (toggleStyle == null) throw new ArgumentNullException("toggleStyle");
            if (text == null) throw new ArgumentNullException("text");
            if (drawMethod == null) throw new ArgumentNullException("drawMethod");


            _toggleStyle = toggleStyle;
            _text = text;
            _drawMethod = drawMethod;
            _expanded = expandedIntitially;
        }


        public void Draw(params GUILayoutOption[] options)
        {
            GUILayout.BeginVertical(options);

            _expanded = GUILayout.Toggle(_expanded, _text, _toggleStyle, options);

            if (Expanded)
                _drawMethod();

            GUILayout.EndVertical();
        }


        public bool Expanded
        {
            get { return _expanded; }
        }
    }
}
