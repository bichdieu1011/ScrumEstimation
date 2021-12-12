using System.ComponentModel.DataAnnotations;

namespace ScrumEstimationServerApp.Entity
{
    public class Attendee
    {
        [Key]
        public long Id { get; set; }
        public string Room { get; set; }
        public string username { get; set; }

        public bool IsActive { get; set; } = true;
    }
}
