using System;

namespace Timelogger.Infrastructure.Persistence.Common;

public class BaseEntity : IAuditableEntity
{
    public DateTimeOffset CreatedAtUct { get; set; }
    public bool IsActive { get; set; }
}