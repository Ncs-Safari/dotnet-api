namespace UserManagement.Application.Dtos
{
	public class ApplTaskDelegationDto
	{
		public string Id { get; set; }
		public string ApplTaskId { get; set; }
		public string DelegateFor { get; set; }
		public string DelegateBy { get; set; }
		public string ApprovedBy { get; set; }
		public string StartDate { get; set; }
		public string EndDate { get; set; }
		public int Status { get; set; }
		public string CreatedBy { get; set; }
		public string CreatedDate { get; set; }
		public string UpdatedBy { get; set; }
		public string? UpdatedDate { get; set; }
	}
}