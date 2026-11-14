using Cs_Hub.Domain.entities.Users;
using System;
using System.Collections.Generic;
using System.Text;

namespace Cs_Hub.Domain.RepositoryContracts
{
    public interface IUserProfileRepository
    {
        Task AddAsync(UserProfile userProfile);
        Task<UserProfile?> GetProfileById(Guid id);
        Task<UserProfile?> GetProfileByName(string name);
        void Update(UserProfile userProfile);
    }
}
