using Microsoft.Extensions.DependencyInjection;
using SchoolBridge.Domain.Services.Abstraction;
using System;
using System.ComponentModel.DataAnnotations;

namespace SchoolBridge.API.Controllers.Attributes.Validation
{
    public class ArgValidAttribute : RequiredAttribute
    {
        public string[] FuncIdsAtributes { get; private set; }

        public ArgValidAttribute(params string[] funcIds)
            => FuncIdsAtributes = funcIds;

        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            Console.WriteLine("asdasda");
            validationContext.GetService<IValidatingService>().Validate(FuncIdsAtributes, value, validationContext.DisplayName);
            return ValidationResult.Success;
        }


    }
}
