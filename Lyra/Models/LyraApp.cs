using System;
using System.IO;

namespace Lyra.Models
{
    public static class LyraApp
    {
        #region BASS for .NET

        public static string BassNetMailAddress => "mikazuki_fuyuno@outlook.com";

        public static string BassNetRegistrationKey => "2X2229242724822";

        public static string BassNetModuleDir => Path.Combine(Directory.GetCurrentDirectory(), "assemblies");

        #endregion

        #region SoundCloud

        public static string ClientId => "029b2b2ce29449a7c0dcf5be8ade4d1f";

        public static string ClientSecret => "9e46cfeea65e14b67c776de4f9b85e91";

        #endregion

        #region Icons

        public static string ButtonPlay => "\uE102";

        public static string ButtonPause => "\uE103";

        public static string ButtonVolumeMute => "\uE198";

        public static string ButtonVolume0 => "\uE992";

        public static string ButtonVolume1 => "\uE993";

        public static string ButtonVolume2 => "\uE994";

        public static string ButtonVolume3 => "\uE995";

        public static string ButtonNoRepeat => "\uE0AD";

        public static string ButtonRepeatOnce => "\uE1CC";

        public static string ButtonRepeat => "\uE1CD";

        #endregion

        #region File / Directory Path

        #region Launch Mode

        private static bool? _isPortable;

        /// <summary>
        /// 起動モードを設定/取得します。
        /// なお、設定は最初の１度のみ可能です(２度め以降は無視されます)。
        /// </summary>
        public static bool IsPortable
        {
            set
            {
                if (_isPortable == null)
                    _isPortable = value;
            }
            get
            {
                if (_isPortable.HasValue)
                    return _isPortable.Value;
                throw new InvalidOperationException(nameof(IsPortable));
            }
        }

        #endregion

        public static string RootDirectory => IsPortable
            ? Path.Combine(Directory.GetCurrentDirectory(), "apps")
            : Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), "mkzk.xyz", "Lyra");

        public static string DatabaseFilePath => Path.Combine(RootDirectory, "application.db");

        #endregion

        #region Database

        public static string DatabaseProvider => "System.Data.SQLite";

        public static string DatabaseConnectionString => $"Data Source={DatabaseFilePath};foreign keys=true";

        public static int DatabaseUnknownArtist { get; set; }

        public static int DatabaseUnknownAlbum { get; set; }

        #endregion
    }
}