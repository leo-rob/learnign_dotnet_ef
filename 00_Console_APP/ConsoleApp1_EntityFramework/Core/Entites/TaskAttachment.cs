using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;


namespace Core.Entites
{	
	[Table("task_attachments")]
	public record TaskAttachment
	{
		[Key, Column("id")]
		public int Id { get; init; }
		[Column("file_name")]
		public required string FileName { get; init; }
		[Column("file_data")]
		public required string FileData { get; init; }
		
		[Column("task_id")]
		public int TaskId { get; init; }
		public Task? Task { get; init; }
	}
}