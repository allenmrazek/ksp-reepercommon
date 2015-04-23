namespace ReeperCommon.Serialization.Surrogates
{
// ReSharper disable once ClassNeverInstantiated.Global
    public class PrimitiveSurrogates : FieldSurrogateBaseToString, 
        ISerializationSurrogate<string>, 
        ISerializationSurrogate<int>, 
        ISerializationSurrogate<float>, 
        ISerializationSurrogate<double>,
        ISerializationSurrogate<bool>
    {
    }
}
