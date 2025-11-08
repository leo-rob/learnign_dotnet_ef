using System;
using System.Collections.Generic;

namespace ConsoleApp1_EntityFramework_Scaffolding;

public partial class Project
{
    public int Id { get; set; }

    public string Name { get; set; } = null!;

    public string? Description { get; set; }

    public DateOnly? StartDate { get; set; }

    public DateOnly? EndDate { get; set; }

    public int? CategoryId { get; set; }

    public int? ManagerId { get; set; }

    public virtual ProjectCategory? Category { get; set; }

    public virtual User? Manager { get; set; }

    public virtual ICollection<Task> Tasks { get; set; } = new List<Task>();
}
