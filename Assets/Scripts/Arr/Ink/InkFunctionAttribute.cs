using System;
using System.Reflection;

namespace Arr.Ink
{
    [AttributeUsage(AttributeTargets.Method)]
    public class InkFunctionAttribute : Attribute
    {
        //USE WITH EXTRA CARE, WILL LOAD EVERYTHING
        public static void BindAll(Action<MethodInfo> forEachMethods)
        {
            var assemblies = AppDomain.CurrentDomain.GetAssemblies();

            foreach (var assembly in assemblies)
            {
                var types = assembly.GetTypes();
                BindWithTypes(types, forEachMethods);
            }
        }
        
        public static void BindWithTypes(Type[] typesWithMethods, Action<MethodInfo> forEachMethods)
        {
            foreach (var type in typesWithMethods)
            {
                var methods = type.GetMethods(BindingFlags.Instance | BindingFlags.Static | BindingFlags.Public | BindingFlags.NonPublic);

                foreach (var method in methods)
                {
                    if (method.GetCustomAttribute<InkFunctionAttribute>() == null) continue;
                    forEachMethods.Invoke(method);
                }
            }
        }
    }
}