using System;

namespace SimpleSoft.Mediator
{
    /// <summary>
    /// This instance has an unique identifier
    /// </summary>
    public interface IHaveIdentifier
    {
        /// <summary>
        /// The unique identifier
        /// </summary>
        Guid Id { get; }
    }
}
