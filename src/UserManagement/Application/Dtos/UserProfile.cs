using AutoMapper;
using Common.Constants;
using UserManagement.Application.Configuration;
using UserManagement.Application.Dtos;
using UserManagement.Domain.Constants;
using UserManagement.Domain.Entities;
using Common.Extensions;
using Common.Dtos;
using UserManagement.Application.Commands;
namespace UserManagement.Application.Dtos
{
    public class UserProfile : Profile
    {
        public UserProfile()
        {
            /*
            Mapping from Domain Entities to DTO
            */
            CreateMap<Appl, ApplDto>()
                .ForMember(dest => dest.Iconfile, opt => opt.MapFrom(src => string.IsNullOrEmpty(src.Iconfile) ? "" : $"{DirectorySetting.UrlFileApp}/{src.Iconfile}"))
                .ForMember(dest => dest.Imagefile, opt => opt.MapFrom(src => string.IsNullOrEmpty(src.Imagefile) ? "" : $"{DirectorySetting.UrlFileApp}/{src.Imagefile}"))
                .ForMember(dest => dest.IconfileBase64, opt => opt.MapFrom(src => string.IsNullOrEmpty(src.Iconfile) ? "" : $"{DirectorySetting.PathFileApp}/{src.Iconfile}".ToBase64()))
                .ForMember(dest => dest.ImagefileBase64, opt => opt.MapFrom(src => string.IsNullOrEmpty(src.Imagefile) ? "" : $"{DirectorySetting.PathFileApp}/{src.Imagefile}".ToBase64()))
            ;
            CreateMap<ApplExtra, ApplExtraDto>();
            CreateMap<ApplGallery, ApplGalleryDto>()
                .ForMember(dest => dest.FileGallery, opt => opt.MapFrom(src => string.IsNullOrEmpty(src.FileGallery) ? "" : $"{DirectorySetting.UrlFileApp}/{src.FileGallery}"))
                .ForMember(dest => dest.FileThumbnail, opt => opt.MapFrom(src => string.IsNullOrEmpty(src.FileThumbnail) ? "" : $"{DirectorySetting.UrlFileApp}/{src.FileThumbnail}"))
                .ForMember(dest => dest.FileGalleryBase64, opt => opt.MapFrom(src => string.IsNullOrEmpty(src.FileGallery) ? "" : $"{DirectorySetting.PathFileApp}/{src.FileGallery}".ToBase64()))
                .ForMember(dest => dest.FileThumbnailBase64, opt => opt.MapFrom(src => string.IsNullOrEmpty(src.FileThumbnail) ? "" : $"{DirectorySetting.PathFileApp}/{src.FileThumbnail}".ToBase64()))
                .ForMember(dest => dest.CreatedDate, opt => opt.MapFrom(src => src.CreatedDate.ToStringId("")))
            ;
            CreateMap<ApplInfographic, ApplInfographicDto>()
                .ForMember(dest => dest.FileInfographic, opt => opt.MapFrom(src => string.IsNullOrEmpty(src.FileInfographic) ? "" : $"{DirectorySetting.UrlFileApp}/{src.FileInfographic}"))
                .ForMember(dest => dest.FileThumbnail, opt => opt.MapFrom(src => string.IsNullOrEmpty(src.FileThumbnail) ? "" : $"{DirectorySetting.UrlFileApp}/{src.FileThumbnail}"))
                .ForMember(dest => dest.FileInfographicBase64, opt => opt.MapFrom(src => string.IsNullOrEmpty(src.FileInfographic) ? "" : $"{DirectorySetting.PathFileApp}/{src.FileInfographic}".ToBase64()))
                .ForMember(dest => dest.FileThumbnailBase64, opt => opt.MapFrom(src => string.IsNullOrEmpty(src.FileThumbnail) ? "" : $"{DirectorySetting.PathFileApp}/{src.FileThumbnail}".ToBase64()))
                .ForMember(dest => dest.CreatedDate, opt => opt.MapFrom(src => src.CreatedDate.ToStringId("")))
            ;
            CreateMap<ApplNewsCategory, ApplNewsCategoryDto>()
                .ForMember(dest => dest.FileLogo, opt => opt.MapFrom(src => string.IsNullOrEmpty(src.FileLogo) ? "" : $"{DirectorySetting.UrlFileApp}/{src.FileLogo}"))
                .ForMember(dest => dest.FileLogoBase64, opt => opt.MapFrom(src => string.IsNullOrEmpty(src.FileLogo) ? "" : $"{DirectorySetting.UrlFileApp}/{src.FileLogo}".ToBase64()))
            ;
            CreateMap<ApplNews, ApplNewsDto>()
                .ForMember(dest => dest.FileNews, opt => opt.MapFrom(src => string.IsNullOrEmpty(src.FileNews) ? "" : $"{DirectorySetting.UrlFileApp}/{src.FileNews}"))
                .ForMember(dest => dest.FileThumbnail, opt => opt.MapFrom(src => string.IsNullOrEmpty(src.FileThumbnail) ? "" : $"{DirectorySetting.UrlFileApp}/{src.FileThumbnail}"))
                .ForMember(dest => dest.FileNewsBase64, opt => opt.MapFrom(src => string.IsNullOrEmpty(src.FileNews) ? "" : $"{DirectorySetting.PathFileApp}/{src.FileNews}".ToBase64()))
                .ForMember(dest => dest.FileThumbnailBase64, opt => opt.MapFrom(src => string.IsNullOrEmpty(src.FileThumbnail) ? "" : $"{DirectorySetting.PathFileApp}/{src.FileThumbnail}".ToBase64()))
                .ForMember(dest => dest.CreatedDate, opt => opt.MapFrom(src => src.CreatedDate.ToStringId("")))
            ;
            CreateMap<ApplTaskDelegation, ApplTaskDelegationDto>()
                .ForMember(dest => dest.StartDate, opt => opt.MapFrom(src => src.StartDate.ToStringId("")))
                .ForMember(dest => dest.EndDate, opt => opt.MapFrom(src => src.EndDate.ToStringId("")))
                .ForMember(dest => dest.CreatedDate, opt => opt.MapFrom(src => src.CreatedDate.ToStringId("")))
                .ForMember(dest => dest.UpdatedDate, opt => opt.MapFrom(src => src.UpdatedDate == null ? string.Empty : ((DateTime)src.UpdatedDate).ToStringId("")))
            ;
            CreateMap<ApplTask, ApplTaskDto>();
            CreateMap<RoleApplTask, RoleApplTaskDto>();
            CreateMap<Role, RoleDto>();
            CreateMap<User, UserDto>()
                .ForMember(dest => dest.LastLogin, opt => opt.MapFrom(src => src.LastLogin == null ? string.Empty : ((DateTime)src.LastLogin).ToStringId("")))
                .ForMember(dest => dest.CreatedDate, opt => opt.MapFrom(src => src.CreatedDate.ToStringId("")))
                .ForMember(dest => dest.UpdatedDate, opt => opt.MapFrom(src => src.UpdatedDate == null ? string.Empty : ((DateTime)src.UpdatedDate).ToStringId("")))
                .ForMember<IEnumerable<RoleDto>>(dest => dest.Roles, opt => opt.MapFrom<IEnumerable<Role>>(src => src.Roles ?? null))
                .ForMember<IEnumerable<UserFileDto>>(dest => dest.Files, opt => opt.MapFrom<IEnumerable<UserFile>>(src => src.Files ?? null))

            ;
            CreateMap<UserFile, UserFileDto>()
                .ForMember(dest => dest.Type, opt => opt.MapFrom(src => string.IsNullOrEmpty(src.Type) ? "" : $"{FileTypeConstant.Dict[src.Type]}"))
                .ForMember(dest => dest.Category, opt => opt.MapFrom(src => string.IsNullOrEmpty(src.Category) ? "" : $"{UserFileCategoryConstant.Dict[src.Category]}"))
                .ForMember(dest => dest.FileName, opt => opt.MapFrom(src => string.IsNullOrEmpty(src.FileName) ? "" : $"{DirectorySetting.UrlFileUser}/{src.FileName}"))
                .ForMember(dest => dest.FileNameBase64, opt => opt.MapFrom(src => string.IsNullOrEmpty(src.FileName) ? "" : $"{DirectorySetting.UrlFileUser}/{src.FileName}".ToBase64()))
                .ForMember(dest => dest.FileThumbnail, opt => opt.MapFrom(src => string.IsNullOrEmpty(src.FileThumbnail) ? "" : $"{DirectorySetting.UrlFileUser}/{src.FileThumbnail}"))
                .ForMember(dest => dest.FileThumbnailBase64, opt => opt.MapFrom(src => string.IsNullOrEmpty(src.FileThumbnail) ? "" : $"{DirectorySetting.UrlFileUser}/{src.FileThumbnail}".ToBase64()))
            ;
            CreateMap<UserRole, UserRoleDto>();

            /*
            Mapping from DTO to Command
            */
            CreateMap<UserRegisterDto, UserAddCommand>();
            CreateMap<UserAddDto, UserAddCommand>();
            CreateMap<UserEditDto, UserEditCommand>();
            CreateMap<UserProfileEditDto, UserEditCommand>();


            /*
            Mapping from Command to Entities
            */
            CreateMap<UserAddCommand, User>()
                .ForMember(dest => dest.Id, opt => opt.MapFrom(src => Ulid.NewUlid().ToString()))
                .ForMember(dest => dest.CreatedDate, opt => opt.MapFrom(src => DateTime.Now))
                .ForMember(dest => dest.Files, opt => opt.Ignore())
                .ForMember(dest => dest.Roles, opt => opt.Ignore())
                ;
        }
    }
}
