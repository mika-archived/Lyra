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

        public static string ButtonVolume0 => "\uE992";

        public static string ButtonVolume1 => "\uE993";

        public static string ButtonVolume2 => "\uE994";

        public static string ButtonVolume3 => "\uE995";

        public static string ButtonNoRepeat => "\uE0AD";

        public static string ButtonRepeatOnce => "\uE1CC";

        public static string ButtonRepeat => "\uE1CD";

        #endregion
    }
}