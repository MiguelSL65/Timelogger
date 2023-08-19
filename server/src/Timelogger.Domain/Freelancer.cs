using System.Collections.Generic;

namespace Timelogger.Domain;

public class Freelancer
{
    public int Id { get; }
    public string Name { get; }
    public string Email { get; }
    public IEnumerable<Project> Projects { get; }

    public Freelancer(int id, string name, string email, IEnumerable<Project> projects)
    {
        Id = id;
        Name = name;
        Email = email;
        Projects = projects;
    }
}