using Cs_Hub.Application.DTO.Profile;
using Cs_Hub.Application.Helpers;
using System;
using System.Collections.Generic;
using System.Text;

namespace Cs_Hub.Application.ServiceContracts
{
    public interface IUserProfileService
    {
        Task<Result<MyProfileDto>> GetMyProfile(Guid id);
        Task<Result<string>> UpdateMyProfile(Guid id, UpdateProfileDto updateProfileDto);
        Task<Result<PublicProfileDto>> GetPublicProfile(string name);
    }
}
