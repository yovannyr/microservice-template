
using Nancy;
using Infrastructure;


namespace Service
{
    public class HealthModule : NancyModule
    {

        public HealthModule()
        {
            var healthTopic = new Topic("health","health");
            var topic2 = new Topic("healthTyped", "healthTyped");

            Get["/health/{ping}"] = x =>
                {
                    healthTopic.Publish((string)x.ping);
                    topic2.Publish<HealthMessage>(new HealthMessage{WhoIsCalling = x.ping});
                    return "OK "+ x.ping;
                };
        }
    }
}
