namespace Lyra.Models
{
    // +--------------------------------------------------+
    // | Location                                         |
    // +--------------+-----------------------------------+
    // | id           | INTEGER PRIMARY KEY AUTOINCREMENT |
    // | path         | TEXT UNIQUE                       |
    // +--------------+-----------------------------------+
    public class Location
    {
        public int Id { get; set; }

        public string Path { get; set; }
    }
}