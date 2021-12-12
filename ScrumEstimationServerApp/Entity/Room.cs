using System.ComponentModel.DataAnnotations;

namespace ScrumEstimationServerApp.Entity
{
    public class Room
    {
        [Key]
        public  string Id { get; set; }
        public string Name { get; set; }
    }
}
