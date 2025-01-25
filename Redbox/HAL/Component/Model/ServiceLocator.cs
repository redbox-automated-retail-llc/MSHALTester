using System;
using System.ComponentModel.Design;


namespace Redbox.HAL.Component.Model
{
    public sealed class ServiceLocator : IServiceProvider
    {
        private readonly ServiceContainer Services = new ServiceContainer();
        private static readonly ServiceLocator m_instance = new ServiceLocator();

        public void AddService<T>(object instance) => this.AddService(typeof(T), instance);

        public void AddService(Type serviceType, object instance)
        {
            this.Services.AddService(serviceType, instance);
        }

        public T GetService<T>() => (T)this.GetService(typeof(T));

        public object GetService(Type serviceType) => this.Services.GetService(serviceType);

        public void RemoveService<T>() => this.Services.RemoveService(typeof(T));

        public void RemoveService(Type serviceType) => this.Services.RemoveService(serviceType);

        public static ServiceLocator Instance => ServiceLocator.m_instance;

        private ServiceLocator()
        {
        }
    }
}
