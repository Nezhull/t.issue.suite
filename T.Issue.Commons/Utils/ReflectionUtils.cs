using System;
using System.Reflection;

namespace T.Issue.Commons.Utils
{
    public static class ReflectionUtils
    {
        public static Assembly GetAssembly<T>()
        {
#if NETSTANDARD1_3
            return typeof(T).GetTypeInfo().Assembly;
#else
            return typeof(T).Assembly;
#endif
        }

        public static MethodInfo GetMethod<T>(string name, Type[] paramTypes)
        {
            Type hashType = typeof(T);

#if NETSTANDARD1_3
            return hashType.GetRuntimeMethod(name, paramTypes);
#else
            return hashType.GetMethod(name, paramTypes);
#endif
        }
    }
}