using Microsoft.EntityFrameworkCore;
using Timelogger.Infrastructure.Persistence.Entities;

namespace Timelogger.Infrastructure.Persistence
{
	public class ApiContext : DbContext
	{
		public ApiContext(DbContextOptions<ApiContext> options)
			: base(options)
		{
		}

		public DbSet<FreelancerEntity> Freelancers { get; set; }
		public DbSet<ProjectEntity> Projects { get; set; }
		public DbSet<TimeRegistrationEntity> TimeRegistrations { get; set; }
	}
}
