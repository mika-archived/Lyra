using Livet;

namespace Lyra.Models.Settings
{
    /// <summary>
    /// アプリケーションの全体的な設定
    /// </summary>
    public class CommonSettings : NotificationObject
    {
        /// <summary>
        /// -portable 引数無しで起動した場合でも、引数ありで起動した時と同様に扱うようにします。
        /// </summary>
        public static LyraSettings<bool> IsPortableOnly => new LyraSettings<bool>(nameof(IsPortableOnly));

        /// <summary>
        /// 起動時に、 application.db のバックアップを作成します。
        /// </summary>
        public static LyraSettings<bool> EnableBackupDatabase => new LyraSettings<bool>(nameof(EnableBackupDatabase), true);
    }
}