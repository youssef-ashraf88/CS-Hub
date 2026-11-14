using Cs_Hub.Application.DTO;
using Cs_Hub.Domain.entities.Users;
using System;
using System.Collections.Generic;
using System.Text;

namespace Cs_Hub.Application.ServiceContracts
{
    public interface IJwtService
    {
        LoginResponseDTO CreateJwtToken(ApplicationUser user, IEnumerable<string> roles);
    }
}
