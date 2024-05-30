using Hwdtech;

namespace SpaceBattle.Lib
{
    public class SetFuelToSpaceshipsCommand : ICommand
    {
        private readonly List<IUObject> _uObjects;
        private readonly List<int> _fuelCapacities;
        public SetFuelToSpaceshipsCommand(List<IUObject> uObjects, List<int> fuelCapacities)
        {
            _uObjects = uObjects;
            _fuelCapacities = fuelCapacities;
        }

        public void Execute()
        {
            IEnumerator<int> fuelCapacitiesEnumerator = _fuelCapacities.GetEnumerator();

            _uObjects.ForEach(u =>
            {
                IoC.Resolve<ICommand>("Game.IUObject.SetProperty", u, "FuelCapacity", fuelCapacitiesEnumerator.Current).Execute();
                fuelCapacitiesEnumerator.MoveNext();
            });

            fuelCapacitiesEnumerator.Reset();
        }
    }
}
