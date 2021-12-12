using System.ComponentModel.DataAnnotations;

namespace ScrumEstimationWeb.Server.Entity
{
    public class Room
    {
        [Key]
        public  string Id { get; set; }
        public string Name { get; set; }
    }
}
