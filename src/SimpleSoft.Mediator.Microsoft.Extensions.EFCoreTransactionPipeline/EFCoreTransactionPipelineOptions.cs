namespace SimpleSoft.Mediator
{
    /// <summary>
    /// Entity Framework Core pipeline options
    /// </summary>
    public class EFCoreTransactionPipelineOptions
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
    }
}