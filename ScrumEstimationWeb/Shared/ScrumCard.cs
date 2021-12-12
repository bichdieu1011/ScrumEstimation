using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ScrumEstimationWeb.Shared
{
    public class ScrumCard
    {
        public decimal Point { get; set; }
        public string Name { get; set; }
        public string Img { get; set; }
    }

    public class RoomModel
    {
        public string Id { get; set; }
        public string Name { get; set; }
    }

    public class TicketModel
    {
        public long Id { get; set; }
        public string TicketName { get; set; }
        public decimal? Point { get; set; }
        public bool IsPick { get; set; }
    }

    public class UserModel
    {
        public string UserName { get; set; }
        public bool IsEstimate { get; set; }
        public decimal? Point { get; set; }

    }


    public class EstimatedInfo
    {
        public decimal? Point { get; set; }

        public List<UserModel> EstimatedPoints { get; set; }
    }

    public class JoinedName
    {
        private static List<string> UserNames
        {
            get
            {
                return new List<string> { "Aaron", "Abbott", "Abel", "Abner", "Abraham", "Adam", "Addison", "Adler", "Adley", "Adrian", "Aedan",
                "Abigail", "Ada", "Adelaide", "Adrienne", "Agatha", "Agnes", "Aileen", "Aimee", "Alanna", "Alarice", "Alda" ,"Donald","Steven",
                "Andrew","Joshua","Kenneth","Brian","Nicholas","Bryan","Joe","Jordan","Billy","Bruce","Albert","Willie","Gabriel","Logan","Alan",
                "Wayne","Juan","Randy","Ralph","Johnny","Philip","Louis","Elijah","Russell","Patrick","Alexander","Frank"
                };
            }
        }

        public static string RandomArrayEntries
        {
            get
            {
                Random rnd = new Random();
                int index = rnd.Next(UserNames.Count);
                return UserNames[index];
            }
        }
    }
}
