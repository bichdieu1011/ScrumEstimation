using System.ComponentModel.DataAnnotations;

namespace ScrumEstimationServerApp.Entity
{
    public class Ticket
    {
        [Key]
        public long Id { get; set; }
        public string Room { get; set; }
        public string Name { get; set; }
        public decimal? Point { get; set; }
        public bool IsPick { get; set; }
    }
}
