using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using ReeperCommon.Containers;
using ReeperCommon.Extensions;
using ReeperCommon.Logging;

namespace ReeperCommon.Locators.Resources.Implementations
{
    public class ResourceFromEmbeddedResource : IResourceLocator
    {
        private readonly Assembly _resource;
 

        public ResourceFromEmbeddedResource(Assembly resource)
        {
            if (resource == null) throw new ArgumentNullException("resource");

            _resource = resource;
        }



        public Maybe<byte[]> FindResource(string identifier)
        {

            var stream = _resource.GetManifestResourceStream(identifier);

            if (stream == null || stream.Length == 0)
                return Maybe<byte[]>.None;
            

            var data = new byte[stream.Length];

            int read = 0;
            var ms = new MemoryStream();

            while ((read = stream.Read(data, 0, data.Length)) > 0)
                ms.Write(data, 0, read);

            return Maybe<byte[]>.With(data);
        }



        public IEnumerable<string> GetPossibilities()
        {
            return _resource.GetManifestResourceNames();
        }
    }
}
