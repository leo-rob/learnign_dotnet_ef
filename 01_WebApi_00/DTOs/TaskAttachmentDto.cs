
namespace Core.Dtos
{
	public record TaskAttachmentDto
	{
		public int Id { get; set; }
		public string? FileName { get; set; }
		public string? FileData { get; set; }
		
		public TaskDto? Task { get; set; }
		
		
		public static TaskAttachmentDto From(Entities.TaskAttachment attachment)
		{
			if (attachment == null) return null!;

			return new TaskAttachmentDto
			{
				Id = attachment.Id,
				FileName = attachment.FileName,
				FileData = attachment.FileData,
				Task = TaskDto.From(attachment.Task)
			};
		}
	}
}