using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;


namespace Core.Entities
{	
	[Table("task_attachments")]
	public record TaskAttachment
	{
		[Key, Column("id")]
		public int Id { get; set; }
		[Column("file_name")]
		public required string FileName { get; set; }
		[Column("file_data")]
		public required string FileData { get; set; }
		
		[Column("task_id")] public int TaskId { get; set; }
		[ForeignKey(nameof(TaskId))] public Task? Task { get; set; }
	}
}