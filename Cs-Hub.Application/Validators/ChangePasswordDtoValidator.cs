using Cs_Hub.Application.DTO.Profile;
using FluentValidation;
using System;
using System.Collections.Generic;
using System.Text;

namespace Cs_Hub.Application.Validators
{
    public class ChangePasswordDtoValidator : AbstractValidator<ChangePasswordDto>
    {
        public ChangePasswordDtoValidator()
        {
            RuleFor(x => x.CurrentPassword).NotEmpty().WithMessage("Current Password can't be blank");

            RuleFor(x => x.Password).NotEmpty().WithMessage("Password can't be blank");

            RuleFor(x => x.ConfirmPassword)
                .NotEmpty().WithMessage("Confirm Password can't be blank")
                .Equal(x => x.Password).WithMessage("Passwords do not match");
        }
    }
}
