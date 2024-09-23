using AutoMapper;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Localization;
using UserManagement.Application.Dtos;
using UserManagement.Application.Interfaces;
using Common.Interfaces;
using Common.Exceptions;
using Common.Extensions;
using UserManagement.Domain.Entities;
using Common.Constants;
using UserManagement.Application.Configuration;
using Common.Dtos;
using UserManagement.Application.Resources;
using Common.ValueObjects;
namespace UserManagement.Application.Commands
{
    public class UserAddCommand : IRequest<UserDto>
    {
        public string? UserId { get; set; }
        public string Username { get; set; }
        public string Password { get; set; }
        public string Email { get; set; }
        public string FirstName { get; set; }
        public string? MiddleName { get; set; }
        public string? LastName { get; set; }
        public string Address { get; set; }
        public string? PhoneNumber { get; set; }
        public string? MobileNumber { get; set; }
        public List<FileUploadDto> Files { get; set; }
        public List<string> Roles { get; set; }
        public bool? IsRegister { get; set; }
    }
    internal class UserAddCommandHandler : IRequestHandler<UserAddCommand, UserDto>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        private readonly IJwtTokenGenerator _jwtTokenGenerator;
        private readonly IEmailService _emailService;
        private readonly IStringLocalizer<MessagesResource> _messageLocalizer;
        private readonly UserPreferences _userPreferences;
        public UserAddCommandHandler(
            IUnitOfWork unitOfWork,
            IMapper mapper,
            IJwtTokenGenerator jwtTokenGenerator,
            IEmailService emailService,
            IStringLocalizer<MessagesResource> messageLocalizer,
            UserPreferences userPreferences)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _jwtTokenGenerator = jwtTokenGenerator;
            _emailService = emailService;
            _messageLocalizer = messageLocalizer;
            _userPreferences = userPreferences;
        }

        public async Task<UserDto> Handle(UserAddCommand request, CancellationToken cancellationToken)
        {
            var user = await _unitOfWork.Users.GetUser(request.Username);
            if (user is not null)
                throw new ApplicationException(_messageLocalizer["Error.User.UsernameRegistered"]);
            user = await _unitOfWork.Users.GetUser(request.Email);
            if (user is not null)
                throw new ApplicationException(_messageLocalizer["Error.User.EmailRegistered"]);
            User userAdd = _mapper.Map<User>(request);
            string password = await _jwtTokenGenerator.GeneratePassword();
            userAdd.Password = password;
            userAdd.Password = userAdd.Password.EncryptString();
            if (string.IsNullOrEmpty(request.UserId))
                userAdd.CreatedBy = userAdd.Id;
            List<Role> roles = [];
            foreach (var role in request.Roles)
            {
                roles.Add(new() { Id = role });
            }
            if (roles.Count > 0)
                userAdd.Roles = roles;
            List<UserFile> userFiles = [];
            Dictionary<string, IFormFile> fileToUploads = [];
            foreach (var file in request.Files)
            {
                string fileName = $"{userAdd.Id}_{DateTime.Now.ToString("yyyyMMddHHmmss")}{Path.GetExtension(file.FileUpload.FileName)}";
                UserFile userFile = new UserFile
                {
                    Id = userAdd.Id,
                    Type = file.Type,
                    Category = file.Category,
                    Description = file.Description,
                    Title = file.Title,
                    FileName = fileName
                };

                if (file.Type == FileTypeConstant.Image)
                {
                    //generate thumbnail
                }
                userFiles.Add(userFile);
                fileToUploads.Add(fileName, file.FileUpload);
            }
            userAdd.Files = userFiles;
            UserDto ret = new();
            _unitOfWork.Users.Add(userAdd, result =>
                {
                    if (result is not null)
                    {
                        //save file
                        foreach (var item in fileToUploads)
                        {
                            using (Stream fileStream = new FileStream($"{DirectorySetting.PathFileUser}/{item.Key}", FileMode.Create, FileAccess.Write))
                            {
                                item.Value.CopyTo(fileStream);
                            }
                        }
                        ret = _mapper.Map<UserDto>(result);

                        if (request.IsRegister is not null)
                        {
                            if (request.IsRegister == true)
                            {
                                string strBody = System.IO.File.ReadAllText(AppDomain.CurrentDomain.BaseDirectory + $"/Resources/Templates/Email/HeaderEmail-{_userPreferences.LanguageId}.html");
                                strBody += System.IO.File.ReadAllText(AppDomain.CurrentDomain.BaseDirectory + $"/Resources/Templates/Email/UserRegistration-{_userPreferences.LanguageId}.html");
                                strBody += System.IO.File.ReadAllText(AppDomain.CurrentDomain.BaseDirectory + $"/Resources/Templates/Email/FooterEmail-{_userPreferences.LanguageId}.html");
                                strBody = strBody.Replace("[FULLNAME]", result.FullName).Replace("[EMAIL]", result.Email).Replace("[USERNAME]", result.Username);
                                EmailMessageDto message = new()
                                {
                                    To = [request.Email],
                                    Cc = [],
                                    Subject = _messageLocalizer["Registration.Email.Subject"],
                                    Attachments = [],
                                    Content = strBody
                                };
                                _emailService.SendHtmlEmailAsync(message);
                            }
                        }
                    }
                });
            return ret;
        }
    }
}