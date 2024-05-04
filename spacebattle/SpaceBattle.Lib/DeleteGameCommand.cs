using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SpaceBattle.Lib
{
    public class DeleteGameCommand:ICommand
    {
        string _gameId;
        public DeleteGameCommand(string gameId)
        {
            _gameId = gameId;
        }
        public void Execute()
        {

        }
    }
}