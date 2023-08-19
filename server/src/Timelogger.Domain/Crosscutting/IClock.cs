using System;

namespace Timelogger.Domain.Crosscutting;

public interface IClock
{
    DateTimeOffset UtcNow { get; }
}