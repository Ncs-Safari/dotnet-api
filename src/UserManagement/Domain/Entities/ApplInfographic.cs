using System.ComponentModel.DataAnnotations.Schema;
using Common.Entities;
namespace UserManagement.Domain.Entities
{
    [Table("appl_infographic", Schema = "usr")]
	public class ApplInfographic : BaseEntity{
		[Column("title")]
		public string Title { get; set; }
		[Column("description")]
		public string Description { get; set; }
		[Column("file_thumbnail")]
		public string FileThumbnail { get; set; }
		[Column("file_infographic")]
		public string FileInfographic { get; set; }
		[Column("is_approve")]
		public bool IsApprove { get; set; }
		[Column("created_date")]
		public DateTime CreatedDate { get; set; }
	}
}