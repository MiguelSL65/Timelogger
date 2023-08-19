using System;
using Timelogger.Domain.Crosscutting;

namespace Timelogger.Infrastructure;

public class Clock : IClock
{
    public DateTimeOffset UtcNow => DateTimeOffset.UtcNow;
}