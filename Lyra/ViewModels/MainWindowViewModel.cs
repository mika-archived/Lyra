using Livet;

namespace Lyra.ViewModels
{
    public class MainWindowViewModel : ViewModel
    {
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

        #endregion Title変更通知プロパティ

        public void Initialize()
        {
            this.Title = "Lyra";
        }
    }
}