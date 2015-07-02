using ReeperCommon.Extensions;
using ReeperCommon.Serialization;

namespace ReeperCommon.Gui
{
// ReSharper disable once InconsistentNaming
    public class WindowID : IReeperPersistent
    {
// ReSharper disable once MemberCanBePrivate.Global
// ReSharper disable once UnusedAutoPropertyAccessor.Global
// ReSharper disable once InconsistentNaming
        public int Value { get; set; }


        public WindowID()
        {
            Value = UniqueWindowIdProvider.Get();
        }


// ReSharper disable once UnusedMember.Global
        public WindowID(int value)
        {
            Value = value;
        }

        public void Serialize(IConfigNodeSerializer formatter, ConfigNode node)
        {
            node.AddValue("WindowID", Value);
        }

        public void Deserialize(IConfigNodeSerializer formatter, ConfigNode node)
        {
            Value = node.Parse("WindowID", UniqueWindowIdProvider.Get());
        }
    }
}
