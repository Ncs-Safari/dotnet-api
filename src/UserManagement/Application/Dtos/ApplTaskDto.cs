
namespace UserManagement.Application.Dtos
{
	public class ApplTaskDto
	{
		public string Id { get; set; }
		public string ApplId { get; set; }
		public int IndexNo { get; set; }
		public string TaskName { get; set; }
		public string ControllerName { get; set; }
		public string ActionName { get; set; }
		public string Description { get; set; }
		public string IconName { get; set; }
		public string CustomId { get; set; }
		public int Status { get; set; }
	}
}