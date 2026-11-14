using Cs_Hub.Application.DTO.Profile;
using FluentValidation;
using System;
using System.Collections.Generic;
using System.Text;

namespace Cs_Hub.Application.Validators
{
    public class UpdateProfileDtoValidator : AbstractValidator<UpdateProfileDto>
    {
        public UpdateProfileDtoValidator()
        {
            RuleFor(x => x.DisplayName)
                .NotEmpty()
                .MinimumLength(2)
                .MaximumLength(50);

            RuleFor(x => x.HeadLine)
                .MaximumLength(100);

            RuleFor(x => x.Bio)
                .MaximumLength(500);

            RuleFor(x => x.Location)
                .MaximumLength(100);

            RuleFor(x => x.WebsiteUrl)
                .Must(BeValidWebsiteUrl)
                .When(x => !string.IsNullOrWhiteSpace(x.WebsiteUrl))
                .WithMessage("Website URL must be a valid HTTP or HTTPS URL.");
        }

        private static bool BeValidWebsiteUrl(string? url)
        {
            return Uri.TryCreate(url, UriKind.Absolute, out var uri) && (uri.Scheme == Uri.UriSchemeHttp || uri.Scheme == Uri.UriSchemeHttps);
        }
    }
}
