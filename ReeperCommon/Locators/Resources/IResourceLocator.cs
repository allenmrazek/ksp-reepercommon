using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ReeperCommon.Containers;

namespace ReeperCommon.Locators.Resources
{
    public interface IResourceLocator
    {
        Maybe<byte[]> FindResource(string identifier);
        IEnumerable<string> GetPossibilities();
    }
}
