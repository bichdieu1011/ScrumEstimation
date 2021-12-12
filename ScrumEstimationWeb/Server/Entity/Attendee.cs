using System.ComponentModel.DataAnnotations;

namespace ScrumEstimationWeb.Server.Entity
{
    public class Attendee
    {
        [Key]
        public long Id { get; set; }
        public string Room { get; set; }
        public string username { get; set; }
    }
}
