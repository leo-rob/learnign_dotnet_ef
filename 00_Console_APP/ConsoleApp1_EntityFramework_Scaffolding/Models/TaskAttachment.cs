using System;
using System.Collections.Generic;

namespace ConsoleApp1_EntityFramework_Scaffolding;

public partial class TaskAttachment
{
    public int Id { get; set; }

    public string FileName { get; set; } = null!;

    public byte[]? FileData { get; set; }

    public int? TaskId { get; set; }

    public virtual Task? Task { get; set; }
}
