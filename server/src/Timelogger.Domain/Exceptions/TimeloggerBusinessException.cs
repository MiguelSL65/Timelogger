using System;
using System.Collections.Generic;

namespace Timelogger.Domain.Exceptions;

public class TimeloggerBusinessException : Exception
{
    public List<string> Errors { get; } = new();

    public TimeloggerBusinessException(
        IEnumerable<string> errors)
    {
        Errors.AddRange(errors);
    }

    public TimeloggerBusinessException(
        string message)
    {
        Errors.Add(message);
    }
}