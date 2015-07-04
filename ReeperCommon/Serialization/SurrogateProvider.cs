using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace ReeperCommon.Serialization
{
    public class SurrogateProvider : DefaultSurrogateProvider
    {
        private readonly Assembly[] _assembliesToSearch;

        public SurrogateProvider(IEnumerable<Assembly> assembliesToSearch)
        {
            if (assembliesToSearch == null) throw new ArgumentNullException("assembliesToSearch");
            _assembliesToSearch = assembliesToSearch.ToArray();

            if (_assembliesToSearch.Length == 0) throw new ArgumentException("No assemblies provided");
        }


        public override IEnumerable<Assembly> GetTargets()
        {
            return _assembliesToSearch;
        }
    }
}
