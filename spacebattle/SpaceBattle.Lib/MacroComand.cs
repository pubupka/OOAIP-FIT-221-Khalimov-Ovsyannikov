namespace SpaceBattle.Lib
{
    public class MacroComand
    {
        private readonly List<ICommand> _cmds;

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
