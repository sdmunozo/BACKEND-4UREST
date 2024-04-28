using System;

namespace NetShip.Entities
{
    public class LandingUserEvent
    {
        public Guid Id { get; set; }
        public Guid UserId { get; set; }
        public string SessionId { get; set; }
        public string EventType { get; set; }
        public DateTime EventTimestamp { get; set; }
        public EventDetails Details { get; set; }
    }

    public class EventDetails
    {
        public int? PresentationViewSecondsElapsed { get; set; }
        public int? MenuHighlightsViewSecondsElapsed { get; set; }
        public int? MenuScreensViewSecondsElapsed { get; set; }
        public int? ForWhoViewSecondsElapsed { get; set; }
        public int? WhyUsViewSecondsElapsed { get; set; }
        public int? SuscriptionsViewSecondsElapsed { get; set; }
        public int? TestimonialsViewSecondsElapsed { get; set; }
        public int? FaqViewSecondsElapsed { get; set; }
        public int? TrustElementsViewSecondsElapsed { get; set; }
        public string LinkDestination { get; set; }
        public string LinkLabel { get; set; }
        public int? PlaybackTime { get; set; }
        public int? Duration { get; set; }
        public string ImageId { get; set; }
        public string FAQId { get; set; }
        public string Status { get; set; }
    }
}
