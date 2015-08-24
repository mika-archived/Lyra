using System;
using System.Data.Common;
using System.Data.SQLite;
using System.Diagnostics;
using System.IO;
using System.Linq;

using Lyra.Models;
using Lyra.Models.Database;

namespace Lyra
{
    public static class AppInitializer
    {
        private static string[] _commandLineArgs;

        public static void PreInitialize()
        {
            _commandLineArgs = Environment.GetCommandLineArgs();

            // ポータブルモード
            LyraApp.IsPortable = _commandLineArgs.Contains("-portable");
            Debug.WriteLine("IsPortable = " + LyraApp.IsPortable);
            Debug.WriteLine("AppRootDir = " + LyraApp.RootDirectory);

            Directory.CreateDirectory(LyraApp.RootDirectory);

            InitializeDatabase();
        }

        public static void Initialize()
        {
            var connection = DbProviderFactories.GetFactory(LyraApp.DatabaseProvider).CreateConnection();
            // ReSharper disable once PossibleNullReferenceException
            connection.ConnectionString = LyraApp.DatabaseConnectionString;

            using (var dbContext = new AppDbContext(connection))
            {
                if (!dbContext.Artists.Any(w => w.Name == "Unknown"))
                    LyraApp.DatabaseUnknownArtist = dbContext.Artists.Add(new Artist { Name = "Unknown" }).Id;
                else
                    LyraApp.DatabaseUnknownArtist = dbContext.Artists.Single(w => w.Name == "Unknown").Id;

                if (!dbContext.Albums.Any(w => w.Title == "Unknown"))
                    LyraApp.DatabaseUnknownAlbum = dbContext.Albums.Add(new Album { Title = "Unknown" }).Id;
                else
                    LyraApp.DatabaseUnknownAlbum = dbContext.Albums.Single(w => w.Title == "Unknown").Id;

                dbContext.SaveChanges();
            }
        }

        public static void PostInitialize()
        {
        }

        private static void InitializeDatabase()
        {
            // System.Data.SQLite;
            // EntityFramework "CodeFirst" is not available?
            using (var connection = new SQLiteConnection(LyraApp.DatabaseConnectionString))
            {
                using (var command = new SQLiteCommand(connection))
                {
                    connection.Open();

                    command.CommandText = "CREATE TABLE IF NOT EXISTS Tracks(" +
                                          "Id INTEGER PRIMARY KEY AUTOINCREMENT," +
                                          "Path TEXT UNIQUE," +
                                          "Number INTEGER," +
                                          "Title TEXT," +
                                          "ArtistId INTEGER," +
                                          "AlbumId INTEGER," +
                                          "Duration INTEGER," +
                                          "FOREIGN KEY(ArtistId) REFERENCES Artists(Id)," +
                                          "FOREIGN KEY(AlbumId) REFERENCES Albums(Id));";
                    command.ExecuteNonQuery();

                    command.CommandText = "CREATE TABLE IF NOT EXISTS Artists(" +
                                          "Id INTEGER PRIMARY KEY AUTOINCREMENT," +
                                          "Name TEXT UNIQUE);";
                    command.ExecuteNonQuery();

                    command.CommandText = "CREATE TABLE IF NOT EXISTS Albums(" +
                                          "Id INTEGER PRIMARY KEY AUTOINCREMENT," +
                                          "Title TEXT UNIQUE," +
                                          "Artwork TEXT);";
                    command.ExecuteNonQuery();

                    command.CommandText = "CREATE TABLE IF NOT EXISTS Locations(" +
                                          "Id INTEGER PRIMARY KEY AUTOINCREMENT," +
                                          "Path TEXT UNIQUE);";
                    command.ExecuteNonQuery();
                }
            }
        }
    }
}