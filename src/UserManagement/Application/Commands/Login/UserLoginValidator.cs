using FluentValidation;

namespace UserManagement.Application.Commands
{
    public class UserLoginValidator : AbstractValidator<UserLoginCommand>
    {
        public UserLoginValidator()
        {
            RuleFor(x => x.UsernameOrEmail).NotEmpty();
            RuleFor(x => x.Password).NotEmpty();
        }        
    }
}
