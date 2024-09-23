using System.Security.Cryptography.Xml;
using Common.Constants;
using FluentValidation;
using Microsoft.AspNetCore.Http;
using UserManagement.Application.Commands;
using UserManagement.Domain.Constants;

namespace UserManagement.Application.Commands
{
    public class UserAddValidator : AbstractValidator<UserAddCommand>
    {
        public UserAddValidator()
        {
            RuleFor(x => x.Username).NotEmpty();
            //            RuleFor(x => x.Password).NotEmpty();
            RuleFor(x => x.Email).NotEmpty();
            RuleFor(x => x.FirstName).NotEmpty();
            RuleFor(x => x.Address).NotEmpty();
            When(x => x.Files is not null || x.Files.Count() > 0, () =>
            {
                RuleForEach(x => x.Files.Select(x => x.Type)).NotEmpty().OverridePropertyName("Type").Must(BeAValidType);
                When(y => y.Files.Select(z => z.Category).FirstOrDefault() is not null, () =>
                    {
                        RuleForEach(x => x.Files.Where(xx => xx.Category != null).Select(y => y.Category)).NotEmpty().OverridePropertyName("Category").Must(BeAValidCategory);
                    }
                );
                RuleForEach(x => x.Files.Where(x => x.Type == FileTypeConstant.Image).Select(x => x.FileUpload)).NotEmpty().OverridePropertyName("FileUpload").Must(BeAImageFile);
            });
        }
        private static bool BeAImageFile(IFormFile file)
        {
            if (file is null) return false;
            string[] imageContenTypes = ["image/jpeg", "image/jpg", "image/png", "image/bmp", "image/x-ms-bmp"];
            string fileExtension = Path.GetExtension(file.FileName).ToLower();
            return imageContenTypes.Contains(file.ContentType);
        }
        private static bool BeAValidType(string type)
        {
            return FileTypeConstant.Dict.ContainsKey(type);
        }
        private static bool BeAValidCategory(string? category)
        {
            if (string.IsNullOrEmpty(category)) return false;
            return UserFileCategoryConstant.Dict.ContainsKey(category);
        }
    }
}