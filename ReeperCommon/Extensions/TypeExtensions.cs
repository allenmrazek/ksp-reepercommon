using System;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using ReeperCommon.Containers;

namespace ReeperCommon.Extensions
{
    public static class TypeExtensions
    {
        public static MethodInfo GetMethod<TInstance>(Expression<Action<TInstance>> expr)
        {
            return ((MethodCallExpression)expr.Body).Method;
        }

        public static bool HasInterface<TInterface>(this Type type)
        {
            if (type == null) return false;

            foreach (var it in type.GetInterfaces())
                if (it == typeof (TInterface)) return true;
            return false;
        }
    }
}
