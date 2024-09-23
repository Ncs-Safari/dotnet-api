using Common.Dtos;

namespace UserManagement.Application.Dtos
{
    public class UserAddDto
    {
        public string Username { get; set; }
        public string Email { get; set; }
        public string FirstName { get; set; }
        public string? MiddleName { get; set; }
        public string? LastName { get; set; }
        public string Address { get; set; }
        public string? PhoneNumber { get; set; }
        public string? MobileNumber { get; set; }
        public List<FileUploadDto> Files{ get; set; }
        public List<string> Roles { get; set; }         
 
    }
}