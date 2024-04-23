// using SpaceBattle.Lib.ICommand;
// using SpaceBattle.Lib.SendCommand;
// using Hwdtech;

// namespace Endpoint
// {
//     public class InitEndpointDependenciesCommand : ICommand
//     {
//         public InitEndpointDependenciesCommand() => { }

//         public void Execute()
//         {
//             IoC.Resolve<Hwdtech.ICommand>(
//                 "IoC.Register",
//                 "Build Command From Message",
//                 (object[] args) => 
//                 {
                    
//                 }
//             ).Execute();

//         }
//     }
// }
