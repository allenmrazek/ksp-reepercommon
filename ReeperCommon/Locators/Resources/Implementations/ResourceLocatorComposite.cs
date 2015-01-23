using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ReeperCommon.Containers;

namespace ReeperCommon.Locators.Resources.Implementations
{
    public class ResourceLocatorComposite : IResourceLocator
    {
        private readonly IEnumerable<IResourceLocator> _locators;

        public ResourceLocatorComposite(params IResourceLocator[] locators)
        {
            if (locators == null) throw new ArgumentNullException("locators");
            _locators = locators;
        }



        public Maybe<byte[]> FindResource(string identifier)
        {
            var result = Maybe<byte[]>.None;

            foreach (var locator in _locators)
            {
                result = locator.FindResource(identifier);
                if (result.Any())
                    return result;
            }

            return result;
        }



        public IEnumerable<string> GetPossibilities()
        {
            return _locators.SelectMany(locator => locator.GetPossibilities());
        }
    }
}
