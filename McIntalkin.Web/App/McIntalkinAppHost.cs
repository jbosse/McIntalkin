using ServiceStack.ServiceInterface.Validation;
using ServiceStack.WebHost.Endpoints;

namespace McIntalkin.Web.App
{
    public class McIntalkinAppHost : AppHostBase
    {
        public McIntalkinAppHost() : base("McIntalkin", typeof(Global).Assembly)
        {
        }

        public override void Configure(Funq.Container container)
        {
            container.RegisterAutoWiredAs<SayWhatRepository, ISayWhatRepository>();
            Plugins.Add(new ValidationFeature());
            container.RegisterValidators(typeof (Global).Assembly);
        }
    }
}