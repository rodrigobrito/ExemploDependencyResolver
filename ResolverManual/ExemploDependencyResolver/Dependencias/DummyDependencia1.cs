using ExemploDependencyResolver.Controllers;

namespace ExemploDependencyResolver.Dependencias
{
    public class DummyDependencia1 : IDependencia1
    {
        public bool Metodo(string parametro)
        {
            return parametro.Equals("foo");
        }
    }
}