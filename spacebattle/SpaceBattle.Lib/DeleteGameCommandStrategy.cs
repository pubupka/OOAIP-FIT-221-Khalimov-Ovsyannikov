using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SpaceBattle.Lib
{
    public class DeleteGameCommandStrategy
    {
        public object Invoke(params object[] args)
        {
            string gameId = (string)args[0];

            return new DeleteGameCommand(gameId);
        }
    }
}