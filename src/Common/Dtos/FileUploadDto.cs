using Microsoft.AspNetCore.Http;

namespace Common.Dtos
{
    public class FileUploadDto
    {
		public string? Category { get; set; }
		public string Title { get; set; }
		public string? Description { get; set; }
		public string Type { get; set; }
        public IFormFile FileUpload { get; set; }
    }
}


