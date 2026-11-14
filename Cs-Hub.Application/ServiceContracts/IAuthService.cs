using Cs_Hub.Application.DTO;
using Cs_Hub.Application.DTO.Profile;
using Cs_Hub.Application.Helpers;
using System;
using System.Collections.Generic;
using System.Text;

namespace Cs_Hub.Application.ServiceContracts
{
    public interface IAuthService
    {
        Task<Result<RegisterResponseDTO>> Register(RegisterDTO registerDto);
        Task<Result<string>> ConfirmEmail(string userId, string token);
        Task<Result<LoginResponseDTO>> Login(LoginDTO loginDto);
        Task<Result<string>> ChangePassword(Guid userId, ChangePasswordDto dto);
        Task<Result<string>> DeactivateAccount(Guid userId, DeactivateAccountDto dto);
    }
}
