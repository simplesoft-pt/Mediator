using System;

namespace SimpleSoft.Mediator
{
    /// <summary>
    /// This instance has information about its creation
    /// </summary>
    public interface IHaveCreatedMeta
    {
        /// <summary>
        /// The date and time in which the instance was created
        /// </summary>
        DateTimeOffset CreatedOn { get; set; }

        /// <summary>
        /// The identifier for the user that created this instance
        /// </summary>
        string CreatedBy { get; set; }
    }
}