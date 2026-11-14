using Cs_Hub.Application.DTO.Profile;
using FluentValidation;
using System;
using System.Collections.Generic;
using System.Text;

namespace Cs_Hub.Application.Validators
{
    public class DeactivateAccountDtoValidator : AbstractValidator<DeactivateAccountDto>
    {
        public DeactivateAccountDtoValidator()
        {
            RuleFor(x => x.Password).NotEmpty().WithMessage("Password can't be blank");
        }
    }
}
