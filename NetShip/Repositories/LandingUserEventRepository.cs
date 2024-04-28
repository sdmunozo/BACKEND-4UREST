using Microsoft.EntityFrameworkCore;
using NetShip.Entities;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace NetShip.Repositories
{
    public class LandingUserEventRepository : ILandingUserEventRepository
    {
        private readonly ApplicationDbContext _context;

        public LandingUserEventRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<Guid> Create(LandingUserEvent landingUserEvent)
        {
            _context.LandingUserEvents.Add(landingUserEvent);
            await _context.SaveChangesAsync();
            return landingUserEvent.Id;
        }

        public async Task<List<LandingUserEvent>> GetAll()
        {
            return await _context.LandingUserEvents
                .Include(e => e.Details)
                .ToListAsync();
        }
    }
}
