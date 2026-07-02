using System;

namespace VeteranLogistics.Shared.Exceptions;

/// <summary>
/// Thrown when an entity cannot be found.
/// </summary>
public class NotFoundException : BusinessException
{
    public NotFoundException()
    {
    }

    public NotFoundException(string message)
        : base(message)
    {
    }

    public NotFoundException(string message, Exception innerException)
        : base(message, innerException)
    {
    }
}
