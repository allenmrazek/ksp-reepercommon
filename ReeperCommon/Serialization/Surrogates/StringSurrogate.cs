namespace ReeperCommon.Serialization.Surrogates
{
// ReSharper disable once UnusedMember.Global
    public class StringSurrogate : FieldSurrogateToSingleValueBase<string>
    {
        protected override string GetFieldContentsFromString(string value)
        {
            return value;
        }
    }
}
