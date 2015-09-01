using Livet;

namespace Lyra.Models
{
    // +--------------------------------------------------------------+
    // | Track                                                        |
    // +--------------+-----------------------------------------------+
    // | id           | INTEGER PRIMARY KEY AUTOINCREMENT             |
    // | path         | TEXT UNIQUE                                   |
    // | number       | INTEGER                                       |
    // | title        | TEXT                                          |
    // | artist_id    | INTEGER  FOREIGN KEY REFERENCES artists(id)   |
    // | album_id     | INTEGER  FOREIGN KEY REFERENCES albums(id)    |
    // | duration     | INTEGER                                       |
    // +--------------+-----------------------------------------------+
    public class Track : NotificationObject
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

        #region Path変更通知プロパティ

        private string _Path;

        public string Path
        {
            get
            { return _Path; }
            set
            {
                if (_Path == value)
                    return;
                _Path = value;
                RaisePropertyChanged();
            }
        }

        #endregion

        #region Number変更通知プロパティ

        private int _Number;

        public int Number
        {
            get
            { return _Number; }
            set
            {
                if (_Number == value)
                    return;
                _Number = value;
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

        // FOREIGN KEY REFERENCES artists(id)
        public int ArtistId { get; set; }

        #region Artist変更通知プロパティ

        private Artist _Artist;

        public virtual Artist Artist
        {
            get
            { return _Artist; }
            set
            {
                if (_Artist == value)
                    return;
                _Artist = value;
                RaisePropertyChanged();
            }
        }

        #endregion

        // FOREIGN KEY REFERENCES akbums(id)
        public int AlbumId { get; set; }

        #region Album変更通知プロパティ

        private Album _Album;

        public virtual Album Album
        {
            get
            { return _Album; }
            set
            {
                if (_Album == value)
                    return;
                _Album = value;
                RaisePropertyChanged();
            }
        }

        #endregion

        #region Duration変更通知プロパティ

        // 単位：ms(1s -> 1000m)

        private int _Duration;

        public int Duration
        {
            get
            { return _Duration; }
            set
            {
                if (_Duration == value)
                    return;
                _Duration = value;
                RaisePropertyChanged();
            }
        }

        #endregion
    }
}