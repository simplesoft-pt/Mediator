using System;

namespace SimpleSoft.Mediator
{
    /// <summary>
    /// Represents a data query
    /// </summary>
    /// <typeparam name="TResult">The result type</typeparam>
    public class Query<TResult> : IQuery<TResult>
    {
        /// <summary>
        /// Creates a new instance using <see cref="Guid.NewGuid"/> and
        /// <see cref="DateTimeOffset.Now"/> to set the default values.
        /// </summary>
        protected Query() : this(Guid.NewGuid(), DateTimeOffset.Now, null)
        {

        }

        /// <summary>
        /// Creates a new instance
        /// </summary>
        /// <param name="id">The unique identifier</param>
        /// <param name="createdOn">The date and time in which the instance was created</param>
        /// <param name="createdBy">The identifier for the user that created this instance</param>
        protected Query(Guid id, DateTimeOffset createdOn, string createdBy)
        {
            Id = id;
            CreatedOn = createdOn;
            CreatedBy = createdBy;
        }

        /// <inheritdoc />
        public Guid Id { get; set; }

        /// <inheritdoc />
        public DateTimeOffset CreatedOn { get; set; }

        /// <inheritdoc />
        public string CreatedBy { get; set; }
    }
}