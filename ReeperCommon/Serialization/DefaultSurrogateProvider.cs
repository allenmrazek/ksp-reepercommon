using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using ReeperCommon.Logging;

namespace ReeperCommon.Serialization
{
// ReSharper disable once ClassWithVirtualMembersNeverInherited.Global
    public class DefaultSurrogateProvider : ISurrogateProvider
    {
        public virtual IEnumerable<KeyValuePair<Type, ISerializationSurrogate>> Get()
        {
            return GetTargets()
                .SelectMany(targetAssembly => targetAssembly.GetTypes())
                .Where(t => t.IsClass && t.IsVisible && !t.IsAbstract && !t.ContainsGenericParameters)
                .Where(t => t.GetConstructor(Type.EmptyTypes) != null && t.GetConstructor(Type.EmptyTypes).IsPublic)
                .Where(ImplementsGenericSerializationSurrogateInterface)
                .SelectMany(
                    t =>
                        GetSerializationTargetType(t)
                            .Select(
                                surrogateIdentifier =>
                                    new KeyValuePair<Type, ISerializationSurrogate>(surrogateIdentifier,
                                        Activator.CreateInstance(t) as ISerializationSurrogate)));
        }


        protected virtual IEnumerable<Assembly> GetTargets()
        {
            return new [] {Assembly.GetExecutingAssembly()};
        }


        private static bool ImplementsGenericSerializationSurrogateInterface(Type typeCheck)
        {
            return GetSerializationTargetType(typeCheck).Any();
        }


        private static IEnumerable<Type> GetSerializationTargetType(Type typeCheck)
        {
            return typeCheck.GetInterfaces()
                .Where(interfaceType => interfaceType.IsGenericType &&
                                        typeof (ISerializationSurrogate).IsAssignableFrom(interfaceType))
                .Select(interfaceType => interfaceType.GetGenericArguments().First());
        }
    }
}
