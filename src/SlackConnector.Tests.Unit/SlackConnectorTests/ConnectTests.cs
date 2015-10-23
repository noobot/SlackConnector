using SpecsFor;

namespace SlackConnector.Tests.Unit.SlackConnectorTests
{
    public static class ConnectTests
    {
         public class given_api_key_when_connecting_to_slack : SpecsFor<SlackConnector>
         {
             protected override void Given()
             {
                 base.Given();
             }
         }
    }
}