using Livet;

namespace Lyra.Models
{
    /// <summary>
    /// トラック情報
    /// </summary>
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

        #region Info変更通知プロパティ

        private TrackInfo _Info;

        public TrackInfo Info
        {
            get
            { return _Info; }
            set
            {
                if (_Info == value)
                    return;
                _Info = value;
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

        #region Artist変更通知プロパティ

        private string _Artist;

        public string Artist
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

        #region Album変更通知プロパティ

        private string _Album;

        public string Album
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

        #region Jacket変更通知プロパティ

        private string _Jacket;

        public string Jacket
        {
            get
            { return _Jacket; }
            set
            {
                if (_Jacket == value)
                    return;
                _Jacket = value;
                RaisePropertyChanged();
            }
        }

        #endregion

        #region Status変更通知プロパティ

        private TrackState _Status;

        public TrackState Status
        {
            get
            { return _Status; }
            set
            {
                if (_Status == value)
                    return;
                _Status = value;
                RaisePropertyChanged();
            }
        }

        #endregion

        #region FilePath変更通知プロパティ

        private string _FilePath;

        public string FilePath
        {
            get
            { return _FilePath; }
            set
            {
                if (_FilePath == value)
                    return;
                _FilePath = value;
                RaisePropertyChanged();
            }
        }

        #endregion
    }
}