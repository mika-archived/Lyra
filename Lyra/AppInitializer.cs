using System;
using System.Data.SQLite;
using System.Diagnostics;
using System.IO;
using System.Linq;

using Lyra.Models;

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