using System;
using System.Threading;
using System.Threading.Tasks;
using FluentValidation.Results;

namespace SimpleSoft.Mediator
{
    /// <summary>
    /// Validation pipeline options
    /// </summary>
    public class ValidationPipelineOptions
    {
        /// <summary>
        /// Should commands be validated? Defaults to 'true'.
        /// </summary>
        public bool ValidateCommand { get; set; } = true;

        /// <summary>
        /// Should events be validated? Defaults to 'false'.
        /// </summary>
        public bool ValidateEvent { get; set; } = false;

        /// <summary>
        /// Should commands be validated? Defaults to 'false'.
        /// </summary>
        public bool ValidateQuery { get; set; } = false;

        /// <summary>
        /// Fail if the validator for a given class is not found? Defaults to 'true'.
        /// </summary>
        public bool FailIfValidatorNotFound { get; set; } = true;

        /// <summary>
        /// Invoked when a validation fails.
        /// </summary>
        public Func<object, ValidationResult, CancellationToken, Task> OnFailedValidation { get; set; }
    }
}