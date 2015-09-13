using System;
using System.Data.SQLite;
using System.Diagnostics;
using System.IO;
using System.Linq;

using Lyra.Models;
using Lyra.Models.Database.Repositories;
using Lyra.Views;

namespace Lyra
{
    public delegate void ApplicationOnExit();

    public static class AppInitializer
    {
        private static string[] _commandLineArgs;

        private static SplashWindow _splashWindow;

        public static event ApplicationOnExit OnExit;

        public static void PreInitialize()
        {
            _commandLineArgs = Environment.GetCommandLineArgs();

            // ポータブルモード
            LyraApp.IsPortable = _commandLineArgs.Contains("-portable");
            Debug.WriteLine("IsPortable = " + LyraApp.IsPortable);
            Debug.WriteLine("AppRootDir = " + LyraApp.RootDirectory);

            Directory.CreateDirectory(LyraApp.RootDirectory);

            // Show splash window
            _splashWindow = new SplashWindow();
            _splashWindow.Show();

            AppDomain.CurrentDomain.SetData("DataDirectory", LyraApp.RootDirectory);

            InitializeDatabase();
        }

        public static void Initialize()
        {
            var dbContext = new AppRepository();
            if (!dbContext.Artists.Contains(w => w.Name == "Unknown"))
                LyraApp.DatabaseUnknownArtist = dbContext.Artists.Add(new Artist { Name = "Unknown" });
            else
                LyraApp.DatabaseUnknownArtist = dbContext.Artists.Find(w => w.Name == "Unknown").First();

            if (!dbContext.Albums.Contains(w => w.Title == "Unknown"))
                LyraApp.DatabaseUnknownAlbum = dbContext.Albums.Add(new Album { Title = "Unknown" });
            else
                LyraApp.DatabaseUnknownAlbum = dbContext.Albums.Find(w => w.Title == "Unknown").First();
        }

        public static void PostInitialize()
        {
            _splashWindow.Close();
        }

        public static void UnInitialize()
        {
            OnExit?.Invoke();
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
                                          "Artwork BLOB);"; // Artwork NONE
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