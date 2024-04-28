using Microsoft.EntityFrameworkCore;
using NetShip.Entities;
using System.Threading.Tasks;

namespace NetShip.Repositories
{
    public class DigitalMenuRepository : IDigitalMenuRepository
    {
        private readonly ApplicationDbContext context;

        public DigitalMenuRepository(ApplicationDbContext context)
        {
            this.context = context;
        }

        public async Task CreateFeedbackAsync(Feedback feedback)
        {
            context.Feedbacks.Add(feedback);
            await context.SaveChangesAsync();
        }

        public async Task TrackDeviceAsync(DeviceTracking deviceTracking)
        {
            context.DeviceTrackings.Add(deviceTracking);
            await context.SaveChangesAsync();
        }
    }
}
