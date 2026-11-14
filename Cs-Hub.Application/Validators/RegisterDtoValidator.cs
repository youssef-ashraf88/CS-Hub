using Cs_Hub.Application.DTO;
using FluentValidation;
using System;
using System.Collections.Generic;
using System.Text;

namespace Cs_Hub.Application.Validators
{
    public class RegisterDtoValidator : AbstractValidator<RegisterDTO>
    {
        public RegisterDtoValidator()
        {
            RuleFor(x => x.Email)
                .NotEmpty()
                .WithMessage("Email is required.")
                .EmailAddress()
                .WithMessage("Invalid email format.");

            RuleFor(x => x.Password)
                .NotEmpty()
                .WithMessage("Password is required.");

            RuleFor(x => x.ConfirmPassword)
                .NotEmpty()
                .WithMessage("Confirm password is required.")
                .Equal(x => x.Password)
                .WithMessage("Passwords do not match.");
            
            RuleFor(x => x.DisplayName)
                .NotEmpty()
                .WithMessage("Display name is required.");
        }
    }
}
