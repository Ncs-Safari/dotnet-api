namespace UserManagement.Application.Dtos
{
	public class ApplNewsDto {
		public string Id { get; set; }
		public string ApplNewsCategoryId { get; set; }
		public string Title { get; set; }
		public string Description { get; set; }
		public string Type { get; set; }
		public string Header { get; set; }
		public string FileThumbnail { get; set; }
		public string FileNews { get; set; }
		public bool IsApprove { get; set; }
		public string CreatedDate { get; set; }
		public string FileThumbnailBase64 { get; set; }
		public string FileNewsBase64 { get; set; }
	}
}