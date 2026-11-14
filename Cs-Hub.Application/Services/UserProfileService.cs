using AutoMapper;
using Cs_Hub.Application.DTO.Profile;
using Cs_Hub.Application.Helpers;
using Cs_Hub.Application.ServiceContracts;
using Cs_Hub.Domain.RepositoryContracts;
using System;
using System.Collections.Generic;
using System.Text;

namespace Cs_Hub.Application.Services
{
    public class UserProfileService : IUserProfileService
    {
        private readonly IUserProfileRepository _userProfileRepository;
        private readonly IMapper _mapper;
        private readonly ITransaction _transaction;

        public UserProfileService(IUserProfileRepository userProfileRepository, IMapper mapper, ITransaction transaction)
        {
            _userProfileRepository = userProfileRepository;
            _mapper = mapper;
            _transaction = transaction;
        }

        public async Task<Result<MyProfileDto>> GetMyProfile(Guid id)
        {
            var profile = await _userProfileRepository.GetProfileById(id);
            if(profile == null)
                return Result<MyProfileDto>.Fail("Profile not found");

            var myProfileDto = _mapper.Map<MyProfileDto>(profile);
            return Result<MyProfileDto>.Success(myProfileDto);
        }

        public async Task<Result<PublicProfileDto>> GetPublicProfile(string name)
        {
            var profile = await _userProfileRepository.GetProfileByName(name);
            if( profile == null)
                return Result<PublicProfileDto>.Fail("Profile not found");

            var userProfileResponse = _mapper.Map<PublicProfileDto>(profile);
            return Result<PublicProfileDto>.Success(userProfileResponse);
        }

        public async Task<Result<string>> UpdateMyProfile(Guid id, UpdateProfileDto updateProfileDto)
        {
            await _transaction.BeginTransaction();
            var profile = await _userProfileRepository.GetProfileById(id);
            if (profile == null)
                return Result<string>.Fail("Profile not found");

            profile.DisplayName = updateProfileDto.DisplayName.Trim();
            profile.HeadLine = string.IsNullOrWhiteSpace(updateProfileDto.HeadLine) ? null : updateProfileDto.HeadLine.Trim();
            profile.Bio = string.IsNullOrWhiteSpace(updateProfileDto.Bio) ? null : updateProfileDto.Bio.Trim();
            profile.Location = string.IsNullOrWhiteSpace(updateProfileDto.Location) ? null : updateProfileDto.Location.Trim();
            profile.WebsiteUrl = string.IsNullOrWhiteSpace(updateProfileDto.WebsiteUrl) ? null : updateProfileDto.WebsiteUrl.Trim();
            
            await _transaction.CommitTransaction();
            return Result<string>.Success("Profile updated successfully");
        }
    }
}
