using System;

using Livet;

namespace Lyra.Extensions
{
    // 参考：http://qiita.com/Temarin_PITA/items/efc1975d3e83287d8891
    public static class ViewModelEx
    {
        // いえーい
        public static T AddTo<T>(this T disposable, ViewModel viewModel) where T : IDisposable
        {
            if (viewModel == null)
                throw new ArgumentNullException(nameof(viewModel));
            viewModel.CompositeDisposable.Add(disposable);
            return disposable;
        }
    }
}