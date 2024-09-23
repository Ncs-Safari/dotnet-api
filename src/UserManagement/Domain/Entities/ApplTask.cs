using System.ComponentModel.DataAnnotations.Schema;
using Common.Entities;
namespace UserManagement.Domain.Entities
{
	[Table("appl_task", Schema = "usr")]
	public class ApplTask : BaseEntity
	{
		[Column("appl_task_parent_id")]
		public string ApplTaskParentId { get; set; }
		[Column("appl_id")]
		public string ApplId { get; set; }
		[Column("index_no")]
		public int IndexNo { get; set; }
		[Column("task_name")]
		public string TaskName { get; set; }
		[Column("controller_name")]
		public string ControllerName { get; set; }
		[Column("action_name")]
		public string ActionName { get; set; }
		[Column("description")]
		public string Description { get; set; }
		[Column("icon_name")]
		public string IconName { get; set; }
		[Column("custom_id")]
		public string CustomId { get; set; }
		[Column("status")]
		public int Status { get; set; }

		public List<ApplTask> Children { get; set; }
		public Appl Appl { get; set; }
		public void SetTr(ApplTaskTr Tr)
		{
			if (Tr != null)
			{
				this.TaskName = Tr.TaskName;
				this.Description = Tr.Description;
			}
		}
	}
}