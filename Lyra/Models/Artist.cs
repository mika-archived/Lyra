using System.Collections.Generic;

using Livet;

namespace Lyra.Models
{
    // +--------------------------------------------------+
    // | Artist                                           |
    // +--------------+-----------------------------------+
    // | id           | INTEGER PRIMARY KEY AUTOINCREMENT |
    // | name         | TEXT UNIQUE                       |
    // +--------------+-----------------------------------+
    public sealed class Artist : NotificationObject
    {
        #region Id変更通知プロパティ

        private int _Id;

        public int Id
        {
            get
            { return _Id; }
            set
            {
                if (_Id == value)
                    return;
                _Id = value;
                RaisePropertyChanged();
            }
        }

        #endregion

        #region Name変更通知プロパティ

        private string _Name;

        public string Name
        {
            get
            { return _Name; }
            set
            {
                if (_Name == value)
                    return;
                _Name = value;
                RaisePropertyChanged();
            }
        }

        #endregion

        public ICollection<Track> Tracks { get; set; }
    }
}