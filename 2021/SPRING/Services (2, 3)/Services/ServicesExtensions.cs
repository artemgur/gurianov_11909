using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using Microsoft.Extensions.DependencyInjection;

namespace Services
{
    public static class ServicesExtensions
    {
        private static readonly Type type = typeof(ServiceCollectionServiceExtensions);
        
        private static Dictionary<string, MethodInfo> methodInfos = new()
        {
            {"transient", type.GetMethods().Single(x => x.Name == "AddTransient" && x.GetParameters().Length == 1 && x.GetGenericArguments().Length == 2)},
            /*{"scoped", type.GetMethod("AddScoped")},
            {"singleton", type.GetMethod("AddSingleton")}*/
        };
        
        public static void AddServicesFromConfig(this IServiceCollection services, string filename)
        {
            foreach (var line in File.ReadLines(filename))
            {
                var a = line.Split(';');
                var interfaceType = Type.GetType(a[0]);
                var classType = Type.GetType(a[1]);
                var injectType = a[2];//transient, scoped or singleton
                var method = methodInfos[injectType].MakeGenericMethod(interfaceType, classType);
                method.Invoke(null, new object[]{services});
            }
        }
    }
}