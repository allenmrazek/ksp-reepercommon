using System;
using System.Collections.Generic;
using ReeperCommon.GameLoadState.Attributes;

namespace ReeperCommon.GameLoadState.Providers
{
    public interface ILoadStateMarkedTypeProvider
    {
        IEnumerable<Type> GetMarkedTypesFor(LoadStateMarker.State state);
    }
}
