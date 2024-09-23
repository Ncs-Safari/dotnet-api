using MediatR;
using Microsoft.Extensions.Localization;
using Common.Constants;
using Common.Exceptions;
using Common.Extensions;
using UserManagement.Application.Interfaces;
using UserManagement.Application.Resources;

namespace UserManagement.Application.Commands
{
    public class UserLoginCommand : IRequest<string>
    {
        public string EndpointName { get; set; }
        public string UsernameOrEmail { get; set; }
        public string Password { get; set; }

    }
    internal class UserLoginCommandHandler : IRequestHandler<UserLoginCommand, string>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IJwtTokenGenerator _jwtTokenGenerator;
        private readonly IStringLocalizer<ErrorsResource> _errorLocalizer;
        private readonly IStringLocalizer<MessagesResource> _messageLocalizer;

        public UserLoginCommandHandler(
            IUnitOfWork unitOfWork,
            IJwtTokenGenerator jwtTokenGenerator,
            IStringLocalizer<ErrorsResource> errorLocalizer,
            IStringLocalizer<MessagesResource> messageLocalizer)
        {
            _unitOfWork = unitOfWork;
            _jwtTokenGenerator = jwtTokenGenerator;
            _errorLocalizer = errorLocalizer;
            _messageLocalizer = messageLocalizer;
        }

        public async Task<string> Handle(UserLoginCommand request, CancellationToken cancellationToken)
        {

            var user = await _unitOfWork.Users.GetUser(request.UsernameOrEmail);
            if (user is null)
                throw new ApplicationException(_messageLocalizer["Error.User.EmailNotRegsitered"]);
            if (user.Status != StatusDataConstant.Active)
                throw new ApplicationException(_messageLocalizer["Error.User.NotActive"]);
            if (user.Password.DecryptString() != request.Password)
                throw new ApplicationException(_messageLocalizer["Error.User.InvalidPassword"]);
            user.LastLogin = DateTime.Now;
            user.Files = null;
            user.Roles = null;
            string token = string.Empty;
            _unitOfWork.Users.Update(
                user.Id,
                request.EndpointName,
                user,
                result =>
            {
                if (result is not null)
                {
                    token = _jwtTokenGenerator.GenerateToken(user);
                }
            });
            return token;
        }
    }
}