using FluentValidation;
using Microsoft.Extensions.Localization;
using UserManagement.Application.Resources;

namespace UserManagement.Application.Commands
{
    public class ChangePasswordValidator : AbstractValidator<ChangePasswordCommand>
    {
        private readonly IStringLocalizer<MessagesResource> _messageLocalizer;
        public ChangePasswordValidator(IStringLocalizer<MessagesResource> messageLocalizer = null)
        {
            _messageLocalizer = messageLocalizer;
            RuleFor(x => x.OldPassword).NotEmpty();
            RuleFor(x => x.NewPassword).NotEmpty();
            RuleFor(x => x.ConfirmPassword).NotEmpty();
            RuleFor(x => x).Must(BeSameNewPassword).WithMessage(_messageLocalizer["Error.User.InvalidPasswordComfirm"]);
        }
        private static bool BeSameNewPassword(ChangePasswordCommand value)
        {
            return value.NewPassword == value.ConfirmPassword;
        }
    }
}