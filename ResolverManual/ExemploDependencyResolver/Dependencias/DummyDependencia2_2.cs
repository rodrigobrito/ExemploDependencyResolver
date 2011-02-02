using ExemploDependencyResolver.Controllers;

namespace ExemploDependencyResolver.Dependencias
{
    public class DummyDependencia2_2 : IDependencia2
    {
        public void AlgumMetodo()
        {
            var a = 1;
            var b = 1;
            var c = a + b;
        }
    }
}