using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;

namespace ReeperCommon.Providers.Implementations
{
    public class AssemblyLocation : IAssemblyLocationProvider
    {
        private readonly string _location;

        public AssemblyLocation(string location)
        {
            if (location == null) throw new ArgumentNullException("location");
            if (string.IsNullOrEmpty(location)) throw new ArgumentException("location must be valid");

            _location = location;
        }

        public string Get()
        {
            return _location;
        }
    }
}
