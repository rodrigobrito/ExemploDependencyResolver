using System.Collections.Generic;
using ExemploDependencyResolver.Controllers;
using ExemploDependencyResolver.Dependencias;
using ExemploDependencyResolver.Ioc;

namespace ExemploDependencyResolver.Dependencies
{
    public class TemDependenciaRegister : IDependencyRegister
    {
        public void RegisterDependency(MyDependencyResolver container)
        {
            var dependencies = new List<Dependency>
            {
                Dependency.CreateDependency<IDependencia1>(new DummyDependencia1()),
                Dependency.CreateDependency<IDependencia2>(new DummyDependencia2())
            };
            container.Register<TemDependenciasController>(dependencies);
        }
    }
}
