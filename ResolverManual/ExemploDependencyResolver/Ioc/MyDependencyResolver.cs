using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Web.Mvc;

namespace ExemploDependencyResolver.Ioc
{
    public class MyDependencyResolver : IDependencyResolver
    {
        private static IDictionary<Type, IEnumerable<Dependency>> services = new Dictionary<Type, IEnumerable<Dependency>>();
        private IEnumerable<Dependency> injectedDependecies;

        public MyDependencyResolver()
        {
            Initialize();
        }

        private void Initialize()
        {
            var types = AppDomain.CurrentDomain.GetAssemblies().ToList().SelectMany(s => s.GetTypes()).Where(p => typeof(IDependencyRegister).IsAssignableFrom(p));
            foreach (var type in types)
            {
                if (type.IsClass)
                {
                    IDependencyRegister dependencyRegister = Activator.CreateInstance(type) as IDependencyRegister;
                    dependencyRegister.RegisterDependency(this);
                }
            }
        }

        public void Register<T>(IEnumerable<Dependency> dependencies)
        {
            services.Add(typeof(T), dependencies);
        }

        public object GetService(Type serviceType)
        {
            try
            {
                injectedDependecies = services[serviceType];
            }
            catch (KeyNotFoundException)
            {
                if (serviceType != typeof(IControllerFactory) && serviceType != typeof(IControllerActivator) 
                    && serviceType != typeof(IViewPageActivator) && serviceType.Namespace != "ASP")
                    throw new InvalidOperationException("The service was not registered.");
                else
                    return null;
            }

            var ctorInfo = serviceType.GetConstructors().FirstOrDefault();
            var paramInfo = ctorInfo.GetParameters();
            var resolvedParams = ResolveParamTypes(paramInfo);
            var resolveDependencies = ResolveDependencies(resolvedParams);
            var resolvedParamDependencies = ResolveParamDependencies(resolveDependencies);

            return Activator.CreateInstance(serviceType, resolvedParamDependencies.ToArray());
        }

        public IEnumerable<object> GetServices(Type serviceType)
        {
            return new List<object>();
        }

        private IEnumerable<Type> ResolveParamTypes(ParameterInfo[] paramInfo)
        {
            foreach (var param in paramInfo)
            {
                yield return param.ParameterType;
            }
        }

        private IEnumerable<Dependency> ResolveDependencies(IEnumerable<Type> resolvedParams)
        {
            return resolvedParams
                .Select(paramType => injectedDependecies.Where(dependency => dependency.Component == paramType)
                .FirstOrDefault());
        }

        private IEnumerable<object> ResolveParamDependencies(IEnumerable<Dependency> dependencies)
        {
            foreach (var dependency in dependencies)
                yield return dependency.ImplementedBy;
        }
    }

    public class Dependency
    {
        private Dependency(Type component, object implementedBy)
        {
            this.Component = component;
            this.ImplementedBy = implementedBy;
        }

        public static Dependency CreateDependency<T>(T implementedBy)
        {
            return new Dependency(typeof(T), implementedBy);
        }

        public Type Component { get; set; }
        public object ImplementedBy { get; set; }
    }
}