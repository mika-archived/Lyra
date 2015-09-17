using System.Collections.Generic;
using System.IO;
using System.Xml;
using System.Xml.Serialization;

using Livet;

namespace Lyra.Models.Settings
{
    /// <summary>
    /// Lyra Settings
    /// 変更通知を行う設定の値を設定/取得します。
    /// </summary>
    public class LyraSettings<T> : NotificationObject
    {
        #region Value変更通知プロパティ

        private T _Value;

        public T Value
        {
            get
            { return _Value; }
            set
            {
                if (Equals(_Value, value))
                    return;
                _Value = value;
                this.Save();
                RaisePropertyChanged();
            }
        }

        #endregion

        public string Key { get; }

        private readonly T _defValue;
        private static readonly LyraSettingsDict Dict = new LyraSettingsDict();

        public LyraSettings(string key, T defValue = default(T))
        {
            this.Key = key;
            this._defValue = defValue;
            this.Load();
        }

        private void Save()
        {
            Dict.SetValue(this.Key, this.Value);
        }

        private void Load()
        {
            this.Value = (T)Dict.TryGetValue(this.Key, this._defValue);
        }

        private class LyraSettingsDict
        {
            private readonly Dictionary<string, object> _settingsDictionary;

            public LyraSettingsDict()
            {
                this._settingsDictionary = new Dictionary<string, object>();
                this.Load();
            }

            private void Save()
            {
                using (var sw = new StreamWriter(LyraApp.ConfigurationFilePath))
                {
                    var settings = new XmlWriterSettings
                    {
                        Indent = true,
                        IndentChars = "    "
                    };
                    using (var writer = XmlWriter.Create(sw, settings))
                    {
                        writer.WriteStartElement("LyraSettings");
                        foreach (var kvp in this._settingsDictionary)
                        {
                            writer.WriteStartElement("KeyValuePair");
                            // Key
                            writer.WriteStartElement("Key");
                            writer.WriteValue(kvp.Key);
                            writer.WriteEndElement();

                            // Valye
                            writer.WriteStartElement("Value");
                            using (var strw = new StringWriter())
                            {
                                var serializer = new XmlSerializer(kvp.Value.GetType());
                                serializer.Serialize(strw, kvp.Value);
                                writer.WriteCData(strw.ToString());
                            }
                            writer.WriteEndElement();
                            writer.WriteEndElement();
                        }
                        writer.WriteEndElement();
                    }
                }
            }

            private void Load()
            {
                if (!File.Exists(LyraApp.ConfigurationFilePath))
                    return;

                using (var sr = new StreamReader(LyraApp.ConfigurationFilePath))
                {
                    var settings = new XmlReaderSettings
                    {
                        IgnoreComments = true,
                        IgnoreWhitespace = true,
                    };
                    using (var reader = XmlReader.Create(sr, settings))
                    {
                        while (reader.Read())
                        {
                            if (!reader.ReadToFollowing("KeyValuePair"))
                                break;
                            reader.ReadToFollowing("Key");
                            var key = reader.ReadString();
                            reader.ReadToFollowing("Value");
                            // Deserialize
                            using (var strr = new StringReader(reader.ReadElementString()))
                            {
                                var serializer = new XmlSerializer(typeof(T));
                                this._settingsDictionary[key] = serializer.Deserialize(strr);
                            }
                        }
                    }
                }
            }

            public object TryGetValue(string key, object defValue)
            {
                object value;
                this._settingsDictionary.TryGetValue(key, out value);
                if (value == null)
                    return defValue;
                return value;
            }

            public void SetValue(string key, object value)
            {
                this._settingsDictionary[key] = value;
                this.Save();
            }
        }
    }
}