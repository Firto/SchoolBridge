using Microsoft.Extensions.DependencyInjection;
using SchoolBridge.Domain.Services.Abstraction;
using System.ComponentModel.DataAnnotations;

namespace SchoolBridge.API.Controllers.Attributes.Validation
{
    public class ArgValidAttribute : ValidationAttribute
    {
        public string[] FuncIdsAtributes { get; private set; }

        public ArgValidAttribute(params string[] funcIds)
            => FuncIdsAtributes = funcIds;

        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            validationContext.GetService<IValidatingService>().Validate(FuncIdsAtributes, value, validationContext.DisplayName);
            return ValidationResult.Success;
        }


    }
}
