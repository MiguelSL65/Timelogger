using System;

namespace Timelogger.Domain.Exceptions;

public class EntityNotFoundException : Exception
{
    public EntityNotFoundException(int entityId, string entity)
        : base($"Entity '{entity}' with id {entityId} was not found.")
    {
    }
}