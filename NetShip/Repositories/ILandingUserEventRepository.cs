using NetShip.Entities;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace NetShip.Repositories
{
    public interface ILandingUserEventRepository
    {
        Task<Guid> Create(LandingUserEvent landingUserEvent);
        Task<List<LandingUserEvent>> GetAll();
    }
}
