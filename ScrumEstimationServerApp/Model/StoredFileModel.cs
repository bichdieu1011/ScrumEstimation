using ScrumEstimationServerApp.Entity;

namespace ScrumEstimationServerApp.Model
{
    public class StoredFileModel
    {
        public Room Room { get; set; }
        public List<Attendee> Attendees { get; set; }
        public List<Estimation> Estimations { get; set; }
        public List<Ticket> Tickets { get; set; }
    }
}
