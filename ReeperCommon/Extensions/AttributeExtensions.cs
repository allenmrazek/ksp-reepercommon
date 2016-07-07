using System;
using ReeperCommon.Containers;

namespace ReeperCommon.Extensions
{
    public static class AttributeExtensions
    {
        public static Maybe<TAttribute> GetAttribute<TAttribute>(this Type type) where TAttribute : Attribute
        {
            var attrtype = typeof (TAttribute);

            if (type == null) return Maybe<TAttribute>.None;

            if (!Attribute.IsDefined(type, attrtype))
                return Maybe<TAttribute>.None;

            var attr = Attribute.GetCustomAttribute(type, attrtype) as TAttribute;

            return attr == null ? Maybe<TAttribute>.None : Maybe<TAttribute>.With(attr);
        }

        public static Maybe<TAttribute> GetAttribute<TAttribute>(this object obj) where TAttribute : Attribute
        {
            if (obj == null) return Maybe<TAttribute>.None;

            return GetAttribute<TAttribute>(obj.GetType());
        }
    }
}
