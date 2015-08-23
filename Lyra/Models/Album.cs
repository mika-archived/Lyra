using System.Collections.Generic;

using Livet;

namespace Lyra.Models
{
    // +--------------------------------------------------+
    // | Album                                            |
    // +--------------+-----------------------------------+
    // | id           | INTEGER PRIMARY KEY AUTOINCREMENT |
    // | title        | TEXT UNIQUE                       |
    // | artwork      | TEXT                              |
    // +--------------+-----------------------------------+
    public sealed class Album : NotificationObject
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

        #region Title変更通知プロパティ

        private string _Title;

        public string Title
        {
            get
            { return _Title; }
            set
            {
                if (_Title == value)
                    return;
                _Title = value;
                RaisePropertyChanged();
            }
        }

        #endregion

        #region Artwork変更通知プロパティ

        private string _Artwork;

        public string Artwork
        {
            get
            { return _Artwork; }
            set
            {
                if (_Artwork == value)
                    return;
                _Artwork = value;
                RaisePropertyChanged();
            }
        }

        #endregion

        public ICollection<Track> Tracks { get; set; }
    }
}