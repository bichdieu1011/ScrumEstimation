using System.ComponentModel.DataAnnotations;

namespace ScrumEstimationWeb.Server.Entity
{
    public class Estimation
    {
        [Key]
        public long Id { get; set; }
        public long userId { get; set; }
        public long ticketId { get; set; }
        public decimal? point { get; set; }
    }
}
