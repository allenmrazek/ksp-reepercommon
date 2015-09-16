using System;
using ReeperCommon.Containers;

namespace ReeperCommon.Serialization
{
// ReSharper disable once UnusedMember.Global
    /// <summary>
    /// The simplest selector just looks for a surrogate to use
    /// </summary>
    public class SerializerSelector : ISerializerSelector
    {
        private readonly ISurrogateProvider _surrogates;

        public SerializerSelector(ISurrogateProvider surrogates)
        {
            if (surrogates == null) throw new ArgumentNullException("surrogates");

            _surrogates = surrogates;
        }

        public virtual Maybe<IConfigNodeItemSerializer> GetSerializer(Type target)
        {
            return _surrogates.Get(target);
        }
    }
}
