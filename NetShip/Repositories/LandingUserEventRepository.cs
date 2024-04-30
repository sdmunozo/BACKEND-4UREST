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
            var existingEvent = await _context.LandingUserEvents
                .FirstOrDefaultAsync(e => e.UserId == landingUserEvent.UserId
                                          && e.SessionId == landingUserEvent.SessionId
                                          && e.EventType == landingUserEvent.EventType);

            if (existingEvent != null)
            {
                // Actualiza los campos existentes
                existingEvent.EventTimestamp = landingUserEvent.EventTimestamp;
                existingEvent.Details = landingUserEvent.Details;
                _context.LandingUserEvents.Update(existingEvent);
            }
            else
            {
                // Crea un nuevo registro si no existe uno correspondiente
                _context.LandingUserEvents.Add(landingUserEvent);
            }

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


/*

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


*/