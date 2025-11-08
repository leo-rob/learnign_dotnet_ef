using System;
using System.Collections.Generic;
using ConsoleApp1_EntityFramework_Scaffolding.Models;

namespace ConsoleApp1_EntityFramework_Scaffolding;

public partial class Task
{
    public int Id { get; set; }
    public string Title { get; set; } = null!;
    public string? Description { get; set; }
    public DateOnly? DueDate { get; set; }
    
    
    public int? StatusId { get; set; }
    public Status? Status { get; set; }

    public int? PriorityId { get; set; }
    public Priority? Priority { get; set; }

    public int? AssignedToUserId { get; set; }
    public virtual User? AssignedToUser { get; set; }

    public int? ProjectId { get; set; }
    public virtual Project? Project { get; set; }

    public virtual ICollection<TaskAttachment> TaskAttachments { get; set; } = new List<TaskAttachment>();
}
