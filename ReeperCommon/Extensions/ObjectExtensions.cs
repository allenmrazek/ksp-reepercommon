using System;
using System.Linq;
using ReeperCommon.Containers;
using ReeperCommon.Logging;

namespace ReeperCommon.Extensions
{
    public static class ObjectExtensions
    {
        public static bool IsNull(this object source)
        {
            return ReferenceEquals(source, null);
        }


        public static bool IsSameAs(this object source, object other)
        {
            return ReferenceEquals(source, other);
        }
    }
}
