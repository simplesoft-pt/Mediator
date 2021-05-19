namespace SimpleSoft.Mediator
{
    /// <summary>
    /// SimpleSoft Database pipeline options
    /// </summary>
    public class DatabaseTransactionPipelineOptions
    {
        /// <summary>
        /// Should a transaction be open on commands? Defaults to 'false'.
        /// </summary>
        public bool BeginTransactionOnCommand { get; set; } = false;

        /// <summary>
        /// Should a transaction be open on events? Defaults to 'false'.
        /// </summary>
        public bool BeginTransactionOnEvent { get; set; } = false;

        /// <summary>
        /// Should a transaction be open on queries? Defaults to 'false'.
        /// </summary>
        public bool BeginTransactionOnQuery { get; set; } = false;

        /// <summary>
        /// When using transactions on queries (<see cref="BeginTransactionOnQuery"/>),
        /// should a rollback be made to prevent changes? Defaults to 'false'.
        /// </summary>
        public bool ForceRollbackOnQuery { get; set; } = false;
    }
}
