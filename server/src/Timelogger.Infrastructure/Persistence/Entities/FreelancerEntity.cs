using System.Collections.Generic;
using Timelogger.Infrastructure.Persistence.Common;

namespace Timelogger.Infrastructure.Persistence.Entities;

public class FreelancerEntity : BaseEntity
{
    public int Id { get; set; }
    public string Name { get; set; }
    public string Email { get; set; }
    
    public ICollection<ProjectEntity> Projects { get; set; }
}