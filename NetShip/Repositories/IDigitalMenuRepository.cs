using NetShip.Entities;
using System;
using System.Threading.Tasks;

namespace NetShip.Repositories
{
    public interface IDigitalMenuRepository
    {
        Task CreateFeedbackAsync(Feedback feedback);
        Task TrackDeviceAsync(DeviceTracking deviceTracking);
    }
}
