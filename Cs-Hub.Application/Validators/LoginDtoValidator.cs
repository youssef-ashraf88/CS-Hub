using Cs_Hub.Application.DTO;
using FluentValidation;
using System;
using System.Collections.Generic;
using System.Text;

namespace Cs_Hub.Application.Validators
{
    public class LoginDtoValidator : AbstractValidator<LoginDTO>
    {
        public LoginDtoValidator()
        {
            RuleFor(x => x.Email)
                .NotEmpty()
                .WithMessage("Email can't be blank");

            RuleFor(x => x.Password)
                .NotEmpty()
                .WithMessage("Password can't be blank");
        }
    }
}
