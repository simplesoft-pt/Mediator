namespace SimpleSoft.Mediator.Pipeline
{
    /// <summary>
    /// Filter that is run on every handling event
    /// </summary>
    public interface IHandlingFilter : IHandlingExecutingFilter, IHandlingExecutedFilter, IHandlingFailedFilter
    {

    }
}
