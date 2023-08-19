using System;

namespace Timelogger.Infrastructure.Persistence.Common;

public interface IAuditableEntity
{
    DateTimeOffset CreatedAtUct { get; set; }
}