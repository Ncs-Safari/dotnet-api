using AutoMapper;
using Common.Constants;
using Common.Dtos;
using Common.Exceptions;
using Common.Extensions;
using Common.Interfaces;
using Common.ValueObjects;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Localization;
using UserManagement.Application.Configuration;
using UserManagement.Application.Dtos;
using UserManagement.Application.Interfaces;
using UserManagement.Application.Resources;
using UserManagement.Domain.Constants;
using UserManagement.Domain.Entities;

namespace UserManagement.Application.Commands
{
    public class UserEditCommand : IRequest<UserDto>
    {
        public string UserId { get; set; }
        public string EndpointName { get; set; }

        public string Id { get; set; }
        public string? Username { get; set; }
        public string? Password { get; set; }
        public string? Email { get; set; }
        public string? FirstName { get; set; }
        public string? MiddleName { get; set; }
        public string? LastName { get; set; }
        public string? Address { get; set; }
        public string? PhoneNumber { get; set; }
        public string? MobileNumber { get; set; }
        public List<FileUploadDto>? Files { get; set; }
        public int? Status { get; set; }
        public List<string>? Roles { get; set; }
    }
    internal class UserEditCommandHandler : IRequestHandler<UserEditCommand, UserDto>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        private readonly IEmailService _emailService;
        private readonly IStringLocalizer<ErrorsResource> _errorLocalizer;
        private readonly IStringLocalizer<MessagesResource> _messageLocalizer;
        private readonly UserPreferences _userPreferences;

        public UserEditCommandHandler(
            IUnitOfWork unitOfWork,
            IStringLocalizer<ErrorsResource> errorLocalizer,
            IMapper mapper = null,
            IEmailService emailService = null
,
            IStringLocalizer<MessagesResource> messageLocalizer = null,
            UserPreferences userPreferences = null)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _emailService = emailService;
            _errorLocalizer = errorLocalizer;
            _messageLocalizer = messageLocalizer;
            _userPreferences = userPreferences;
        }

        public async Task<UserDto> Handle(UserEditCommand request, CancellationToken cancellationToken)
        {
            var userUpdate = await _unitOfWork.Users.GetById(request.UserId);
            var oldData = await _unitOfWork.Users.GetById(request.Id);
            if (oldData is null)
                throw new ApplicationException(_errorLocalizer["Error.Common.NotFound"]);
            User user = await _unitOfWork.Users.GetById(request.Id);
            if (!string.IsNullOrEmpty(request.Username))
            {
                var userOther = await _unitOfWork.Users.GetUser(request.Username);
                if (userOther is not null)
                    if (userOther.Id != user.Id)
                        throw new ApplicationException(_messageLocalizer["Error.User.UsernameRegistered"]);
            }
            if (!string.IsNullOrEmpty(request.Email))
            {
                var userOther = await _unitOfWork.Users.GetUser(request.Email);
                if (userOther is not null)
                    if (userOther.Id != user.Id)
                        throw new ApplicationException(_messageLocalizer["Error.User.EmailRegistered"]);
            }
            if (!string.IsNullOrEmpty(request.Username)) user.Username = request.Username;
            if (!string.IsNullOrEmpty(request.Password)) user.Password = request.Password;
            if (!string.IsNullOrEmpty(request.Email)) user.Email = request.Email;
            if (!string.IsNullOrEmpty(request.FirstName)) user.FirstName = request.FirstName;
            user.MiddleName = request.MiddleName;
            user.LastName = request.LastName;
            if (!string.IsNullOrEmpty(request.Address)) user.Address = request.Address;
            if (!string.IsNullOrEmpty(request.PhoneNumber)) user.PhoneNumber = request.PhoneNumber;
            if (!string.IsNullOrEmpty(request.MobileNumber)) user.MobileNumber = request.MobileNumber;
            Dictionary<string, IFormFile> fileToUploads = new();
            List<UserFile> files = new();
            if (request.Files is not null)
            {
                foreach (var file in request.Files)
                {
                    string fileName = $"{request.Id}_{DateTime.Now.ToString("yyyyMMddHHmmss")}{Path.GetExtension(file.FileUpload.FileName)}";
                    files.Add(new UserFile
                    {
                        Id = user.Id,
                        Type = file.Type,
                        Category = file.Category,
                        Title = !string.IsNullOrEmpty(file.Title) ? file.Title : "title",
                        Description = !string.IsNullOrEmpty(file.Description) ? file.Description : "deskripsi",
                        FileName = fileName
                    });
                    fileToUploads.Add(fileName, file.FileUpload);                    
                }
            }
            user.Files = files.Count == 0 ? null : files;
            if (request.Status is not null) user.Status = (int)request.Status;
            user.UpdatedDate = DateTime.Now;
            user.UpdatedBy = request.UserId;
            user.Roles = null;
            if (request.Roles is not null)
                user.Roles = request.Roles.Select(x => new Role
                {
                    Id = x
                });
            UserDto ret = new();
            _unitOfWork.Users.Update(
                request.UserId,
                request.EndpointName,
                user,
                result =>
                        {
                            if (result is not null)
                            {
                                foreach (var item in fileToUploads)
                                {
                                    using (Stream fileStream = new FileStream($"{DirectorySetting.PathFileUser}/{item.Key}", FileMode.Create, FileAccess.Write))
                                    {
                                        item.Value.CopyTo(fileStream);
                                    }
                                }
                                ret = _mapper.Map<UserDto>(result);
                                SendEmail(request.EndpointName, userUpdate.FullName, (DateTime)user.UpdatedDate, oldData, result);
                            }
                        });
            return ret;
        }
        private void SendEmail(string endpointName, string updatedBy, DateTime updatedDate, User oldData, User newData)
        {
            string strSubject = string.Empty;
            List<string> attactments = new();
            string strBody = System.IO.File.ReadAllText(AppDomain.CurrentDomain.BaseDirectory + $"/Resources/Templates/Email/HeaderEmail-{_userPreferences.LanguageId}.html");
            if (endpointName == "UserController/UserActivation")
            {
                strSubject = _messageLocalizer["UserActivation.Email.Subject"];
                strBody += System.IO.File.ReadAllText(AppDomain.CurrentDomain.BaseDirectory + $"/Resources/Templates/Email/UserActivation-{_userPreferences.LanguageId}.html");
            }
            else if (endpointName == "UserController/UserDeActivation")
            {
                strSubject = _messageLocalizer["UserDeActivation.Email.Subject"];
                strBody += System.IO.File.ReadAllText(AppDomain.CurrentDomain.BaseDirectory + $"/Resources/Templates/Email/UserDeActivation-{_userPreferences.LanguageId}.html");
            }
            else if (endpointName == "UserController/UserEdit")
            {
                strSubject = _messageLocalizer["UserEdit.Email.Subject"];
                strBody += System.IO.File.ReadAllText(AppDomain.CurrentDomain.BaseDirectory + $"/Resources/Templates/Email/UserEdit-{_userPreferences.LanguageId}.html");
                var fotoLama = oldData.Files.Where(x => x.Category == UserFileCategoryConstant.PhotoProfile).FirstOrDefault();
                if (File.Exists($"{DirectorySetting.PathFileUser}/{fotoLama?.FileName}"))
                    attactments.Add($"{DirectorySetting.PathFileUser}/{fotoLama?.FileName}");
                var fotoBaru = newData.Files.Where(x => x.Category == UserFileCategoryConstant.PhotoProfile).FirstOrDefault();
                if (File.Exists($"{DirectorySetting.PathFileUser}/{fotoBaru?.FileName}"))
                    attactments.Add($"{DirectorySetting.PathFileUser}/{fotoBaru?.FileName}");
            }
            else if (endpointName == "UserController/UserProfileEdit")
            {
                strSubject = _messageLocalizer["UserProfileEdit.Email.Subject"];
                strBody += System.IO.File.ReadAllText(AppDomain.CurrentDomain.BaseDirectory + $"/Resources/Templates/Email/UserProfileEdit-{_userPreferences.LanguageId}.html");
                var fotoLama = oldData.Files?.Where(x => x.Category == UserFileCategoryConstant.PhotoProfile).FirstOrDefault();
                if (File.Exists($"{DirectorySetting.PathFileData}/{fotoLama?.FileName}"))
                    attactments.Add($"{DirectorySetting.PathFileData}/{fotoLama?.FileName}");
                var fotoBaru = newData.Files?.Where(x => x.Category == UserFileCategoryConstant.PhotoProfile).FirstOrDefault();
                if (File.Exists($"{DirectorySetting.PathFileData}/{fotoBaru?.FileName}"))
                    attactments.Add($"{DirectorySetting.PathFileData}/{fotoBaru?.FileName}");
            }
            else
            {
                return;
            }

            strBody += System.IO.File.ReadAllText(AppDomain.CurrentDomain.BaseDirectory + $"/Resources/Templates/Email/FooterEmail-{_userPreferences.LanguageId}.html");
            strBody = strBody.Replace("[FULLNAME]", newData.FullName)
                            .Replace("[USERNAME_OLD]", oldData.Username)
                            .Replace("[USERNAME_NEW]", newData.Username)
                            .Replace("[EMAIL_OLD]", oldData.Email)
                            .Replace("[EMAIL_NEW]", newData.Email)
                            .Replace("[FIRSTNAME_OLD]", oldData.FirstName)
                            .Replace("[FIRSTNAME_NEW]", newData.FirstName)
                            .Replace("[MIDDLENAME_OLD]", oldData.MiddleName)
                            .Replace("[MIDDLENAME_NEW]", newData.MiddleName)
                            .Replace("[LASTNAME_OLD]", oldData.LastName)
                            .Replace("[LASTNAME_NEW]", newData.LastName)
                            .Replace("[ADDRESS_OLD]", oldData.Address)
                            .Replace("[ADDRESS_NEW]", newData.Address)
                            .Replace("[PHONENUMBER_OLD]", oldData.PhoneNumber)
                            .Replace("[PHONENUMBER_NEW]", newData.PhoneNumber)
                            .Replace("[MOBILENUMBER_OLD]", oldData.MobileNumber)
                            .Replace("[MOBILENUMBER_NEW]", newData.MobileNumber)
                            .Replace("[UPDATEDDATE]", updatedDate.ToLongStringId())
                            .Replace("[UPDATEDBY]", updatedBy)
                            ;

            EmailMessageDto message = new()
            {
                To = new() { newData.Email },
                Cc = new(),
                Subject = strSubject,
                Attachments = attactments
            };
            message.Content = strBody;
            _emailService.SendHtmlEmailAsync(message);
        }
    }
}