using CoreWCF;
using Hwdtech;

namespace WebHttp
{
    [ServiceBehavior(IncludeExceptionDetailInFaults = true)]
    public class Endpoint : IWebApi
    {
        public void ProcessMessage(MessageContract message)
        {
            var cmd = IoC.Resolve<ICommand>("Build Command From Message", message);
            var threadId = (int)IoC.Resolve<object>("Get Thread Id By Game Id", message.GameId);
            IoC.Resolve<ICommand>("Send Command", threadId, cmd).Execute();
        }
    }
}
