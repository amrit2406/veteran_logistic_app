using System;

namespace VeteranLogistics.Shared.Exceptions;

/// <summary>
/// Represents a business rule violation.
/// </summary>
public class BusinessException : Exception
{
    public BusinessException()
    {
    }

    public BusinessException(string message)
        : base(message)
    {
    }

    public BusinessException(string message, Exception innerException)
        : base(message, innerException)
    {
    }
}
