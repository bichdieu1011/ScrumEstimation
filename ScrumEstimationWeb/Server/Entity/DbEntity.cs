using Microsoft.EntityFrameworkCore;

namespace ScrumEstimationWeb.Server.Entity
{
    public class DbEntity : DbContext
    {

        private readonly string _connectionString;


        public DbEntity(string connectionString)
        {
            this._connectionString = connectionString;
        }

        public DbEntity(DbContextOptions<DbEntity> options) : base(options)
        {
        }


        public virtual DbSet<Room> Room { get; set; }
        public virtual DbSet<Ticket> Ticket { get; set; }

        public virtual DbSet<Attendee> Attendee { get; set; }
        public virtual DbSet<Estimation> Estimation { get; set; }
    }
}
