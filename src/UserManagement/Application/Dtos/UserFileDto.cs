namespace UserManagement.Application.Dtos
{
	public class UserFileDto
	{
		public string Id { get; set; }
		public string Type { get; set; }
		public string Category { get; set; }
		public string Title { get; set; }
		public string Description { get; set; }
		public string FileName { get; set; }
		public string FileThumbnail { get; set; }
		public string FileNameBase64 { get; set; }
		public string FileThumbnailBase64 { get; set; }

	}
}