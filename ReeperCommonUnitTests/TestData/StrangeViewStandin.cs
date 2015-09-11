using ReeperCommon.Gui;
using ReeperCommon.Gui.Window;
using ReeperCommon.Serialization;
using UnityEngine;

namespace ReeperCommonUnitTests.TestData
{
    public abstract class StrangeViewStandin : IWindowComponent
    {
        [ReeperPersistent] private string TestField = "blah";

        public void OnWindowPreDraw()
        {
            throw new System.NotImplementedException();
        }

        public void OnWindowDraw(int winid)
        {
            throw new System.NotImplementedException();
        }

        public void OnWindowFinalize(int winid)
        {
            throw new System.NotImplementedException();
        }

        public void OnUpdate()
        {
            throw new System.NotImplementedException();
        }

        public Rect Dimensions { get; set; }
        public WindowID Id { get; private set; }
        public string Title { get; set; }
        public GUISkin Skin { get; set; }
        public bool Draggable { get; set; }
        public bool Visible { get; set; }
        public float Width { get; set; }
        public float Height { get; set; }
        public void DuringSerialize(IConfigNodeSerializer formatter, ConfigNode node)
        {
            formatter.Serialize(this, node);
        }

        public void DuringDeserialize(IConfigNodeSerializer formatter, ConfigNode node)
        {
            formatter.Deserialize(this, node);
        }
    }
}