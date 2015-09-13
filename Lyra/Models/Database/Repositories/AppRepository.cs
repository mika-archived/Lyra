namespace Lyra.Models.Database.Repositories
{
    public class AppRepository
    {
        private readonly TrackDbContext _trackDbContext;
        private readonly LocationDbContext _locationDbContext;

        public DbRepository<Track> Tracks { get; }
        public DbRepository<Album> Albums { get; }
        public DbRepository<Artist> Artists { get; }
        public DbRepository<Location> Locations { get; }

        public AppRepository()
        {
            this._trackDbContext = new TrackDbContext();
            this._locationDbContext = new LocationDbContext();

            this.Tracks = new DbRepository<Track>(this._trackDbContext);
            this.Albums = new DbRepository<Album>(this._trackDbContext);
            this.Artists = new DbRepository<Artist>(this._trackDbContext);
            this.Locations = new DbRepository<Location>(this._locationDbContext);
        }

        public void SaveChanges()
        {
            this._trackDbContext.SaveChanges();
            this._locationDbContext.SaveChanges();
        }
    }
}