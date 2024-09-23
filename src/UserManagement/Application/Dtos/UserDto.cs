namespace UserManagement.Application.Dtos
{
	public class UserDto {
		public string Id { get; set; }
		public string Username { get; set; }
		public string Password { get; set; }
		public string Email { get; set; }
		public string FirstName { get; set; }
		public string MiddleName { get; set; }
		public string LastName { get; set; }
		public string Address { get; set; }
		public string PhoneNumber { get; set; }
		public string MobileNumber { get; set; }
		public string Orgid { get; set; }
		public int Status { get; set; }
		public string? LastLogin { get; set; }
		public string CreatedBy { get; set; }
		public string CreatedDate { get; set; }
		public string UpdatedBy { get; set; }
		public string? UpdatedDate { get; set; }
        public string FullName { get; set; }
        public IEnumerable<RoleDto>? Roles { get; set; }
        public IEnumerable<UserFileDto>? Files { get; set; }

	}
}