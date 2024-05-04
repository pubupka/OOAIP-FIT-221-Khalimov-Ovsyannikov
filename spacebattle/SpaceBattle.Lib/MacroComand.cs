using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SpaceBattle.Lib
{
    public class MacroComand
    {
        public List<ICommand> _cmds;

        public MacroComand(List<ICommand> cmds)
        {
            _cmds = cmds;
        }

        public void Execute()
        {
            _cmds.ForEach(cmd => cmd.Execute());
        }
    }
}