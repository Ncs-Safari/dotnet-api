namespace UserManagement.Application.Dtos
{
	public class ApplGalleryDto
	{
		public string Id { get; set; }
		public string Title { get; set; }
		public string Description { get; set; }
		public string Type { get; set; }
		public string FileThumbnail { get; set; }
		public string FileGallery { get; set; }
		public bool IsBanner { get; set; }
		public bool IsSlider { get; set; }
		public bool IsApprove { get; set; }
		public string CreatedDate { get; set; }
		public string FileThumbnailBase64 { get; set; }
		public string FileGalleryBase64 { get; set; }

	}
}