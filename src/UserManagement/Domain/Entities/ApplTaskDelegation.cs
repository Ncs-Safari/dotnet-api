using System.ComponentModel.DataAnnotations.Schema;
using Common.Entities;
namespace UserManagement.Domain.Entities
{
    [Table("appl_task_delegation", Schema = "usr")]
	public class ApplTaskDelegation : BaseEntity{
		[Column("appl_task_id")]
		public string ApplTaskId { get; set; }
		[Column("delegate_for")]
		public string DelegateFor { get; set; }
		[Column("delegate_by")]
		public string DelegateBy { get; set; }
		[Column("approved_by")]
		public string ApprovedBy { get; set; }
		[Column("start_date")]
		public DateTime StartDate { get; set; }
		[Column("end_date")]
		public DateTime EndDate { get; set; }
		[Column("status")]
		public int Status { get; set; }
		[Column("created_by")]
		public string CreatedBy { get; set; }
		[Column("created_date")]
		public DateTime CreatedDate { get; set; }
		[Column("updated_by")]
		public string UpdatedBy { get; set; }
		[Column("updated_date")]
		public DateTime? UpdatedDate { get; set; }
	}
}