using AutoMapper;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;
using Microsoft.Extensions.Localization;
using Common.Exceptions;
using Common.Dtos;
using UserManagement.Application.Dtos;
using UserManagement.Application.Queries;
using UserManagement.Domain.Constants;
using UserManagement.Application.Commands;
using Common.Constants;
using UserManagement.Application.Resources;
using System.Globalization;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace UserManagement.Adm.Api.Controllers
{
    [Route("api/[controller]")]
    public class UserController : BaseApiController
    {
        private readonly IMediator _mediator;
        private readonly IMapper _mapper;
        private readonly IStringLocalizer<ErrorsResource> _errorLocalizer;
        private readonly IStringLocalizer<MessagesResource> _messageLocalizer;

        public UserController(
            IMediator mediator,
            IMapper mapper,
            IStringLocalizer<ErrorsResource> errorLocalizer = null,
            IStringLocalizer<MessagesResource> messageLocalizer = null)
        {
            _mediator = mediator;
            _mapper = mapper;
            this._errorLocalizer = errorLocalizer;
            _messageLocalizer = messageLocalizer;
        }

        private string GetRouteName()
        {
            var actionDescriptor = ControllerContext.ActionDescriptor;
            var httpPostAttribute = actionDescriptor.EndpointMetadata
                .OfType<HttpPostAttribute>()
                .FirstOrDefault();

            return $"{GetType().Name}/{httpPostAttribute?.Name}";
        }
        [HttpGet("GetUsers", Name = "GetUsers")]
        [SwaggerOperation(Summary = "Get all user data, complete with all attributes. Especially for administrators, the password column will be displayed", Tags = new[] { "User" })]
        public async Task<ResponseDto<IEnumerable<UserDto>>> GetUsers()
        {
            var query = new GetUsersQuery() { IsAdministrator = UserInfo().Roles.Where(x => x.Code == RoleConstant.Administrator).FirstOrDefault() is not null };
            var result = await _mediator.Send(query);
            return new ResponseDto<IEnumerable<UserDto>>
            {
                IsSuccess = result.Any(),
                Message = result.Any() ? "" : _errorLocalizer["Error.Common.NotFound"],
                Data = result
            };

        }
        [HttpGet("GetUser", Name = "GetUser")]
        [SwaggerOperation(Summary = "Get user data, parameter id or email or username", Tags = new[] { "User" })]
        public async Task<ResponseDto<UserDto>> GetUser(string idOrUsernameOrEmail)
        {
            var query = new GetUserQuery() { IdOrUsernameOrEmail = idOrUsernameOrEmail };
            var result = await _mediator.Send(query);
            return new ResponseDto<UserDto>
            {
                IsSuccess = result is not null,
                Message = result is not null ? "" : _errorLocalizer["Error.Common.NotFound"],
                Data = result
            };

        }
        [HttpPost("UserDelete", Name = "UserDelete")]
        [SwaggerOperation(Summary = "Delete user data based on id, only for users whose status is new", Tags = new[] { "User" })]
        public async Task<ResponseDto<string>> UserDelete(string id)
        {
            var command = new UserDeleteCommand
            {
                UserId = UserInfo().Id,
                EndpointName = GetRouteName(),
                Id = id,
            };
            var result = await _mediator.Send(command);
            return new ResponseDto<string>
            {
                IsSuccess = result is not null,
                Message = result,
                Data = null
            };
        }
        [HttpPost("UserAdd", Name = "UserAdd")]
        [SwaggerOperation(Summary = "Add user data, which can only be done by application administrators", Tags = new[] { "User" })]
        public async Task<ResponseDto<string>> UserAdd(UserAddDto data)
        {
            if (!UserInfo().Roles.Select(x => x.Code == RoleConstant.Administrator).Any())
                throw new ApplicationException(_errorLocalizer["Error.Common.Unauthorized"]);
            UserAddCommand command = _mapper.Map<UserAddCommand>(data);
            command.UserId = UserInfo().Id;
            var validator = new UserAddValidator();
            var validatorResult = validator.Validate(command);
            if (!validatorResult.IsValid)
                throw new ApplicationException(string.Join("; ", validatorResult.Errors.Select(error => error.ErrorMessage).ToList()));

            var result = await _mediator.Send(command);
            return new ResponseDto<string>
            {
                IsSuccess = result != null
            };
        }
        [HttpPost("UserEdit", Name = "UserEdit")]
        [SwaggerOperation(Summary = "Edit data User, only administrators can edit data user", Tags = new[] { "User" })]
        public async Task<ResponseDto<string>> UserEdit(UserEditDto data)
        {
            if (!UserInfo().Roles.Where(x => x.Code == RoleConstant.Administrator).Any())
            {
                throw new ApplicationException(_errorLocalizer["Error.Common.Unauthorized"]);
            }
            UserEditCommand command = _mapper.Map<UserEditCommand>(data);
            command.EndpointName = GetRouteName();
            command.UserId = UserInfo().Id;
            var validator = new UserEditValidator();
            var validatorResult = validator.Validate(command);
            if (!validatorResult.IsValid)
                throw new ApplicationException(string.Join("; ", validatorResult.Errors.Select(error => error.ErrorMessage).ToList()));

            var result = await _mediator.Send(command);
            return new ResponseDto<string>
            {
                IsSuccess = result != null
            };
        }
        [HttpPost("UserProfileEdit", Name = "UserProfileEdit")]
        [SwaggerOperation(Summary = "Edit data profile User, All users can make changes to their own profile data.", Tags = new[] { "User" })]
        public async Task<ResponseDto<string>> UserProfileEdit(UserProfileEditDto data)
        {
            if (data.Id != UserInfo().Id)
            {
                throw new ApplicationException(_errorLocalizer["Error.Common.Unauthorized"]);
            }
            UserEditCommand command = _mapper.Map<UserEditCommand>(data);
            command.EndpointName = GetRouteName();
            command.UserId = UserInfo().Id;
            var validator = new UserEditValidator();
            var validatorResult = validator.Validate(command);
            if (!validatorResult.IsValid)
                throw new ApplicationException(string.Join("; ", validatorResult.Errors.Select(error => error.ErrorMessage).ToList()));

            var result = await _mediator.Send(command);
            return new ResponseDto<string>
            {
                IsSuccess = result != null
            };
        }
        [HttpPost("UserActivation", Name = "UserActivation")]
        [SwaggerOperation(Summary = "User account activation", Tags = new[] { "User" })]
        public async Task<ResponseDto<string>> UserActivation(string id, string roleId)
        {
            UserEditCommand command = new()
            {
                UserId = UserInfo().Id,
                EndpointName = GetRouteName(),
                Id = id,
                Status = StatusDataConstant.Active,
                Roles = string.IsNullOrEmpty(roleId) ? null : [roleId]
            };
            var validator = new UserEditValidator();
            var validatorResult = validator.Validate(command);
            if (!validatorResult.IsValid)
                throw new ApplicationException(string.Join("; ", validatorResult.Errors.Select(error => error.ErrorMessage).ToList()));

            var result = await _mediator.Send(command);
            return new ResponseDto<string>
            {
                IsSuccess = result != null
            };
        }
        [HttpPost("UserDeActivation", Name = "UserDeActivation")]
        [SwaggerOperation(Summary = "User account activation", Tags = new[] { "User" })]
        public async Task<ResponseDto<string>> UserDeActivation(string id)
        {
            UserEditCommand command = new()
            {
                UserId = UserInfo().Id,
                EndpointName = GetRouteName(),
                Id = id,
                Status = StatusDataConstant.NotActive
            };
            var validator = new UserEditValidator();
            var validatorResult = validator.Validate(command);
            if (!validatorResult.IsValid)
                throw new ApplicationException(string.Join("; ", validatorResult.Errors.Select(error => error.ErrorMessage).ToList()));

            var result = await _mediator.Send(command);
            return new ResponseDto<string>
            {
                IsSuccess = result != null
            };
        }
        [HttpPost("ChangePassword", Name = "ChangePassword")]
        [SwaggerOperation(Summary = "Password changes can be made if the new password and password confirmation have the same value.", Tags = new[] { "User" })]
        public async Task<ResponseDto<string>> ChangePassword(string oldPassword, string newPassword, string confirmPassword)
        {
            var command = new ChangePasswordCommand
            {
                UserId = UserInfo().Id,
                EndpointName = GetRouteName(),
                Id = UserInfo().Id,
                OldPassword = oldPassword,
                NewPassword = newPassword,
                ConfirmPassword = confirmPassword
            };
            var validator = new ChangePasswordValidator(_messageLocalizer);
            var validatorResult = validator.Validate(command);
            if (!validatorResult.IsValid)
                throw new ApplicationException(string.Join("; ", validatorResult.Errors.Select(error => error.ErrorMessage).ToList()));

            var result = await _mediator.Send(command);
            return new ResponseDto<string>
            {
                IsSuccess = result is not null,
                Message = string.Empty,
                Data = null
            };
        }
        [HttpGet("GetMenus", Name = "GetMenus")]
        [SwaggerOperation(Summary = "Get All Menus By User Login", Tags = new[] { "User" })]
        public async Task<ResponseDto<IEnumerable<ApplMenuDto>>> GetMenus()
        {
            GetMenusQuery query = new()
            {
                ApplId = "adm",
                UserId = UserInfo().Id,
            };
            var result = await _mediator.Send(query);

            return new ResponseDto<IEnumerable<ApplMenuDto>>
            {
                IsSuccess = result.Any(),
                Message = result.Any() ? "" : _errorLocalizer["Error.Common.NotFound"],
                Data = result
            };
        }
    }
}

