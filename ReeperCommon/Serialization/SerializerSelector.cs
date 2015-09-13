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





        //protected virtual Maybe<IConfigNodeItemSerializer> GetNative(Type target)
        //{
        //    return typeof(IReeperPersistent).IsAssignableFrom(target)
        //        ? Maybe<IConfigNodeItemSerializer>.With(new NativeSerializer())
        //        : Maybe<IConfigNodeItemSerializer>.None;
        //}


        //protected virtual Maybe<IConfigNodeItemSerializer> GetSurrogate(Type target)
        //{
        //    if (target == null) throw new ArgumentNullException("target");

        //    List<SurrogateInfo> potentialSurrogatesForType;

        //    if (!_surrogates.TryGetValue(target, out potentialSurrogatesForType))
        //        return target.IsGenericType ? GetGenericSurrogate(target) : Maybe<IConfigNodeItemSerializer>.None;

        //    foreach (var createdInstanceOfSurrogate in potentialSurrogatesForType
        //        .Select(potential => potential.Factory(target))
        //        .Where(createdInstanceOfSurrogate => createdInstanceOfSurrogate.Any()))
        //        return createdInstanceOfSurrogate;

        //    return target.IsGenericType ? GetGenericSurrogate(target) : Maybe<IConfigNodeItemSerializer>.None;
        //}


        ///// <summary>
        ///// Attempts to locate a serializer that has been specifically added to support
        ///// multiple target types with one surrogate instance.
        ///// 
        ///// For example, if I had a Setting<T>, I might AddSurrogate(typeof(Setting<>), someInstance)
        ///// to have someInstance handle serialization of all Setting variants. This puts a lot of
        ///// complexity into the surrogate itself, however, so use it sparingly
        ///// </summary>
        ///// <param name="target"></param>
        ///// <returns></returns>
        //protected virtual Maybe<IConfigNodeItemSerializer> GetGenericSurrogate(Type target)
        //{
        //    if (target == null) throw new ArgumentNullException("target");

        //    List<SurrogateInfo> genericSurrogatesForTarget;

        //    var genericDefinition = target.GetGenericTypeDefinition();
        //    if (genericDefinition == null) throw new Exception("Couldn't get generic definition of " + target.FullName);

        //    if (!_surrogates.TryGetValue(genericDefinition, out genericSurrogatesForTarget))
        //        return Maybe<IConfigNodeItemSerializer>.None;

        //    foreach (var createdInstanceOfSurrogate in genericSurrogatesForTarget
        //        .Select(potential => potential.Factory(target))
        //        .Where(createdInstanceOfSurrogate => createdInstanceOfSurrogate.Any()))
        //        return createdInstanceOfSurrogate;

        //    return Maybe<IConfigNodeItemSerializer>.None;
        //}


        //public virtual Maybe<IConfigNodeItemSerializer> GetSerializer(Type target)
        //{
        //    var native = GetNative(target);
        //    var surrogate = GetSurrogate(target);

        //    if (!native.Any() && !surrogate.Any())
        //        return Maybe<IConfigNodeItemSerializer>.None;

        //    return Maybe<IConfigNodeItemSerializer>.With(new ReeperPersistentMethodCaller(
        //        native.SingleOrDefault() ?? surrogate.SingleOrDefault()));
        //}


        //public override string ToString()
        //{
        //    return "SerializerSelector: surrogates: " +
        //           string.Join(",", _surrogates.Keys.Select(k => k.FullName).ToArray());
        //}

    }
}
