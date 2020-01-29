using System.Reflection;
using Funq;
using ServiceStack;

namespace WebApplication1
{
    public class AppHost : AppHostBase
    {
        public AppHost(string serviceName, params Assembly[] assembliesWithServices) : base(serviceName, assembliesWithServices)
        {
        }

        public override void Configure(Container container)
        {
            var serviceInit = new AppHostCommon(this);
            serviceInit.Init();
        }
    }
}
