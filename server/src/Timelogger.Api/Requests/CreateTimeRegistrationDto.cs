using System;
using System.ComponentModel.DataAnnotations;

namespace Timelogger.Api.Requests;

public class CreateTimeRegistrationDto
{
    [MinLength(3)]
    [MaxLength(100)]
    public string Description { get; set; }
    
    [Required]
    public DateTimeOffset StartDate { get; set; }
    
    [Required]
    public DateTimeOffset EndDate { get; set; }
}