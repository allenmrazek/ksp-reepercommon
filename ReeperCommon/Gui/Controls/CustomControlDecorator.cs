using System;
using UnityEngine;

namespace ReeperCommon.Gui.Controls
{
    public abstract class CustomControlDecorator : ICustomControl
    {
        private readonly ICustomControl _decorated;

        protected CustomControlDecorator(ICustomControl decorated)
        {
            if (decorated == null) throw new ArgumentNullException("decorated");
            _decorated = decorated;
        }

        public void Draw(params GUILayoutOption[] options)
        {
            PreDecorate(options);
            _decorated.Draw(options);
            PostDecorate(options);
        }

        protected abstract void PreDecorate(params GUILayoutOption[] options);
        protected abstract void PostDecorate(params GUILayoutOption[] options);
    }
}
