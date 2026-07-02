using System;

namespace VeteranLogistics.Shared.Exceptions;

/// <summary>
/// Thrown when an authenticated user does not have permission to perform an action.
/// </summary>
public class ForbiddenException : BusinessException
{
    public ForbiddenException()
    {
    }

    public ForbiddenException(string message)
        : base(message)
    {
    }

    public ForbiddenException(string message, Exception innerException)
        : base(message, innerException)
    {
    }
}
