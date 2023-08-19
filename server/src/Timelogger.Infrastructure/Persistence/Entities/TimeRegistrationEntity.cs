using System;
using System.ComponentModel.DataAnnotations.Schema;
using Timelogger.Infrastructure.Persistence.Common;

namespace Timelogger.Infrastructure.Persistence.Entities;

public class TimeRegistrationEntity : BaseEntity
{
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public int Id { get; set; }
    public int ProjectId { get; set; }
    public ProjectEntity Project { get; set; }
    public string Description { get; set; }
    public DateTimeOffset StartDate { get; set; }
    public DateTimeOffset EndDate { get; set; }
    public double HoursLogged { get; set; }
}