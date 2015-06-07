using System;
using System.Reflection;
using System.Runtime.Serialization;

namespace ReeperCommon.Serialization.Surrogates
{
    public class PrimitiveSurrogateBase<T> : FieldSurrogateToSingleValueBase<T>
    {
        protected override T GetFieldContentsFromString(string value)
        {
            var parseMethod = typeof (T).GetMethod("TryParse", 
                BindingFlags.Public | BindingFlags.Static, null, 
                new [] { typeof(string), typeof(T).MakeByRefType()}, null);

            if (parseMethod == null)
                throw new MissingMethodException(typeof (T).FullName +
                                                 " missing TryParse method. You may need to implement a speciality serialization surrogate for this type.");

            var parameters = new object[] {value, null};

            if ((bool) parseMethod.Invoke(null, parameters))
                return (T) parameters[1];
            
            throw new SerializationException("Could not parse " + typeof (T).FullName + " from string value '" +
                                                 value + "' with TryParse");
        }
    }
}
