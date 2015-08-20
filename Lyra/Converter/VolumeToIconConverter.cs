using System;
using System.Globalization;
using System.Windows.Data;

using Lyra.Models;

namespace Lyra.Converter
{
    /// <summary>
    /// 入力されたボリューム値から、それにあったボリュームアイコンへ変換します。
    /// </summary>
    public class VolumeToIconConverter : IValueConverter
    {
        /// <summary>
        /// 値を変換します。
        /// </summary>
        /// <returns>
        /// 変換された値。メソッドが null を返す場合は、有効な null 値が使用されています。
        /// </returns>
        /// <param name="value">バインディング ソースによって生成された値。</param><param name="targetType">バインディング ターゲット プロパティの型。</param><param name="parameter">使用するコンバーター パラメーター。</param><param name="culture">コンバーターで使用するカルチャ。</param>
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            var volume = value as float?;
            if (volume == null)
                throw new ArgumentException(nameof(value) + "is must be float(System.Single).");

            if (volume == 0)
                return LyraApp.ButtonVolumeMute;
            else if (0 < volume && volume <= 33)
                return LyraApp.ButtonVolume1;
            else if (33 < volume && volume <= 66)
                return LyraApp.ButtonVolume2;
            else
                return LyraApp.ButtonVolume3;
        }

        /// <summary>
        /// 値を変換します。
        /// </summary>
        /// <returns>
        /// 変換された値。メソッドが null を返す場合は、有効な null 値が使用されています。
        /// </returns>
        /// <param name="value">バインディング ターゲットによって生成される値。</param><param name="targetType">変換後の型。</param><param name="parameter">使用するコンバーター パラメーター。</param><param name="culture">コンバーターで使用するカルチャ。</param>
        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}