namespace UserManagement.Application.Dtos
{
	public class ApplInfographicDto
	{
		public string Id { get; set; }
		public string Title { get; set; }
		public string Description { get; set; }
		public string FileThumbnail { get; set; }
		public string FileInfographic { get; set; }
		public bool IsApprove { get; set; }
		public string CreatedDate { get; set; }
		public string FileThumbnailBase64 { get; set; }
		public string FileInfographicBase64 { get; set; }
	}
}