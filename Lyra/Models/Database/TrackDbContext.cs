using System.Data.Entity;

namespace Lyra.Models.Database
{
    public class TrackDbContext : DbContext
    {
        public TrackDbContext() : base(DatabaseConnectionProvider.GetConnection(), true)
        {
            // Disable Migration by Entity Framework
            System.Data.Entity.Database.SetInitializer<TrackDbContext>(null);
        }

        // -- Tracks --
        public DbSet<Track> Tracks { get; set; }

        public DbSet<Artist> Artists { get; set; }

        public DbSet<Album> Albums { get; set; }
    }
}