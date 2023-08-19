using System;
using System.Collections.Generic;
using Timelogger.Infrastructure.Persistence.Common;

namespace Timelogger.Infrastructure.Persistence.Entities
{
	public class ProjectEntity : BaseEntity
	{
		public int Id { get; set; }
		public int FreelancerId { get; set; }
		public FreelancerEntity Freelancer { get; set; }
		public string Name { get; set; }
		public string CompanyName { get; set; }
		public DateTimeOffset Deadline { get; set; }
		public bool IsCompleted { get; set; }
		
        public ICollection<TimeRegistrationEntity> TimeRegistrations { get; set; }
	}
}
