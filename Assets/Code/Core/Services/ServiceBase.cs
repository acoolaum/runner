namespace Acoolaum.Core.Services
{
    public class ServiceBase : IService
    {
        protected ServiceContainer ServiceContainer;
        
        void IService.Initialize(ServiceContainer serviceContainer)
        {
            ServiceContainer = serviceContainer;
        }
    }
}