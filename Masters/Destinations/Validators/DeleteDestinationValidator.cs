using veteran_logistic.Masters.Destinations.Contracts;
using veteran_logistic.Masters.Destinations.Models;
using VeteranLogistics.Shared.Validation;

namespace veteran_logistic.Masters.Destinations.Validators;

/// <summary>
/// Validates delete destination requests to ensure required fields are present and valid.
/// </summary>
public sealed class DeleteDestinationValidator : IDeleteDestinationValidator
{
    /// <summary>
    /// Validates a delete destination request.
    /// </summary>
    /// <param name="request">The delete destination request to validate.</param>
    /// <returns>A ValidationResult indicating whether the request is valid.</returns>
    public ValidationResult Validate(DeleteDestinationRequest request)
    {
        var result = new ValidationResult();

        if (request is null)
        {
            result.AddError(new ValidationError(nameof(DeleteDestinationRequest), "Delete destination request cannot be null."));
            return result;
        }

        if (request.DestinationId <= 0)
        {
            result.AddError(new ValidationError(nameof(DeleteDestinationRequest.DestinationId), "Destination ID must be a positive value."));
        }

        return result;
    }
}
