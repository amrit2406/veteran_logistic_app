using System;

namespace VeteranLogistics.Shared.Exceptions;

/// <summary>
/// Thrown when authentication fails.
/// </summary>
public class UnauthorizedException : BusinessException
{
    public UnauthorizedException()
    {
    }

    public UnauthorizedException(string message)
        : base(message)
    {
    }

    public UnauthorizedException(string message, Exception innerException)
        : base(message, innerException)
    {
    }
}
