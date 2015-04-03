using UnityEngine;

namespace ReeperCommon.Gui.Controls
{
    public interface ICustomControl
    {
        void Draw(params GUILayoutOption[] options);
    }
}
