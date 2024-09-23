using AutoMapper;
using Common.Dtos;
using Common.ValueObjects;
using MediatR;
using UserManagement.Application.Dtos;
using UserManagement.Application.Interfaces;
using UserManagement.Domain.Entities;

namespace UserManagement.Application.Queries
{
    public class GetMenusQuery : IRequest<IEnumerable<ApplMenuDto>>
    {
        public string ApplId { get; set; }
        public string UserId { get; set; }
    }
    internal class GetMenusQueryHandler : IRequestHandler<GetMenusQuery, IEnumerable<ApplMenuDto>>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        private readonly UserPreferences _userPreferences;

        public GetMenusQueryHandler(IUnitOfWork unitOfWork, IMapper mapper, UserPreferences userPreferences)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _userPreferences = userPreferences;
        }
        private static List<ApplMenuDto> GetMenusByIdParent(IEnumerable<ApplTask> Data, string applTaskParentId)
        {
            List<ApplMenuDto> ret = new List<ApplMenuDto>();
            foreach (var item in Data.Where(x => x.ApplTaskParentId == applTaskParentId))
            {
                ApplMenuDto m = new()
                {
                    number = item.IndexNo.ToString(),
                    name = item.TaskName,
                    icon = item.IconName,
                    to = item.ActionName,
                    customid = item.CustomId,
                    tag = "CSidebarNavItems",
                    route = ""
                };

                var cek = from a in Data
                          where a.ApplTaskParentId == item.ApplTaskParentId
                          select a;
                if (cek != null)
                {
                    var recursiveChild = GetMenusByIdParent(Data, item.Id);
                    m.tag = (GetMenusByIdParent(Data, item.Id).Count > 0) ? "CSidebarNavDropdown" : "CSidebarNavItem";
                    m.childrens = (GetMenusByIdParent(Data, item.Id).Count > 0) ? recursiveChild : null;
                }
                ret.Add(m);
            }
            return ret;
        }
        private static List<ApplMenuDto> SetMenus(IEnumerable<ApplTask> data)
        {
            List<ApplMenuDto> ret = new();
            foreach (var item in data.ToList().Where(x => x.ApplTaskParentId is null))
            {
                ApplMenuDto m = new()
                {
                    number = item.IndexNo.ToString(),
                    name = item.TaskName,
                    icon = item.IconName,
                    to = item.ActionName,
                    customid = item.CustomId,
                    tag = "CSidebarNavItems",
                    route = ""
                };
                m.childrens = GetMenusByIdParent(data, item.Id);

                ret.Add(m);
            }
            return ret;
        }
        public async Task<IEnumerable<ApplMenuDto>> Handle(GetMenusQuery request, CancellationToken cancellationToken)
        {
            var applTasks = await _unitOfWork.Users.GetMenus(_userPreferences.LanguageId, request.ApplId, request.UserId);
            if (applTasks is null)
                return await Task.FromResult<IEnumerable<ApplMenuDto>>(null);
            var menus = SetMenus(applTasks);
            return await Task.FromResult(menus);
        }
    }
}