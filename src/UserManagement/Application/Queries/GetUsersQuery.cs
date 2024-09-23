using AutoMapper;
using MediatR;
using UserManagement.Application.Dtos;
using UserManagement.Application.Interfaces;
using Common.Extensions;
namespace UserManagement.Application.Queries
{
    public class GetUsersQuery : IRequest<IEnumerable<UserDto>>
    {
        public bool IsAdministrator { get; set; }
    }
    internal class GetUsersQueryHandler : IRequestHandler<GetUsersQuery, IEnumerable<UserDto>>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public GetUsersQueryHandler(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task<IEnumerable<UserDto>> Handle(GetUsersQuery request, CancellationToken cancellationToken)
        {
            var users = await _unitOfWork.Users.GetAll();
            return users?.Select(user => _mapper.Map<UserDto>(user, opt =>
                                                opt.AfterMap((src, dest) => dest.Password = request.IsAdministrator ? user.Password.DecryptString() : string.Empty)));
        }
    }
}