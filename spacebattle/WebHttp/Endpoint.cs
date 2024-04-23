using CoreWCF;
using Hwdtech;
using System;

namespace WebHttp
{
    [ServiceBehavior(IncludeExceptionDetailInFaults = true)]
    internal class Endpoint : IWebApi
    {
        public int ProcessMessage(MessageContract message)
        {
            try
            {
                var cmd = IoC.Resolve<ICommand>("Build Command From Message", message);
                var threadId = IoC.Resolve<int>("Get Thread Id By Game Id", message.GameId);
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
