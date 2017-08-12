namespace SimpleSoft.Mediator
{
    /// <summary>
    /// Represents a command
    /// </summary>
    public interface ICommand : IHaveIdentifier, IHaveCreatedMeta
    {

    }

    /// <summary>
    /// Represents a command with a result
    /// </summary>
    /// <typeparam name="TResult">The result type</typeparam>
    public interface ICommand<out TResult> : IHaveIdentifier, IHaveCreatedMeta
    {

    }
}