using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Text.RegularExpressions;

namespace HwaTec.Blog.DependencyManagement
{
    public static class DependencyRegissterExtensions
    {
        public static void AddDependencyRegister(this IServiceCollection services, string assemblyPattern)
        {
            services.AddDependencyRegister(assemblyPattern, DependencyContext.Default);
        }
        public static void AddDependencyRegister(this IServiceCollection services, string assemblyPattern, params Assembly[] assemblies)
        {
            AddDependencyRegisterClass(services, assemblyPattern, assemblies);
        }

        public static void AddDependencyRegister(this IServiceCollection services, string assemblyPattern, DependencyContext dependencyContext)
        {
            services.AddDependencyRegister(assemblyPattern,
                dependencyContext.RuntimeLibraries
                    .SelectMany(lib => lib.GetDefaultAssemblyNames(dependencyContext).Select(Assembly.Load))
                    .Where(x => Regex.IsMatch(x.FullName, assemblyPattern, RegexOptions.Compiled))
                    .ToArray());
        }

        private static void AddDependencyRegisterClass(IServiceCollection services, string assemblyPattern, IEnumerable<Assembly> assembliesToScan)
        {
            assembliesToScan = assembliesToScan as Assembly[] ?? assembliesToScan.ToArray();

            var allTypes = assembliesToScan.SelectMany(a => a.ExportedTypes).ToArray();

            var dependencyRegisters =
                allTypes
                .Where(t => !t.GetTypeInfo().IsAbstract && t.GetInterfaces().SingleOrDefault(x => x == typeof(IDependencyRegister)) != null)
                .ToList();

            var serviceProvider = services.BuildServiceProvider();
            //根据执行优先级排序
            var registerInstances = dependencyRegisters.Select(service => (IDependencyRegister)serviceProvider.TryGetService(service)).OrderByDescending(x => x.ExecuteOrder);

            foreach (var instance in registerInstances)
            {
                var registerMethod = instance.GetType().GetMethod("Register");
                var methodParameters = registerMethod.GetParameters().Select(parm => serviceProvider.GetService(parm.ParameterType)).ToArray();
                registerMethod.Invoke(instance, methodParameters);
            }

            ConvenientRegister(services, allTypes);
        }

        const string ServiceTypeSuffix = "Service";
        const string RepositoryTypeSuffix = "Repository";

        private static void ConvenientRegister(IServiceCollection services, Type[] allTypes)
        {
            ConvenientRegisterFor<IScopedDependency>(services, allTypes, ServiceTypeSuffix, RepositoryTypeSuffix);
            ConvenientRegisterFor<ITransistDependency>(services, allTypes, ServiceTypeSuffix, RepositoryTypeSuffix);
            ConvenientRegisterFor<ISignaltonDependency>(services, allTypes, ServiceTypeSuffix, RepositoryTypeSuffix);
        }

        private static void ConvenientRegisterFor<TDenpendencyInterface>(IServiceCollection services, Type[] allTypes, params string[] suffixList)
        {
            var scopedDependency = allTypes
                 .Where(t => !t.GetTypeInfo().IsAbstract && t.GetInterfaces().SingleOrDefault(x => x == typeof(TDenpendencyInterface)) != null)
                 .ToList();

            foreach (var item in scopedDependency)
            {
                var typeName = item.GetTypeInfo().Name;
                suffixList.ToList().ForEach(suffix =>
                {
                    if (typeName.EndsWith(suffix))
                    {
                        var interfaceList = item.GetInterfaces().Where(x => x.GetTypeInfo().Name.EndsWith(suffix) && !x.GetTypeInfo().Name.Contains("Base")).ToList();
                        foreach (var interfaceType in interfaceList)
                        {
                            services.AddScoped(item, interfaceType);
                        }

                        services.AddScoped(item);
                    }
                });
            }
        }

        public static object TryGetService(this IServiceProvider serviceProvider, Type type, bool throwExceptionIfReturnNull = true)
        {
            var constructors = type.GetConstructors();

            StringBuilder sbErrorMessage = new StringBuilder();

            foreach (var constructor in constructors)
            {
                try
                {
                    var parameters = constructor.GetParameters();
                    var parameterInstances = new List<object>();
                    foreach (var parameter in parameters)
                    {
                        var service = serviceProvider.GetService(parameter.ParameterType);
                        if (service == null) throw new Exception($"Can't reslove Type {parameter.ParameterType} when Create {type.FullName} object");
                        parameterInstances.Add(service);
                    }
                    return Activator.CreateInstance(type, parameterInstances.ToArray());
                }
                catch (Exception ex)
                {
                    //sbErrorMessage.AppendLine(ex.FullMessage());
                }
            }
            if (throwExceptionIfReturnNull)
            {
                throw new Exception(sbErrorMessage.ToString());
            }
            return null;
        }

        public static T TryGetService<T>(this IServiceProvider serviceProvider)
        {
            return (T)serviceProvider.TryGetService(typeof(T));
        }

        private static bool ImplementsGenericInterface(this Type type, Type interfaceType)
        {
            if (type.IsGenericType(interfaceType))
            {
                return true;
            }
            foreach (var @interface in type.GetTypeInfo().ImplementedInterfaces)
            {
                if (@interface.IsGenericType(interfaceType))
                {
                    return true;
                }
            }
            return false;
        }

        private static bool IsGenericType(this Type type, Type genericType)
        {
            return type.GetTypeInfo().IsGenericType && type.GetGenericTypeDefinition() == genericType;
        }

    }
}
