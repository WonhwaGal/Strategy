using UnityEngine;

namespace Abstractions.Commands
{
    public interface ICommandExecutor
    {
        CommandExecuterType CommandType { get; }
        void ExecuteCommand(object command);
    }

    public abstract class CommandExecutorBase<T> : MonoBehaviour, ICommandExecutor where T : ICommand
    {
        public CommandExecuterType CommandType { get; protected set; }
        public void ExecuteCommand(object command) => ExecuteSpecificCommand((T)command);
        public abstract void ExecuteSpecificCommand(T command);
    }
    public enum CommandExecuterType
    {
        ProduceUnit = 0,
        Move = 1,
        Attack = 2,
        Patrol = 3,
        Stop = 4
    }
}


