using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Hwdtech;

namespace SpaceBattle.Lib
{
    public class MessageProcessingCommand:ICommand
    {
        IProcessable _message;
        public MessageProcessingCommand(IProcessable message)
        {
            _message = message;
        }

        public void Execute()
        {
            var cmd = IoC.Resolve<ICommand>("Server.ProcessedCommand", _message);
            var id = _message.gameId;

            IoC.Resolve<ICommand>("Server.Game.Queue.PushByID", id, cmd).Execute();
        }
    }
}
