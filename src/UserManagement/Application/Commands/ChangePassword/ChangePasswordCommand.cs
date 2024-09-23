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
using UserManagement.Domain.Entities;

namespace UserManagement.Application.Commands
{
    public class ChangePasswordCommand : IRequest<UserDto>
    {
        public string UserId { get; set; }
        public string EndpointName { get; set; }

        public string Id { get; set; }
        public string? OldPassword { get; set; }
        public string? NewPassword { get; set; }
        public string? ConfirmPassword { get; set; }
    }
    internal class ChangePasswordCommandHandler : IRequestHandler<ChangePasswordCommand, UserDto>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        private readonly IEmailService _emailService;
        private readonly IStringLocalizer<ErrorsResource> _errorLocalizer;
        private readonly IStringLocalizer<MessagesResource> _messageLocalizer;
        private readonly UserPreferences _userPreferences;

        public ChangePasswordCommandHandler(
            IUnitOfWork unitOfWork,
            IStringLocalizer<ErrorsResource> errorLocalizer,
            IMapper mapper = null,
            IEmailService emailService = null,
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

        public async Task<UserDto> Handle(ChangePasswordCommand request, CancellationToken cancellationToken)
        {
            var user = await _unitOfWork.Users.GetById(request.Id);
            if (user is null)
                throw new ApplicationException(_errorLocalizer["Error.Common.NotFound"]);
            if (request.OldPassword != user.Password.DecryptString())
                throw new ApplicationException(_messageLocalizer["Error.User.InvalidPassword"]);
            user.Password = request.NewPassword.EncryptString();
            user.UpdatedDate = DateTime.Now;
            user.UpdatedBy = request.UserId;

            UserDto ret = new();
            _unitOfWork.Users.Update(
                request.UserId,
                request.EndpointName,
                user,
                result =>
                {
                    if (result is not null)
                    {
                        ret = _mapper.Map<UserDto>(result);

                        string strBody = System.IO.File.ReadAllText(AppDomain.CurrentDomain.BaseDirectory + $"/Resources/Templates/Email/HeaderEmail-{_userPreferences.LanguageId}.html");
                        strBody += System.IO.File.ReadAllText(AppDomain.CurrentDomain.BaseDirectory + $"/Resources/Templates/Email/ChangePassword-{_userPreferences.LanguageId}.html");
                        strBody += System.IO.File.ReadAllText(AppDomain.CurrentDomain.BaseDirectory + $"/Resources/Templates/Email/FooterEmail-{_userPreferences.LanguageId}.html");

                        strBody = strBody.Replace("[UserName]", result.FullName);
                        strBody = strBody.Replace("[UserEmail]", result.Email);
                        strBody = strBody.Replace("[Date]", ((DateTime)result.UpdatedDate).ToLongStringId());
                        
                        EmailMessageDto message = new()
                        {
                            To = new() { user.Email },
                            Cc = new(),
                            Subject = _messageLocalizer["ChangePassword.Email.Subject"],
                            Attachments = new List<string> ()
                        };
                        message.Content = strBody;
                        _emailService.SendHtmlEmailAsync(message);
                    }
                });
            return ret;
        }
    }
}