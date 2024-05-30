using Hwdtech;

namespace SpaceBattle.Lib
{
    public class InitSpaceshipsCommand : ICommand
    {
        private readonly List<IUObject> _uObjects;
        private readonly List<Vector> _positions;
        public InitSpaceshipsCommand(List<IUObject> uObjects, List<Vector> positions)
        {
            _uObjects = uObjects;
            _positions = positions;
        }

        public void Execute()
        {
            System.Collections.IEnumerator positionsEnumerator = _positions.GetEnumerator();

            _uObjects.ForEach(u =>
            {
                IoC.Resolve<ICommand>("Game.IUObject.SetProperty", u, "Position", positionsEnumerator.Current);
                positionsEnumerator.MoveNext();
            });

            positionsEnumerator.Reset();
        }
    }
}
