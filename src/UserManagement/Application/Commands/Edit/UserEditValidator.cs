using FluentValidation;

namespace UserManagement.Application.Commands
{
    public class UserEditValidator : AbstractValidator<UserEditCommand>
    {
        public UserEditValidator()
        {
            //RuleFor(x => x.Username).NotEmpty();
            //RuleFor(x => x.Password).NotEmpty();
            //RuleFor(x => x.Email).NotEmpty();
            //RuleFor(x => x.FirstName).NotEmpty();
            //RuleFor(x => x.Address).NotEmpty();
        }
    }
}