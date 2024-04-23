using System;
using CoreWCF;
using Hwdtech;

namespace WebHttp
{
    [ServiceBehavior(IncludeExceptionDetailInFaults = true)]
    public class Endpoint : IWebApi
    {
        public int ProcessMessage(MessageContract message)
        {
            try
            {
                var cmd = IoC.Resolve<ICommand>("Build Command From Message", message);
                var threadId = (int)IoC.Resolve<object>("Get Thread Id By Game Id", message.GameId);
                IoC.Resolve<ICommand>("Send Command", threadId, cmd).Execute();
                return 202;
            }
            catch (Exception ex)
            {
                IoC.Resolve<ICommand>("EndPoint.Exception.Handle", ex).Execute();
                return 0;
            }
        }
    }
}
