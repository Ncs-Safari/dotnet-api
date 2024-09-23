using System.ComponentModel.DataAnnotations.Schema;
using Common.Entities;
namespace UserManagement.Domain.Entities
{
    [Table("appl_gallery", Schema = "usr")]
	public class ApplGallery : BaseEntity{
		[Column("title")]
		public string Title { get; set; }
		[Column("description")]
		public string Description { get; set; }
		[Column("type")]
		public string Type { get; set; }
		[Column("file_thumbnail")]
		public string FileThumbnail { get; set; }
		[Column("file_gallery")]
		public string FileGallery { get; set; }
		[Column("is_banner")]
		public bool IsBanner { get; set; }
		[Column("is_slider")]
		public bool IsSlider { get; set; }
		[Column("is_approve")]
		public bool IsApprove { get; set; }
		[Column("created_date")]
		public DateTime CreatedDate { get; set; }
	}
}