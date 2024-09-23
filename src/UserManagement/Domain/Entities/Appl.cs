using System.ComponentModel.DataAnnotations.Schema;
using Common.Entities;
namespace UserManagement.Domain.Entities
{
    [Table("appl", Schema = "usr")]
	public class Appl : BaseEntity {
		[Column("code")]
		public string Code { get; set; }
		[Column("name")]
		public string Name { get; set; }
		[Column("description")]
		public string Description { get; set; }
		[Column("bgcolor")]
		public string Bgcolor { get; set; }
		[Column("iconfile")]
		public string Iconfile { get; set; }
		[Column("imagefile")]
		public string Imagefile { get; set; }
		public void SetTr(ApplTr Tr)
		{
			if (Tr != null)
			{
				this.Name = Tr.Name;
				this.Description = Tr.Description;
			}
		}
	}
}