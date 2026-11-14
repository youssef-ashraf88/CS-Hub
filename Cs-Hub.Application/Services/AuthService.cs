using Cs_Hub.Application.DTO;
using Cs_Hub.Application.DTO.Profile;
using Cs_Hub.Application.Helpers;
using Cs_Hub.Application.ServiceContracts;
using Cs_Hub.Application.Settings;
using Cs_Hub.Domain.entities.Users;
using Cs_Hub.Domain.Enums;
using Cs_Hub.Domain.RepositoryContracts;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.WebUtilities;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Text;

namespace Cs_Hub.Application.Services
{
    public class AuthService : IAuthService
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly IUserProfileRepository _userProfileRepository;
        private readonly ITransaction _transaction;
        private readonly IEmailService _emailService;
        private readonly EmailConfirmationSettings _emailConfirmationSettings;
        private readonly IJwtService _jwtService;

        public AuthService(UserManager<ApplicationUser> userManager, IUserProfileRepository userProfileRepository, ITransaction transaction, IEmailService emailService, IOptions<EmailConfirmationSettings> emailConfirmationSettings, IJwtService jwtService)
        {
            _userManager = userManager;
            _userProfileRepository = userProfileRepository;
            _transaction = transaction;
            _emailService = emailService;
            _emailConfirmationSettings = emailConfirmationSettings.Value;
            _jwtService = jwtService;
        }

        public async Task<Result<RegisterResponseDTO>> Register(RegisterDTO registerDto)
        {
            registerDto.Email = registerDto.Email.Trim();
            registerDto.DisplayName = registerDto.DisplayName.Trim();

            var user = new ApplicationUser
            {
                Email = registerDto.Email,
                UserName = registerDto.Email,
                AccountStatus = AccountStatus.Active,
                CreatedAt = DateTime.UtcNow
            };

            try
            {
                await _transaction.BeginTransaction();
                var result = await _userManager.CreateAsync(user, registerDto.Password);
                if (!result.Succeeded)
                {
                    await _transaction.RollbackTransaction();
                    var errors = string.Join(", ", result.Errors.Select(e => e.Description));

                    return Result<RegisterResponseDTO>.Fail(errors);
                }

                var role = await _userManager.AddToRoleAsync(user, "User");
                if (!role.Succeeded)
                {
                    await _transaction.RollbackTransaction();
                    var errors = string.Join(", ", role.Errors.Select(e => e.Description));

                    return Result<RegisterResponseDTO>.Fail(errors);
                }

                var userProfile = new UserProfile
                {
                    UserId = user.Id,
                    DisplayName = registerDto.DisplayName,
                    CreatedAt = DateTime.UtcNow
                };
                await _userProfileRepository.AddAsync(userProfile);
                await _transaction.CommitTransaction();
            }
            catch
            {
                await _transaction.RollbackTransaction();
                throw;
            }

            bool emailSent = false;

            try
            {
                var token = await _userManager.GenerateEmailConfirmationTokenAsync(user);
                var encodedToken = WebEncoders.Base64UrlEncode(Encoding.UTF8.GetBytes(token));
                var confirmationLink = $"{_emailConfirmationSettings.BaseUrl}/api/auth/confirm-email?userId={user.Id}&token={encodedToken}";

                var emailBody = $@"
                    <h2>Welcome to CS Hub!</h2>
                   <p>Thank you for registering. Please confirm your email by clicking the link below:</p>
                   <a href='{confirmationLink}' target='_blank'>Confirm Email</a>
                ";

                await _emailService.SendEmailAsync(user.Email, "Confirm Your CS Hub Email", emailBody);
                emailSent = true;
            }
            catch
            {
                emailSent = false;
            }
            return Result<RegisterResponseDTO>.Success(new RegisterResponseDTO
            {
                Message = emailSent ? "User registered successfully. Confirmation email sent" : "User registered successfully, but confirmation email could not be sent",
                EmailSent = emailSent
            });
        }

        public async Task<Result<string>> ConfirmEmail(string userId, string token)
        {
            var user = await _userManager.FindByIdAsync(userId);
            if (user == null)
                return Result<string>.Fail("User not found.");

            var decodedToken = Encoding.UTF8.GetString(WebEncoders.Base64UrlDecode(token));
            var result = await _userManager.ConfirmEmailAsync(user, decodedToken);

            if (!result.Succeeded)
            {
                var errors = string.Join(", ", result.Errors.Select(e => e.Description));
                return Result<string>.Fail(errors);
            }
            return Result<string>.Success("Email confirmed successfully");
        }

        public async Task<Result<LoginResponseDTO>> Login(LoginDTO loginDto)
        {
            var user = await _userManager.FindByEmailAsync(loginDto.Email);
            if (user == null)
                return Result<LoginResponseDTO>.Fail("Email or password is incorrect.");

            if (user.AccountStatus != AccountStatus.Active)
                return Result<LoginResponseDTO>.Fail("Your account is not active");

            if (await _userManager.IsLockedOutAsync(user))
                return Result<LoginResponseDTO>.Fail("Your account is temporarily locked. Try again later.");

            var passwordCheck = await _userManager.CheckPasswordAsync(user, loginDto.Password);
            if (!passwordCheck)
            {
                await _userManager.AccessFailedAsync(user);
                return Result<LoginResponseDTO>.Fail("Email or password is incorrect.");
            }
            await _userManager.ResetAccessFailedCountAsync(user);

            var roles = (await _userManager.GetRolesAsync(user));

            var loginResponse = _jwtService.CreateJwtToken(user, roles);

            return Result<LoginResponseDTO>.Success(loginResponse);
        }

        public async Task<Result<string>> ChangePassword(Guid userId, ChangePasswordDto dto)
        {
            var user = await _userManager.FindByIdAsync(userId.ToString());
            if (user == null)
                return Result<string>.Fail("User not found.");

            var result = await _userManager.ChangePasswordAsync(user, dto.CurrentPassword, dto.Password);
            if (!result.Succeeded)
            {
                var errors = string.Join(", ", result.Errors);
                return Result<string>.Fail(errors);
            }
            return Result<string>.Success("Password changed successfully.");
        }

        public async Task<Result<string>> DeactivateAccount(Guid userId, DeactivateAccountDto dto)
        {
            var user = await _userManager.FindByIdAsync(userId.ToString());
            if (user == null)
                return Result<string>.Fail("User does not exist.");

            var result = await _userManager.CheckPasswordAsync(user, dto.Password);
            if (!result)
                return Result<string>.Fail("Password is incorrect.");
            
            user.AccountStatus = AccountStatus.Inactive;
            var updateResult = await _userManager.UpdateAsync(user);
            if (!updateResult.Succeeded)
            {
                var errors = string.Join(", ", updateResult.Errors.Select(e => e.Description));
                return Result<string>.Fail(errors);
            }
            return Result<string>.Success("Deactivation completed");
        }
    }
}
