using System.Globalization;
using AutoMapper;
using Common.Constants;
using Common.Dtos;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Localization;
using Swashbuckle.AspNetCore.Annotations;
using UserManagement.Application.Commands;
using UserManagement.Application.Dtos;
using UserManagement.Application.Resources;
using UserManagement.Domain.Constants;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace UserManagement.Adm.Api.Controllers
{
    [Route("api/[controller]")]
    public class AuthController : Controller
    {
        private readonly IMediator _mediator;
        private readonly IMapper _mapper;
        private readonly IStringLocalizer<MessagesResource> _messageLocalizer;

        public AuthController(
            IMediator mediator,
            IMapper mapper = null,
            IStringLocalizer<MessagesResource> messageLocalizer = null)
        {
            _mediator = mediator;
            _mapper = mapper;
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

        [HttpPost("Login", Name = "Login")]
        [SwaggerOperation(Summary = "User Authentication", Tags = new[] { "User" })]
        public async Task<ResponseDto<string>> Login(
            [SwaggerParameter("Username or Email", Required = true)] string UsernameOrEmail,
            [SwaggerParameter("Password", Required = true)] string Password)
        {
            UserLoginCommand command = new()
            {
                EndpointName = GetRouteName(),
                UsernameOrEmail = UsernameOrEmail,
                Password = Password
            };
            var validator = new UserLoginValidator();
            var validatorResult = validator.Validate(command);
            if (!validatorResult.IsValid)
                throw new ApplicationException(string.Join("; ", validatorResult.Errors.Select(error => error.ErrorMessage).ToList()));

            var result = await _mediator.Send(command);
            return new ResponseDto<string>
            {
                IsSuccess = !string.IsNullOrEmpty(result),
                Data = result
            };
        }
        [HttpPost("Register", Name = "Register")]
        [SwaggerOperation(Summary = "Register User", Tags = new[] { "Public" })]
        public async Task<ResponseDto<string>> Register(UserRegisterDto user)
        {
            UserAddCommand command = _mapper.Map<UserAddCommand>(user);
            command.IsRegister = true;
            command.Roles = [RoleConstant.Public];
            command.Files = new();
            var validator = new UserAddValidator();
            var validatorResult = validator.Validate(command);
            if (!validatorResult.IsValid)
                throw new ApplicationException(string.Join("; ", validatorResult.Errors.Select(error => error.ErrorMessage).ToList()));

            var result = await _mediator.Send(command);
            return new ResponseDto<string>
            {
                IsSuccess = result != null,
                Message = result != null ? $"{_messageLocalizer["Registration.Success.Message"]} {command.Email}" : _messageLocalizer["Registration.Failed.Message"]
            };
        }

    }
}

