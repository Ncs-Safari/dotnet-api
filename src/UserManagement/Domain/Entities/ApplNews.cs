using System.ComponentModel.DataAnnotations.Schema;
using Common.Entities;
namespace UserManagement.Domain.Entities
{
    [Table("appl_news", Schema = "usr")]
	public class ApplNews : BaseEntity{
		[Column("appl_news_category_id")]
		public string ApplNewsCategoryId { get; set; }
		[Column("title")]
		public string Title { get; set; }
		[Column("description")]
		public string Description { get; set; }
		[Column("type")]
		public string Type { get; set; }
		[Column("header")]
		public string Header { get; set; }
		[Column("file_thumbnail")]
		public string FileThumbnail { get; set; }
		[Column("file_news")]
		public string FileNews { get; set; }
		[Column("is_approve")]
		public bool IsApprove { get; set; }
		[Column("created_date")]
		public DateTime CreatedDate { get; set; }
	}
}