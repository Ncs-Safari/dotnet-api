using Common.Dtos;
using Common.Exceptions;
using Common.Interfaces;
using Common.ValueObjects;
using MediatR;
using Microsoft.Extensions.Localization;
using UserManagement.Application.Configuration;
using UserManagement.Application.Interfaces;
using UserManagement.Application.Resources;

namespace UserManagement.Application.Commands
{
    public class UserDeleteCommand : IRequest<string>
    {
        public string UserId { get; set; }
        public string EndpointName { get; set; }

        public string Id { get; set; }

    }
    internal class UserDeleteCommandHandler : IRequestHandler<UserDeleteCommand, string>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IStringLocalizer<ErrorsResource> _errorLocalizer;
        private readonly UserPreferences _userPreferences;
        private readonly IEmailService _emailService;
        private readonly IStringLocalizer<MessagesResource> _messageLocalizer;

        public UserDeleteCommandHandler(
            IUnitOfWork unitOfWork,
            IStringLocalizer<ErrorsResource> errorLocalizer,
            UserPreferences userPreferences,
            IStringLocalizer<MessagesResource> messageLocalizer,
            IEmailService emailService)
        {
            _unitOfWork = unitOfWork;
            _errorLocalizer = errorLocalizer;
            _userPreferences = userPreferences;
            _messageLocalizer = messageLocalizer;
            _emailService = emailService;
        }

        public async Task<string> Handle(UserDeleteCommand request, CancellationToken cancellationToken)
        {
            var user = await _unitOfWork.Users.GetById(request.Id);
            if (user is null)
                throw new ApplicationException(_errorLocalizer["Error.Common.NotFound"]);

            _unitOfWork.Users.Delete(
                request.UserId, 
                request.EndpointName, 
                request.Id, 
                result =>
            {
                if (result is not null)
                {
                    if (user.Files is not null)
                        foreach (var userFile in user.Files)
                        {
                            string file = $"{DirectorySetting.PathFileUser}/{userFile.FileName}";
                            if (File.Exists(file))
                                File.Delete(file);
                            string fileThumbnail = $"{DirectorySetting.PathFileUser}/{userFile.FileThumbnail}";
                            if (File.Exists(fileThumbnail))
                                File.Delete(fileThumbnail);
                        }
                        string strBody = System.IO.File.ReadAllText(AppDomain.CurrentDomain.BaseDirectory + $"/Resources/Templates/Email/HeaderEmail-{_userPreferences.LanguageId}.html");
                        strBody += System.IO.File.ReadAllText(AppDomain.CurrentDomain.BaseDirectory + $"/Resources/Templates/Email/UserDelete-{_userPreferences.LanguageId}.html");
                        strBody += System.IO.File.ReadAllText(AppDomain.CurrentDomain.BaseDirectory + $"/Resources/Templates/Email/FooterEmail-{_userPreferences.LanguageId}.html");

                        strBody = strBody.Replace("[FULLNAME]", user.FullName);
                        strBody = strBody.Replace("[EMAIL]", user.Email);
                        strBody = strBody.Replace("[USERNAME]", user.Username);
                        
                        EmailMessageDto message = new()
                        {
                            To = new() { user.Email },
                            Cc = new(),
                            Subject = _messageLocalizer["UserDelete.Email.Subject"],
                            Attachments = new List<string> ()
                        };
                        message.Content = strBody;
                        _emailService.SendHtmlEmailAsync(message);                        
                }
            });
            return string.Empty;
        }
    }
}