using System;

namespace ExemploDependencyResolver.Ioc
{
    public interface IDependencyRegister
    {
        void RegisterDependency(MyDependencyResolver container);
    }
}
