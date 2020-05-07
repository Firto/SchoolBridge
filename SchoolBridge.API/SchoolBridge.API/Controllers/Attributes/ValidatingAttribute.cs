using SchoolBridge.Domain.Services.Abstraction;
using Microsoft.Extensions.DependencyInjection;
using System.ComponentModel.DataAnnotations;

namespace SchoolBridge.API.Controllers.Attributes
{
    public class MyValidationAttribute : ValidationAttribute
    {
        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            validationContext.GetService<IValidatingService>().Validate(validationContext.ObjectType, value);
            return ValidationResult.Success;
        }
    }
}
