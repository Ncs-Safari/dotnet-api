using AutoMapper;
using MediatR;
using UserManagement.Application.Dtos;
using UserManagement.Application.Interfaces;

namespace UserManagement.Application.Queries
{
    public class GetUserQuery : IRequest<UserDto>
    {
        public string IdOrUsernameOrEmail { get; set; }
    }
    internal class GetUserQueryHandler : IRequestHandler<GetUserQuery, UserDto>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public GetUserQueryHandler(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task<UserDto> Handle(GetUserQuery request, CancellationToken cancellationToken)
        {
            if(string.IsNullOrEmpty(request.IdOrUsernameOrEmail)) return null;
            var user = await _unitOfWork.Users.GetUser(request.IdOrUsernameOrEmail);
            return user is null ? null : _mapper.Map<UserDto>(user, opt =>
                                                opt.AfterMap((src, dest) => dest.Password =  string.Empty));
        }
    }
}