using C42A.Ra180.Infrastructure;
using Nancy;

namespace C42A.Ra180.Web
{
    public class KanaldataModule : NancyModule
    {
        public KanaldataModule()
        {
            Get["/KDA"] = parameters => View["kda"];
            Get["/KDA/generate-new-key"] = _ => Response.AsJson(PassiveKeyCalculator.Default.GenerateNewKey());
        }
    }
}