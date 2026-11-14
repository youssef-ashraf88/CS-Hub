using Cs_Hub.Domain.entities.Users;
using Cs_Hub.Domain.Enums;
using Cs_Hub.Domain.RepositoryContracts;
using Cs_Hub.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;

namespace Cs_Hub.Infrastructure.Repositories
{
    public class UserProfileRepository : IUserProfileRepository
    {
        private readonly ApplicationDbContext _db;

        public UserProfileRepository(ApplicationDbContext db)
        {
            _db = db;
        }

        public Task AddAsync(UserProfile userProfile)
        {
            _db.UserProfiles.AddAsync(userProfile);
            return Task.CompletedTask;
        }

        public async Task<UserProfile?> GetProfileById(Guid id)
        {
            return await _db.UserProfiles
                .Include(p => p.User)
                .FirstOrDefaultAsync(p => p.UserId == id);
        }

        public async Task<UserProfile?> GetProfileByName(string name)
        {
            return await _db.UserProfiles
                .AsNoTracking()
                .Include(p => p.User)
                .FirstOrDefaultAsync(p => p.User.UserName == name && p.User.AccountStatus == AccountStatus.Active);
        }

        public void Update(UserProfile userProfile)
        {
            _db.UserProfiles.Update(userProfile);
        }
    }
}
